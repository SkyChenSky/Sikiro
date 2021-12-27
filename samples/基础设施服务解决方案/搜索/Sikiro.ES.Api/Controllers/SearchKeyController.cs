using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Sikiro.Elasticsearch.Extension;
using Sikiro.ES.Api.Model.SearchKey;
using Sikiro.ES.Api.Model.SearchKey.Entity;
using Sikiro.ES.Api.Model.SearchKey.Enum;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.ES.Api.Controllers
{
    /// <summary>
    /// 作品搜索
    /// </summary>
    [Route("/api/[controller]")]
    public class SearchKeyController : BaseController
    {
        private readonly ElasticClient _elasticClient;

        public SearchKeyController(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// 作品搜索列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public ApiResult<List<SearchKeyGetResponse>> Get(SearchKeyGetRequest request)
        {
            var shouldQuerys = new List<Func<QueryContainerDescriptor<SearchKey>, QueryContainer>>();
            int minimumShouldMatch = 0;
            if (!request.KeyName.IsNullOrWhiteSpace())
            {
                shouldQuerys.Add(a => a.MatchPhrase(m => m.Field("key_name.pinyin").Query(request.KeyName)));
                shouldQuerys.Add(a => a.MatchPhrase(m => m.Field("key_name.standard").Query(request.KeyName)));
                minimumShouldMatch = 1;
            }

            var mustQuerys = new List<Func<QueryContainerDescriptor<SearchKey>, QueryContainer>>
            {
                a => a.Range(t => t.Field(f => f.Weight).GreaterThanOrEquals(0))
            };

            if (request.IsSubsidiary.HasValue)
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.IsSubsidiary).Value(request.IsSubsidiary.Value)));

            if (request.SysTagIds != null && request.SysTagIds.Any())
                mustQuerys.Add(a => a.Terms(t => t.Field(f => f.SysTagId).Terms(request.SysTagIds)));

            if (request.EntityType.HasValue)
            {
                if (request.EntityType.Value == ESearchKey.EntityType.AllNovel)
                {
                    mustQuerys.Add(a => a.Terms(t => t.Field(f => f.EntityType).Terms(ESearchKey.EntityType.Novel, ESearchKey.EntityType.ChatNovel, ESearchKey.EntityType.FanNovel)));
                }
                else
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityType).Value((int)request.EntityType.Value)));
            }

            var sortDescriptor = new SortDescriptor<SearchKey>();
            sortDescriptor = request.Sort == ESearchKey.Sort.Weight
                ? sortDescriptor.Field(f => f.Weight, SortOrder.Descending)
                : sortDescriptor.Field(f => f.ActiveDate, SortOrder.Descending);

            var searchResult = _elasticClient.Search<SearchKey>(a =>
                a.Index(typeof(SearchKey).GetRelationName())
                    .From(request.Size * request.Page)
                    .Size(request.Size)
                    .Query(q => q.Bool(b => b.Should(shouldQuerys).Must(mustQuerys).MinimumShouldMatch(minimumShouldMatch)))
                    .Sort(s => sortDescriptor));

            var apiResult = searchResult.GetApiResult<SearchKey, List<SearchKeyGetResponse>>();

            if (apiResult.Success)
                return apiResult;

            return ApiResult<List<SearchKeyGetResponse>>.IsSuccess("空集合数据");
        }

        /// <summary>
        /// 搜索数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("count")]
        public ApiResult<long> Count(SearchKeyCountRequest request)
        {
            var shouldQuerys = new List<Func<QueryContainerDescriptor<SearchKey>, QueryContainer>>();
            int minimumShouldMatch = 0;
            if (!request.KeyName.IsNullOrWhiteSpace())
            {
                shouldQuerys.Add(a => a.MatchPhrase(m => m.Field("key_name.pinyin").Query(request.KeyName)));
                shouldQuerys.Add(a => a.MatchPhrase(m => m.Field("key_name.standard").Query(request.KeyName)));
                minimumShouldMatch = 1;
            }

            var mustQuerys = new List<Func<QueryContainerDescriptor<SearchKey>, QueryContainer>>
            {
                a => a.Range(t => t.Field(f => f.Weight).GreaterThanOrEquals(0))
            };

            if (request.IsSubsidiary.HasValue)
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.IsSubsidiary).Value(request.IsSubsidiary.Value)));

            if (request.SysTagIds != null && request.SysTagIds.Any())
                mustQuerys.Add(a => a.Terms(t => t.Field(f => f.SysTagId).Terms(request.SysTagIds)));

            if (request.EntityType.HasValue)
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityType).Value((int)request.EntityType.Value)));

            var searchResult = _elasticClient.Count<SearchKey>(a =>
                a.Index(typeof(SearchKey).GetRelationName())
                    .Query(q => q.Bool(b => b.Should(shouldQuerys).Must(mustQuerys).MinimumShouldMatch(minimumShouldMatch))));

            return ApiResult<long>.IsSuccess(searchResult.Count);
        }

        /// <summary>
        /// 批量新增作品搜索列表(返回创建的indexName)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult Post(SearchKeyPostRequest request)
        {
            if (!request.Items.Any())
                return ApiResult.IsFailed("无传入数据");

            var date = DateTime.Now;
            var relationName = typeof(SearchKey).GetRelationName();
            var indexName = request.IndexName.IsNullOrWhiteSpace() ? (relationName + "-" + date.ToString("yyyyMMddHHmmss")) : request.IndexName;

            if (request.IndexName.IsNullOrWhiteSpace())
            {
                var createResult = _elasticClient.Indices.Create(indexName,
                    a =>
                        a.Map<SearchKey>(m => m.AutoMap().Properties(p =>
                            p.Custom(new TextProperty
                            {
                                Name = "key_name",
                                Analyzer = "standard",
                                Fields = new Properties(new Dictionary<PropertyName, IProperty>
                                {
                                    { new PropertyName("pinyin"),new TextProperty{ Analyzer = "pinyin"} },
                                    { new PropertyName("standard"),new TextProperty{ Analyzer = "standard"} }
                                })
                            }))));

                if (!createResult.IsValid && request.IndexName.IsNullOrWhiteSpace())
                    return ApiResult.IsFailed("创建索引失败");
            }
            
            var document = request.Items.MapTo<List<SearchKey>>();

            var result = _elasticClient.BulkAll(indexName, document);

            return result ? ApiResult.IsSuccess(data: indexName) : ApiResult.IsFailed();
        }

        /// <summary>
        /// 更改别名
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public ApiResult Rename(SearchKeyRanameRequest request)
        {
            var aliasName = typeof(SearchKey).GetRelationName();
            var getAliasResult = _elasticClient.Indices.GetAlias(aliasName);

            //给新index指定别名
            var bulkAliasRequest = new BulkAliasRequest
            {
                Actions = new List<IAliasAction>
                {
                    new AliasAddDescriptor().Index(request.IndexName).Alias(aliasName)
                }
            };

            //移除别名里旧的索引
            if (getAliasResult.IsValid)
            {
                var indeNameList = getAliasResult.Indices.Keys;
                foreach (var indexName in indeNameList)
                {
                    bulkAliasRequest.Actions.Add(new AliasRemoveDescriptor().Index(indexName.Name).Alias(aliasName));
                }
            }

            var result = _elasticClient.Indices.BulkAlias(bulkAliasRequest);

            //删除旧的index
            if (getAliasResult.IsValid)
            {
                var indeNameList = getAliasResult.Indices.Keys;
                foreach (var indexName in indeNameList)
                {
                    _elasticClient.Indices.Delete(indexName);
                }
            }

            return result != null && result.ApiCall.Success ? ApiResult.IsSuccess() : ApiResult.IsFailed();
        }
    }
}