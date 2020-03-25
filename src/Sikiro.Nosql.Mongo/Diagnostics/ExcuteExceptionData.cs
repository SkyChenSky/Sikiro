using System;
using MongoDB.Driver;

namespace Sikiro.Nosql.Mongo.Diagnostics
{
    public class ExcuteExceptionData : ExcuteData
    {
        public Exception Ex { get; }


        public ExcuteExceptionData(Guid operationId, string operation, MongoClient mongoClient, Exception ex) : base(operationId, operation, mongoClient)
        {
            Ex = ex;
        }
    }
}
