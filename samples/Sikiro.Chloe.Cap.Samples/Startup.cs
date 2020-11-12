using System;
using DotNetCore.CAP.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sikiro.Chloe.Cap.Samples.Db;
using Sikiro.Chloe.Extension;

namespace Sikiro.Chloe.Cap.Samples
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
            services.AddChloeDbContext<BusinessPlatformContext>("Server=im.gshichina.com;Port=5002;Database=business_platform;Uid=ge;Pwd=shi2019");

            services.AddCap(x =>
            {
                x.UseMySql("Server=im.gshichina.com;Port=5002;Database=business_platform;Uid=ge;Pwd=shi2019");
                x.UseRabbitMq("amqp=amqp://guest:guest@rabbitmq.gshichina.com:5112");
                x.UseDashboard();
                x.FailedRetryCount = 5;
                x.FailedRetryInterval = 30;
                x.DefaultGroup = "sky.groups.order";
                x.FailedThresholdCallback = (type, name, content) =>
                {
                    Console.WriteLine($@"A message of type {type} failed after executing {x.FailedRetryCount} several times, requiring manual troubleshooting. Message name: {name}, message body: {content}");
                };
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}
