using System;
using Nest;
using Sikiro.Elasticsearch.Extension;

namespace Sikiro.ES.Api.Model.UserViewDuration.Entity
{
    [ElasticsearchType(RelationName = "user_view_duration")]
    public class UserViewDuration : ElasticsearchEntity
    {
        /// <summary>
        /// 作品ID
        /// </summary>
        [Number(NumberType.Long, Name = "entity_id")]
        public long EntityId { get; set; }

        /// <summary>
        /// 作品类型
        /// </summary>
        [Number(NumberType.Long, Name = "entity_type")]
        public long EntityType { get; set; }

        /// <summary>
        /// 章节ID
        /// </summary>
        [Number(NumberType.Long, Name = "charpter_id")]
        public long CharpterId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Number(NumberType.Long, Name = "user_id")]
        public long UserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Date(Name = "create_datetime")]
        public DateTimeOffset CreateDateTime { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [Number(NumberType.Long, Name = "duration")]
        public long Duration { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Ip(Name = "Ip")]
        public string Ip { get; set; }
    }
}
