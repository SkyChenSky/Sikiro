﻿using System;
using System.Linq;
using Nest;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;

namespace Sikiro.Elasticsearch.Extension
{
    public static class TypeExtension
    {
        public static string GetRelationName(this Type type)
        {
            var attribute = AttributeHelper<ElasticsearchTypeAttribute>.GetAttribute(type);

            if (attribute != null && !attribute.RelationName.IsNullOrWhiteSpace())
                return attribute.RelationName;

            return null;
        }

        public static double? GetValue(this AggregateDictionary ad, string name)
        {
            if (!ad.Any())
                return null;

            return ((ValueAggregate)ad[name]).Value;
        }
    }
}
