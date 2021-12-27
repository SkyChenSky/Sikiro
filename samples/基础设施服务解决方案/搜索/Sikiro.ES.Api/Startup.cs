using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Sikiro.Bus.Extension;
using Sikiro.Elasticsearch.Extension;
using Sikiro.ES.Api.Attribute;
using Sikiro.ES.Api.Extention;
using Sikiro.ES.Api.Model.UserViewDuration.MQ;
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
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
                options.SerializerSettings.Formatting = Formatting.Indented;
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Elasticsearch内部API",
                        Version = "v1",
                        Description = Assembly.GetExecutingAssembly().GetName(true).Name
                    });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                if (!string.IsNullOrWhiteSpace(basePath))
                {
                    var xmlPath = Path.Combine(basePath, "Sikiro.ES.Api.xml");
                    c.IncludeXmlComments(xmlPath);
                }
            });

            services.AddElasticsearch(Configuration);

            services.AddEasyNetQ(Configuration["RabbitMQ"]);

            services.AddConsumer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            app.UseAllElasticApm(Configuration);

            app.UseHealthChecks("/health");

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SF.ES.Api v1");
                c.RoutePrefix = "";
            });

            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSubscribe<UserViewDurationMessage, UserViewDurationConsumer>(lifetime);
        }
    }

    public static class ConfigureServicesExtension
    {
        public static List<Type> GetTypeOfConsumer(this AssemblyName assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName).ExportedTypes.Where(b => b.BaseType?.Name == typeof(BaseConsumer<>).Name).ToList();
        }

        public static IServiceCollection AddConsumer(this IServiceCollection services)
        {
            var defaultAssemblyNames = DependencyContext.Default.GetDefaultAssemblyNames().Where(a => a.FullName.Contains("Sikiro.")).ToList();

            var assemblies = defaultAssemblyNames.SelectMany(a => a.GetTypeOfConsumer()).ToList();

            assemblies.ForEach(assembliy =>
            {
                services.AddSingleton(assembliy);
            });

            return services;
        }
    }
}
