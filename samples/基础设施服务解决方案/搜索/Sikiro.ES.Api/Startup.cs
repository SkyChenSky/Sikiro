using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sikiro.Bus.Extension;
using Sikiro.Elasticsearch.Extension;
using Sikiro.ES.Api.Attribute;
using Sikiro.ES.Api.Extention;
using Sikiro.ES.Api.Model.UserViewRecord.MQ;
using Sikiro.Tookits.Base;

namespace Sikiro.ES.Api
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
                options.Filters.Add<GolbalExceptionAttribute>();
                options.ModelBinderProviders.Insert(0, new TrimModelBinderProvider());
            }).AddMvcOptions(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var vaildMsg = context.ModelState.GetModelStateMsg();

                    return new BadRequestObjectResult(ApiResult.IsFailed(vaildMsg));
                };
            });

            services.AddHealthChecks();

            services.AddHttpContextAccessor();

            services.AddService();

            services.AddSwaggerDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Elasticsearch内部API";
                    document.Info.Description = Assembly.GetExecutingAssembly().GetName(true).Name;
                };
            });

            services.AddElasticsearch(Configuration);

            services.AddEasyNetQ(Configuration["RabbitMQ"]);
            services.AddSingleton<UserViewRecordConsumer>();
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

            app.UseSubscribe<UserViewRecordMessage, UserViewRecordConsumer>(lifetime);
        }
    }
}
