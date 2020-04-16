using System.Reflection;
using GS.MicroService.Extension;
using GS.MicroService.Extension.Attributes;
using GS.MicroService.Extension.SkyApm;
using GS.Nosql.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sikiro.Infrastructure.Id.Models;

namespace Sikiro.Infrastructure.Id
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

            services.AddSingleton<FormatConvert>();

            services.AddSingleton(Configuration);

            services.AddHttpContextAccessor();

            services.UseSkyApm();

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "基础设施-分布式id";
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
