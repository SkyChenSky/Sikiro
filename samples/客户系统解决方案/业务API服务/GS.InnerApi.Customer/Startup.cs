using System.Reflection;
using GS.Chloe.Cap;
using GS.Chloe.Extension;
using GS.MicroService.Extension;
using GS.MicroService.Extension.Attributes;
using GS.MicroService.Extension.SkyApm;
using GS.Tookits.Base;
using GS.Tookits.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sikiro.Entity.Customer.DBContext;
using Sikiro.InnerApi.Customer.Extention;

namespace Sikiro.InnerApi.Customer
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

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var vaildMsg = context.ModelState.GetModelStateMsg();

                    return new OkObjectResult(ServiceResult.IsFailed(vaildMsg));
                };
            });

            services.AddCap(x =>
            {
                x.UseMySql(Configuration["PersonpPlatformDB"]);
                x.UseRabbitMq(Configuration["RabbitMqHost"]);
                x.DefaultGroup = "Cap.Queue";
                x.FailedRetryCount = 10;
                x.FailedRetryInterval = 30;
                x.FailedThresholdCallback = (type, name, content) =>
                {
                    LoggerHelper.WriteToFile(
                        $@"cap warming:A message of type {type} failed after executing {x.FailedRetryCount} several times, requiring manual troubleshooting. Message name: {name}, message body: {content}");
                };
            });

            services.UseSkyApm();

            services.AddHealthChecks();

            services.AddHttpContextAccessor();

            services.AddService();

            services.AddChloeDbContext<PersonPlatformContext>(Configuration["PersonpPlatformDB"]);

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "个人平台内部API";
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
