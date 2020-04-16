using System.Reflection;
using GS.MicroService.Extension;
using GS.MicroService.Extension.Attributes;
using GS.MicroService.Extension.SkyApm;
using GS.Nosql.Mongo;
using GS.Nosql.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sikiro.Infrastructure.Msg.Service;

namespace Sikiro.Infrastructure.Msg
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new RpcGolbalExceptionAttribute());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.Formatting = Formatting.Indented;
                }
            );

            services.AddHealthChecks();

            services.AddSingleton(new RedisRepository(Configuration["redisUrl"]));

            services.AddSingleton(Configuration);

            services.AddSingleton(new SmsService(Configuration["Sms:key"], Configuration["Sms:secret"], Configuration["Sms:sign"], Configuration["Sms:code"], Configuration["Sms:foreignsign"], Configuration["Sms:foreigncode"]));

            services.AddHttpContextAccessor();

            services.UseSkyApm();

            services.AddSingleton(new MongoRepository(Configuration["MongoDbUrl"]));

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "基础设施-消息服务";
                    document.Info.Description = Assembly.GetExecutingAssembly().GetName(true).Name;
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            app.UseHealthChecks("/health");

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseMvc();

            app.UseConsul(lifetime, Configuration);
        }
    }
}
