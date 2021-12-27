using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Sikiro.Elasticsearch.Extension;
using Sikiro.ES.Api.Model;
using Sikiro.ES.Api.Model.UserViewDuration;
using Sikiro.ES.Api.Model.UserViewDuration.Entity;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.ES.Api.Controllers
{
    /// <summary>
    /// 用户阅读时长记录
    /// </summary>
    [Route("/api/[controller]")]
    public class UserViewDurationController : BaseController
    {
        private readonly ElasticClient _elasticClient;

        public UserViewDurationController(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// 获取用户指定记录阅读时长
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult Get([FromQuery] UserViewDurationGetRequest request)
        {
            var durationSumList = new List<int>();
            var searchDateTimeList = new List<string>();

            if (request.BeginDateTime.HasValue && request.EndDateTime.HasValue)
            {
                var month = request.EndDateTime.Value.DifferMonth(request.BeginDateTime.Value) + 1;

                for (int i = 0; i < month; i++)
                {
                    searchDateTimeList.Add(request.BeginDateTime.Value.AddMonths(i).ToString("yyyy-MM"));
                }
            }
            else
                searchDateTimeList.Add("*");

            foreach (var dateTime in searchDateTimeList)
            {
                var mustQuerys = new List<Func<QueryContainerDescriptor<UserViewDuration>, QueryContainer>>();

                if (request.UserId.HasValue)
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.UserId).Value(request.UserId.Value)));

                if (request.EntityType.HasValue)
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityType).Value(request.EntityType.Value)));

                if (request.EntityId.HasValue)
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityId).Value(request.EntityId.Value)));

                if (request.CharpterId.HasValue)
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.CharpterId).Value(request.CharpterId.Value)));

                if (request.BeginDateTime.HasValue)
                    mustQuerys.Add(a => a.DateRange(dr =>
                        dr.Field(f => f.CreateDateTime).GreaterThanOrEquals(request.BeginDateTime.Value).TimeZone(EsConst.TimeZone)));

                if (request.EndDateTime.HasValue)
                    mustQuerys.Add(a =>
                        a.DateRange(dr => dr.Field(f => f.CreateDateTime).LessThanOrEquals(request.EndDateTime.Value).TimeZone(EsConst.TimeZone)));

                var searchResult = _elasticClient.Search<UserViewDuration>(a =>
                    a.Index(typeof(UserViewDuration).GetRelationName() + "-" + dateTime)
                        .Query(q => q.Bool(b => b.Must(mustQuerys)))
                        .Sort(s => s.Field(f => f.Timestamp, SortOrder.Descending))
                        .Aggregations(aggr => aggr.Sum("Duration_Sum", s => s.Field(f => f.Duration))));

                var durationSum = searchResult.Aggregations.GetValue("Duration_Sum").TryInt();
                durationSumList.Add(durationSum);
            }

            return ApiResult<int>.IsSuccess(durationSumList.Sum());
        }

        /// <summary>
        /// 获取用户指定记录阅读时长记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("record")]
        public ApiResult<List<UserViewDurationRecordGetResponse>> GetRecord([FromQuery] UserViewDurationRecordGetRequest request)
        {
            var dataList = new List<UserViewDurationRecordGetResponse>();
            var searchDateTimeList = new List<string>();

            if (request.BeginDateTime.HasValue && request.EndDateTime.HasValue)
            {
                var month = request.EndDateTime.Value.DifferMonth(request.BeginDateTime.Value) + 1;

                for (int i = 0; i < month; i++)
                {
                    searchDateTimeList.Add(request.BeginDateTime.Value.AddMonths(i).ToString("yyyy-MM"));
                }
            }
            else
                searchDateTimeList.Add("*");

            foreach (var dateTime in searchDateTimeList)
            {
                var mustQuerys = new List<Func<QueryContainerDescriptor<UserViewDuration>, QueryContainer>>();

                if (request.UserId.HasValue)
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.UserId).Value(request.UserId.Value)));

                if (request.EntityType.HasValue)
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityType).Value(request.EntityType)));

                if (request.EntityId.HasValue)
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.EntityId).Value(request.EntityId.Value)));

                if (request.CharpterId.HasValue)
                    mustQuerys.Add(a => a.Term(t => t.Field(f => f.CharpterId).Value(request.CharpterId.Value)));

                if (request.BeginDateTime.HasValue)
                    mustQuerys.Add(a => a.DateRange(dr =>
                        dr.Field(f => f.CreateDateTime).GreaterThanOrEquals(request.BeginDateTime.Value).TimeZone(EsConst.TimeZone)));

                if (request.EndDateTime.HasValue)
                    mustQuerys.Add(a =>
                        a.DateRange(dr => dr.Field(f => f.CreateDateTime).LessThanOrEquals(request.EndDateTime.Value).TimeZone(EsConst.TimeZone)));

                var searchResult = _elasticClient.Search<UserViewDuration>(a =>
                    a.Index(typeof(UserViewDuration).GetRelationName() + "-" + dateTime)
                        .Size(request.Size)
                        .Query(q => q.Bool(b => b.Must(mustQuerys)))
                        .SearchAfterTimestamp(request.Timestamp)
                        .Sort(s => s.Field(f => f.Timestamp, SortOrder.Descending)));

                var apiResult = searchResult.GetApiResult<UserViewDuration, List<UserViewDurationRecordGetResponse>>();
                if (apiResult.Success)
                    dataList.AddRange(apiResult.Data);
            }


            return ApiResult<List<UserViewDurationRecordGetResponse>>.IsSuccess(dataList);
        }
    }
}