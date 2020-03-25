using System;
using System.Diagnostics;
using MongoDB.Driver;

namespace Sikiro.Nosql.Mongo.Diagnostics
{
    public static class MongoDiagnosticListenerExtensions
    {
        public const string MONGO_DIAGNOSTIC_LISTENER = "MongoDiagnosticListener";
        public static readonly DiagnosticListener Instance = new DiagnosticListener(MONGO_DIAGNOSTIC_LISTENER);
        public const string MONGO_SQL_PREFIX = "Mongo.";

        public const string MONGO_EXCUTE_BEFORE = MONGO_SQL_PREFIX + nameof(ExcuteBefore);
        public const string MONGO_EXCUTE_AFTER = MONGO_SQL_PREFIX + nameof(ExcuteAfter);
        public const string MONGO_EXCUTE_ERROR = MONGO_SQL_PREFIX + nameof(ExcuteError);

        public static Guid ExcuteBefore(this DiagnosticListener @this, MongoClient mongoClient, string operation = null)
        {
            if (!@this.IsEnabled(MONGO_EXCUTE_BEFORE))
                return Guid.Empty;
            var operationId = Guid.NewGuid();
            @this.Write(MONGO_EXCUTE_BEFORE, new ExcuteData(operationId, operation, mongoClient));
            return operationId;
        }

        public static void ExcuteAfter(this DiagnosticListener @this, Guid operationId, MongoClient mongoClient, string operation = null)
        {
            if (@this.IsEnabled(MONGO_EXCUTE_AFTER))
            {
                @this.Write(MONGO_EXCUTE_AFTER, new ExcuteData(operationId, operation, mongoClient));
            }
        }

        public static void ExcuteError(this DiagnosticListener @this, Guid operationId, Exception ex, MongoClient mongoClient, string operation = null)
        {
            if (@this.IsEnabled(MONGO_EXCUTE_ERROR))
            {
                @this.Write(MONGO_EXCUTE_ERROR, new ExcuteExceptionData(operationId, operation, mongoClient, ex));
            }
        }

        public static T Excuting<T>(this DiagnosticListener @this, Func<T> mongoRun, MongoClient mongoClient)
        {
            var operationId = Guid.Empty;
            try
            {
                operationId = Instance.ExcuteBefore(mongoClient);
                var result = mongoRun();
                Instance.ExcuteAfter(operationId, mongoClient);
                return result;
            }
            catch (Exception ex)
            {
                Instance.ExcuteError(operationId, ex, mongoClient);
                throw;
            }
        }
    }
}
