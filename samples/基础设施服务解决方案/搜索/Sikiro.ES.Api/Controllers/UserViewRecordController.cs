using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Sikiro.Elasticsearch.Extension;
using Sikiro.ES.Api.Model.UserViewRecord;
using Sikiro.ES.Api.Model.UserViewRecord.Entity;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.ES.Api.Controllers
{
    /// <summary>
    /// 用户记录
    /// </summary>
    [Route("/api/[controller]")]
    public class UserViewRecordController : BaseController
    {
        private readonly ElasticClient _elasticClient;

        public UserViewRecordController(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// 获取用户记录列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult<List<UserViewRecord>> Get([FromQuery] UserViewRecordGetRequest request)
        {
            var mustQuerys = new List<Func<QueryContainerDescriptor<UserViewRecord>, QueryContainer>>();

            if (request.UserId.HasValue)
            {
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.UserId).Value(request.UserId.Value)));
            }

            if (request.EntityId.HasValue)
            {
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityId).Value(request.EntityId.Value)));
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityType).Value(request.EntityType)));
            }

            if (request.CharpterId.HasValue)
            {
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.CharpterId).Value(request.CharpterId.Value)));
            }

            if (request.BeginDateTime.HasValue)
            {
                mustQuerys.Add(a => a.DateRange(dr =>
                    dr.Field(f => f.CreateDateTime).GreaterThanOrEquals(request.BeginDateTime.Value)));
            }

            if (request.EndDateTime.HasValue)
            {
                mustQuerys.Add(a =>
                    a.DateRange(dr => dr.Field(f => f.CreateDateTime).LessThanOrEquals(request.EndDateTime.Value)));
            }

            var searchResult = _elasticClient.Search<UserViewRecord>(a =>
                a.Index(typeof(UserViewRecord).GetRelationName() + "-*")
                    .Size(request.Size)
                    .Query(q => q.Bool(b => b.Must(mustQuerys)))
                    .SearchAfterTimestamp(request.Timestamp)
                    .Sort(s => s.Field(f => f.Timestamp, SortOrder.Descending)));

            var apiResult = searchResult.GetApiResult<UserViewRecord, List<UserViewRecord>>();
            return apiResult;
        }

        /// <summary>
        /// 获取用户记录数量
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Count")]
        public ApiResult Count([FromQuery] UserViewRecordHeadRequest request)
        {
            var mustQuerys = new List<Func<QueryContainerDescriptor<UserViewRecord>, QueryContainer>>();

            if (request.UserId.HasValue)
            {
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.UserId).Value(request.UserId.Value)));
            }

            if (request.EntityId.HasValue)
            {
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityId).Value(request.EntityId.Value)));
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityType).Value(request.EntityType)));
            }

            if (request.CharpterId.HasValue)
            {
                mustQuerys.Add(a => a.Term(t => t.Field(f => f.CharpterId).Value(request.CharpterId.Value)));
            }

            if (request.BeginDateTime.HasValue)
            {
                mustQuerys.Add(a => a.DateRange(dr => dr.Field(f => f.CreateDateTime).GreaterThanOrEquals(request.BeginDateTime.Value)));
            }

            if (request.EndDateTime.HasValue)
            {
                mustQuerys.Add(a => a.DateRange(dr => dr.Field(f => f.CreateDateTime).LessThanOrEquals(request.EndDateTime.Value)));
            }

            var countResult = _elasticClient.Count<UserViewRecord>(a =>
                a.Index(typeof(UserViewRecord).GetRelationName() + "-*").Query(q => q.Bool(b => b.Must(mustQuerys))));

            return ApiResult.IsSuccess(countResult.Count);
        }

        /// <summary>
        /// 新增用户记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult Post(UserViewRecordPostRequest request)
        {
            var document = request.MapTo<UserViewRecord>();
            document.CreateDateTime = DateTime.Now;

            var result = _elasticClient.Create(document, a => a.Index(typeof(UserViewRecord).GetRelationName() + "-" + DateTime.Now.ToString("yyyy-MM"))).GetApiResult();

            return result;
        }
    }
}