using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sikiro.Nosql.Mongo.Base
{
    public abstract class MongoEntity
    {
        protected MongoEntity()
        {
            Id = new ObjectId(Guid.NewGuid().ToString("N"));
        }

        [BsonElement("_id")]
        public ObjectId Id { get; set; }

    }
}
