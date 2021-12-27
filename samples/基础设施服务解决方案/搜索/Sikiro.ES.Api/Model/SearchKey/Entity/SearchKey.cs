using System;
using System.Collections.Generic;
using Nest;
using Sikiro.Elasticsearch.Extension;

namespace Sikiro.ES.Api.Model.SearchKey.Entity
{
    [ElasticsearchType(RelationName = "search_key")]
    public class SearchKey : ElasticsearchEntity
    {
        [Number(NumberType.Integer, Name = "key_id")]
        public int KeyId { get; set; }

        [Number(NumberType.Integer, Name = "entity_id")]
        public int EntityId { get; set; }

        [Number(NumberType.Integer, Name = "entity_type")]
        public int EntityType { get; set; }

        [Text(Name = "key_name")]
        public string KeyName { get; set; }

        [Number(NumberType.Integer, Name = "weight")]
        public int Weight { get; set; }

        [Boolean(Name = "is_subsidiary")]
        public bool IsSubsidiary { get; set; }

        [Number(NumberType.Integer, Name = "sys_tag_id")]
        public List<int> SysTagId { get; set; }

        [Date(Name = "active_date")]
        public DateTimeOffset? ActiveDate { get; set; }
    }
}
