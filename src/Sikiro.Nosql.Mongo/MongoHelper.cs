using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;

namespace Sikiro.Nosql.Mongo
{
    public static class MongoHelper
    {
        /// <summary>
        /// 字符串ID转换成ObjectId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ObjectId ToObjectId(this string id)
        {
            return string.IsNullOrEmpty(id) ? ObjectId.Empty : new ObjectId(id);
        }

        /// <summary>
        /// 字符串集合转换成objectId集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static IEnumerable<ObjectId> ToObjectIds(this IEnumerable<string> ids)
        {
            return ids.Select(a => a.ToObjectId());
        }
    }
}
