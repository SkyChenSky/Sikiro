using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Sikiro.Nosql.Mongo.Base
{
    public abstract class MongoEntity
    {
        [BsonElement("_id")]
        public ObjectId Id { get; set; }

    }
}
