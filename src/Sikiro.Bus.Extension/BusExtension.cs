using System;
using System.Text;
using System.Threading.Tasks;
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
        /// 定时发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="msg">消息对象</param>
        /// <param name="seconds">定时发布时间（秒）</param>
        /// <param name="topic">话题</param>
        public static async Task FuturePublishAsync<T>(this IBus bus, T msg, long seconds, string topic)
        {
            var publishDate = TimeSpan.FromSeconds(seconds);
            publishDate = publishDate.TotalSeconds > 0 ? publishDate : TimeSpan.FromSeconds(1);
            await bus.Scheduler.FuturePublishAsync(msg, publishDate, config =>
            {
                config.WithTopic(topic);
            });
        }

        /// <summary>
        /// 定时发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="msg">消息对象</param>
        /// <param name="expireDateTime">定时发布日期</param>
        /// <param name="topic">话题</param>
        public static async Task FuturePublishAsync<T>(this IBus bus, T msg, DateTime expireDateTime, string topic)
        {
            var seconds = (expireDateTime - DateTime.Now).TotalSeconds;
            var publishDate = TimeSpan.FromSeconds((long)seconds);

            await FuturePublishAsync(bus, msg, (long)publishDate.TotalSeconds, topic);
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
