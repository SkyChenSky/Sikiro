using Microsoft.Extensions.DependencyInjection;
using Sikiro.MicroService.Extension.WebApiClient;
using WebApiClient;

namespace Sikiro.MicroService.Extension.Rpc
{
    /// <summary>
    /// 基于DependencyInjection的扩展
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// 添加HttpApi
        /// 返回HttpApi工厂
        /// </summary>
        /// <typeparam name="TInterface">基类接口类型</typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static HttpApiFactoryBuilder<TInterface> AddHttpApis<TInterface>(this IServiceCollection services)
            where TInterface : class, IHttpApi
        {
            return new HttpApiFactoryBuilder<TInterface>(services);
        }
    }
}
