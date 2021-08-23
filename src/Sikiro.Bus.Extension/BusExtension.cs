using System;
using System.Text;
using EasyNetQ;
using EasyNetQ.Topology;
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
            bus.PubSub.Subscribe<T>(string.Empty, async msg =>
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

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="queueName"></param>
        /// <param name="exchange"></param>
        /// <param name="topic"></param>
        /// <param name="action"></param>
        public static void Subscribe<T>(this IBus bus, string queueName, string exchange, string topic, Action<T> action) where T : EasyNetQEntity, new()
        {
            var qu = bus.Advanced.QueueDeclare(queueName);
            var ex = bus.Advanced.ExchangeDeclare(exchange, ExchangeType.Topic);
            bus.Advanced.Bind(ex, qu, topic);
            bus.Advanced.Consume(qu, (body, properties, info) =>
            {
                try
                {
                    var msg = Encoding.UTF8.GetString(body).FromJson<T>();
                    action(msg);
                }
                catch (Exception e)
                {
                    e.WriteToFile("业务执行异常");
                    Console.WriteLine(e);
                }
            });
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="action"></param>
        public static void Subscribe<T>(this IBus bus, Action<T> action) where T : EasyNetQEntity, new()
        {
            var queueAttribute = AttributeHelper<QueueAttribute>.GetAttribute(typeof(T));
            if (queueAttribute == null)
                throw new ArgumentNullException(nameof(QueueAttribute));

            var qu = bus.Advanced.QueueDeclare(queueAttribute.QueueName);
            var ex = bus.Advanced.ExchangeDeclare(queueAttribute.ExchangeName, ExchangeType.Topic);

            bus.Advanced.Bind(ex, qu, "");
            bus.Advanced.Consume(qu, (body, properties, info) =>
            {
                try
                {
                    var msg = Encoding.UTF8.GetString(body).FromJson<T>();
                    action(msg);
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
