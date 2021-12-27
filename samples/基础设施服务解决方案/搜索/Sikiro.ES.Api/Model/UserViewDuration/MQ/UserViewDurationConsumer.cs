using Nest;
using Sikiro.Bus.Extension;
using Sikiro.Elasticsearch.Extension;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;

namespace Sikiro.ES.Api.Model.UserViewDuration.MQ
{
    public class UserViewDurationConsumer : BaseConsumer<UserViewDurationMessage>
    {
        private readonly ElasticClient _elasticClient;

        public UserViewDurationConsumer(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public override void Excute(UserViewDurationMessage msg)
        {
            var document = msg.MapTo<Entity.UserViewDuration>();

            var result = _elasticClient.Create(document, a => a.Index(typeof(Entity.UserViewDuration).GetRelationName() + "-" + msg.CreateDateTime.ToString("yyyy-MM"))).GetApiResult();
            if (result.Failed)
                LoggerHelper.WriteToFile(result.Message);
        }
    }
}
