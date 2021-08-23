using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sikiro.Bus.Extension
{
    /// <summary>
    /// 跟业务强关联
    /// </summary>
    public static class ConsumerExtension
    {
        public static IApplicationBuilder UseSubscribe<T, TConsumer>(this IApplicationBuilder appBuilder, IHostApplicationLifetime lifetime) where T : EasyNetQEntity, new() where TConsumer : BaseConsumer
        {
            var bus = appBuilder.ApplicationServices.GetRequiredService<IBus>();
            var consumer = appBuilder.ApplicationServices.GetRequiredService<TConsumer>();

            lifetime.ApplicationStarted.Register(() =>
            {
                bus.Subscribe<T>(msg => consumer.Excute(msg));
            });

            lifetime.ApplicationStopped.Register(() => bus?.Dispose());

            return appBuilder;
        }
    }
}
