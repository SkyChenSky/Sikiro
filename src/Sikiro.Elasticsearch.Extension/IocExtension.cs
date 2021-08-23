using System;
using System.Linq;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace Sikiro.Elasticsearch.Extension
{
    public static class IocExtension
    {
        /// <summary>
        /// NEST
        /// </summary>
        /// <param name="services"></param>
        /// <param name="option"></param>
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var option = configuration.GetSection("Elasticsearch").Get<ElasticsearchOption>();
            var nodes = option.Uris.Select(a => new Node(new Uri(a)));
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
            settings.BasicAuthentication(option.UserName, option.Password);
            var client = new ElasticClient(settings);
            services.AddSingleton(client);
        }
    }

    public class ElasticsearchOption
    {
        public string[] Uris { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
