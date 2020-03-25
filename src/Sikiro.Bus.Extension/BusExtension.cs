using System;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;

namespace Sikiro.Bus.Extension
{
    /// <summary>
    /// 消息总线扩展
    /// </summary>
    public static class BusExtension
    {
        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="serviceProvider"></param>
        public static void Subscribe<T>(this IBus bus, ServiceProvider serviceProvider) where T : class, new()
        {
            var busConsumer = serviceProvider.GetService<IBusConsumer<T>>();
            bus.Subscribe<T>(string.Empty, async msg =>
            {
                LoggerHelper.WriteToFile("业务开始：" + msg.ToJson(), null);
                try
                {
                    await busConsumer.Excute(msg);
                }
                catch (Exception e)
                {
                    e.WriteToFile("业务执行异常");
                    Console.WriteLine(e);
                }
            });
        }
    }
}
