using System.Linq;
using MongoDB.Driver;
using Sikiro.Nosql.Mongo.Diagnostics;
using SkyApm;
using SkyApm.Common;
using SkyApm.Diagnostics;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace Sikiro.MicroService.Extension.SkyApm.Diagnostics
{
    public class MongoTracingDiagnosticProcessor : ITracingDiagnosticProcessor
    {
        public string ListenerName => MongoDiagnosticListenerExtensions.MONGO_DIAGNOSTIC_LISTENER;

        private readonly ITracingContext _tracingContext;
        private readonly ILocalSegmentContextAccessor _localSegmentContextAccessor;

        public MongoTracingDiagnosticProcessor(ITracingContext tracingContext,
            ILocalSegmentContextAccessor localSegmentContextAccessor)
        {
            _tracingContext = tracingContext;
            _localSegmentContextAccessor = localSegmentContextAccessor;
        }

        private void AddConnectionTag(SegmentContext context, MongoClient mongoClient)
        {
            if (mongoClient == null || mongoClient.Settings == null)
            {
                return;
            }
            if (mongoClient.Settings.Server != null)
            {
                context.Span.Peer = new StringOrIntValue(mongoClient.Settings.Server.ToString());
            }
            if (string.IsNullOrEmpty(mongoClient.Settings.Credential.Source))
            {
                context.Span.AddTag(Tags.DB_INSTANCE, mongoClient.Settings.Credential.Source);
            }
        }
        private SegmentContext CreateSmartSqlLocalSegmentContext(string operation)
        {
            var context = _tracingContext.CreateLocalSegmentContext(operation);
            context.Span.SpanLayer = SpanLayer.DB;
            context.Span.Component = "GS.Nosql.Mongo";
            context.Span.AddTag(Tags.DB_TYPE, "MongoDB");
            return context;
        }

        [DiagnosticName(MongoDiagnosticListenerExtensions.MONGO_EXCUTE_BEFORE)]
        public void ExcuteBefore([Object] ExcuteData eventData)
        {
            var context = CreateSmartSqlLocalSegmentContext("mongo");
            AddConnectionTag(context, eventData.MongoClient);
        }

        [DiagnosticName(MongoDiagnosticListenerExtensions.MONGO_EXCUTE_AFTER)]
        public void ExcuteAfter([Object] ExcuteData eventData)
        {
            var context = _localSegmentContextAccessor.Context;
            if (context != null)
            {
                _tracingContext.Release(context);
            }
        }

        [DiagnosticName(MongoDiagnosticListenerExtensions.MONGO_EXCUTE_ERROR)]
        public void ExcuteError([Object] ExcuteExceptionData eventData)
        {
            var context = _localSegmentContextAccessor.Context;
            if (context != null)
            {
                context.Span.ErrorOccurred(eventData.Ex);
                _tracingContext.Release(context);
            }
        }
    }
}
