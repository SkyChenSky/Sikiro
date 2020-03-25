using System;
using MongoDB.Driver;

namespace Sikiro.Nosql.Mongo.Diagnostics
{
    public class ExcuteData
    {
        public ExcuteData(Guid operationId, string operation, MongoClient mongoClient)
        {
            OperationId = operationId;
            Operation = operation;
            MongoClient = mongoClient;
        }
        public Guid OperationId { get; }

        public string Operation { get; }

        public MongoClient MongoClient { get; }
    }
}
