using System;
using System.Reflection;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sikiro.Bus.Extension
{
    public static class IocExtension
    {
        public static IServiceCollection AddEasyNetQ(this IServiceCollection services, string connectionStr)
        {
            services.AddSingleton(RabbitHutch.CreateBus(connectionStr, a => a.EnableLegacyDeadLetterExchangeAndMessageTtlScheduler()));
            return services;
        }

        public static IServiceCollection AddEasyNetQ(this IServiceCollection services, string connectionStr, Action<IServiceRegister> registerServices)
        {
            services.AddSingleton(RabbitHutch.CreateBus(connectionStr, registerServices));
            return services;
        }

        public static IApplicationBuilder UseAutoSubscribe(this IApplicationBuilder appBuilder, IHostApplicationLifetime lifetime, Assembly[] assemblies, string subscriptionIdPrefix = "")
        {
            var bus = appBuilder.ApplicationServices.GetService<IBus>();

            lifetime.ApplicationStarted.Register(() =>
            {
                var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
                subscriber.Subscribe(assemblies);
                subscriber.SubscribeAsync(assemblies);
            });

            lifetime.ApplicationStopped.Register(() => bus.Dispose());

            return appBuilder;
        }

        public static IApplicationBuilder UseAutoSubscribe(this IApplicationBuilder appBuilder, IHostApplicationLifetime lifetime, string subscriptionIdPrefix)
        {
            return UseAutoSubscribe(appBuilder, lifetime, new[] { Assembly.GetEntryAssembly() }, subscriptionIdPrefix);
        }
    }
}
