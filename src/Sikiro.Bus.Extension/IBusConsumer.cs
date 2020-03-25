using System.Threading.Tasks;

namespace Sikiro.Bus.Extension
{
    /// <summary>
    /// 消息总线消费者
    /// </summary>
    public interface IBusConsumer<T>
    {
        /// <summary>
        /// 业务执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        Task Excute(T msg);
    }
}
