using System;

namespace Sikiro.Nosql.Mongo.Base
{
    #region Mongo实体标签
    /// <inheritdoc />
    /// <summary>
    /// Mongo实体标签
    /// </summary>
    public class MongoAttribute : Attribute
    {
        public MongoAttribute(string database, string collection = null)
        {
            Database = database;
            Collection = collection;
        }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string Collection { get; }

    }
    #endregion
}
