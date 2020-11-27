using System.Collections.Generic;
using System.Reflection;
using Sikiro.MicroService.Extension.SkyApm;
using Sikiro.Tookits.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using Sikiro.WebApi.Customer.Attribute;
using Sikiro.WebApi.Customer.Extention;

namespace Sikiro.WebApi.Customer
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

            services.AddAuth();

            services.AddHttpContextAccessor();

            services.AddService();

            //  services.UseSkyApm();

            services.AddHttpContextAccessor();

            services.AddRpc(Configuration);

            services.AddSwaggerDocument(config =>
            {
                config.OperationProcessors.Add(new OperationSecurityScopeProcessor("access-token"));
                config.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("access-token", new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "access-token",
                        Description = "复制jwt进文本框",
                        In = OpenApiSecurityApiKeyLocation.Header
                    }));
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "个人平台API";
                    document.Info.Description = Assembly.GetExecutingAssembly().GetName(true).Name;
                };
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks("/health");

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3(options =>
            {
                options.Path = "";
            });

            app.UseMvc();
        }
    }
}
