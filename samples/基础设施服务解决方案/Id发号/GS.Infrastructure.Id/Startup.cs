using System.Reflection;
using Sikiro.Nosql.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sikiro.Infrastructure.Id.Models;
using Sikiro.MicroService.Extension;
using Sikiro.MicroService.Extension.Attributes;
using Sikiro.MicroService.Extension.Consul;
using Sikiro.MicroService.Extension.SkyApm;

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
            services.AddControllers(options =>
            {
                options.Filters.Add<RpcGolbalExceptionAttribute>();
            });

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.UseHealthChecks("/health");

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3(options =>
            {
                options.Path = "";
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseConsul(lifetime, Configuration);
        }
    }
}
