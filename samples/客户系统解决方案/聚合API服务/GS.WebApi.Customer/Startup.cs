using System.Reflection;
using GS.MicroService.Extension.SkyApm;
using GS.Tookits.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSwag;
using NSwag.Generation.Processors.Security;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.RegisterServices;
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
                options.Filters.Add(new GolbalExceptionAttribute());
                options.ModelBinderProviders.Insert(0, new TrimModelBinderProvider());
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
            services.AddHealthChecks();

            services.AddAuth();

            services.AddHttpContextAccessor();

            services.AddService();

            services.UseSkyApm();

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

            services.AddSenparcGlobalServices(Configuration) //Senparc.CO2NET 全局注册
                .AddSenparcWeixinServices(Configuration); //Senparc.Weixin 注册

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            IRegisterService register = RegisterService.Start(env, senparcSetting.Value).UseSenparcGlobal();// 启动 CO2NET 全局注册，必须！

            register.UseSenparcWeixin(senparcWeixinSetting.Value, senparcSetting.Value);//微信全局注册，必须！
            AccessTokenContainer.Register(Configuration["wechat:appid"], Configuration["wechat:appSecret"]);
            app.UseHealthChecks("/health");

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseMvc();
        }
    }
}
