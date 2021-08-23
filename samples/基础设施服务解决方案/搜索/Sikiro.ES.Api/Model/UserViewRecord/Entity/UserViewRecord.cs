using System;
using Nest;
using Sikiro.Elasticsearch.Extension;

namespace Sikiro.ES.Api.Model.UserViewRecord.Entity
{
    [ElasticsearchType(RelationName = "user_view_record")]
    public class UserViewRecord : ElasticsearchEntity
    {
        [Number(NumberType.Long, Name = "entity_id")]
        public long EntityId { get; set; }

        [Number(NumberType.Long, Name = "entity_type")]
        public long EntityType { get; set; }

        [Number(NumberType.Long, Name = "charpter_id")]
        public long CharpterId { get; set; }

        [Number(NumberType.Long, Name = "user_id")]
        public long UserId { get; set; }

        [Date(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }

        [Number(NumberType.Long, Name = "duration")]
        public long Duration { get; set; }

        [Ip(Name = "Ip")]
        public string Ip { get; set; }
    }
}
