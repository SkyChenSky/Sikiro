using System;
using Nest;
using Sikiro.Bus.Extension;
using Sikiro.Elasticsearch.Extension;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;

namespace Sikiro.ES.Api.Model.UserViewRecord.MQ
{
    public class UserViewRecordConsumer : BaseConsumer
    {
        private readonly ElasticClient _elasticClient;

        public UserViewRecordConsumer(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public override void Excute<T>(T msg)
        {
            var document = msg.MapTo<Entity.UserViewRecord>();

            var result = _elasticClient.Create(document, a => a.Index(typeof(Entity.UserViewRecord).GetRelationName() + "-" + DateTime.Now.ToString("yyyy-MM"))).GetApiResult();
            if (result.Failed)
                LoggerHelper.WriteToFile(result.Message);
        }
    }
}
