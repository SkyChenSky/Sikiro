using Microsoft.Extensions.DependencyInjection;
using Sikiro.MicroService.Extension.SkyApm.Diagnostics;
using SkyApm.Tracing;

namespace Sikiro.MicroService.Extension.SkyApm
{
    /// <summary>
    /// 
    /// </summary>
    public static class SkyApmExtension
    {
        /// <summary>
        /// Consul服务注册
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseSkyApm(this IServiceCollection services)
        {
            return services.AddSingleton<ISamplingInterceptor, IgnoreSamplingInterceptor>();
        }
    }
}
