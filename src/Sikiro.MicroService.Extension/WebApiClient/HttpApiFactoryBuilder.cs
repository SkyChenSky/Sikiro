using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApiClient;

namespace Sikiro.MicroService.Extension.WebApiClient
{
    /// <summary>
    /// HttpApi实例工厂创建器
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    public class HttpApiFactoryBuilder<TInterface> where TInterface : class, IHttpApi
    {
        private bool _keepCookieContainer = true;

        private TimeSpan _lifeTime = TimeSpan.FromMinutes(2d);

        private TimeSpan _cleanupInterval = TimeSpan.FromSeconds(10d);

        private Action<HttpApiConfig, IServiceProvider> _configOptions;

        private Func<IServiceProvider, HttpMessageHandler> _handlerFactory;


        /// <summary>
        /// HttpApi实例工厂创建器
        /// </summary>
        /// <param name="services"></param>
        public HttpApiFactoryBuilder(IServiceCollection services)
        {
            var httpApiDic = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(TInterface))))
                .ToDictionary(k => k, v => new HttpApiFactory(v));

            foreach (var item in httpApiDic)
            {
                services.AddSingleton(p =>
                {
                    return item.Value
                        .SetLifetime(_lifeTime)
                        .SetCleanupInterval(_cleanupInterval)
                        .SetKeepCookieContainer(_keepCookieContainer)
                        .ConfigureHttpMessageHandler(() => _handlerFactory?.Invoke(p)) as IHttpApiFactory;
                });

                services.AddTransient(item.Key, p =>
                {
                    var factory = p.GetServices<IHttpApiFactory>().First(a => a.InterfaceType == item.Key);
                    factory.ConfigureHttpApiConfig(c =>
                    {
                        c.ServiceProvider = p;
                        _configOptions?.Invoke(c, p);
                    });
                    return factory.CreateHttpApi();
                });
            }
        }

        /// <summary>
        /// 配置HttpApiConfig
        /// </summary>
        /// <param name="configOptions">配置选项</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public HttpApiFactoryBuilder<TInterface> ConfigureHttpApiConfig(Action<HttpApiConfig> configOptions)
        {
            if (configOptions == null)
            {
                throw new ArgumentNullException(nameof(configOptions));
            }
            return ConfigureHttpApiConfig((c, p) => configOptions.Invoke(c));
        }


        /// <summary>
        /// 配置HttpApiConfig
        /// </summary>
        /// <param name="configOptions">配置选项</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public HttpApiFactoryBuilder<TInterface> ConfigureHttpApiConfig(Action<HttpApiConfig, IServiceProvider> configOptions)
        {
            _configOptions = configOptions ?? throw new ArgumentNullException(nameof(configOptions));
            return this;
        }

        /// <summary>
        /// 配置HttpMessageHandler的创建
        /// </summary>
        /// <param name="handlerFactory">创建委托</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public HttpApiFactoryBuilder<TInterface> ConfigureHttpMessageHandler(Func<HttpMessageHandler> handlerFactory)
        {
            if (handlerFactory == null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }
            return ConfigureHttpMessageHandler(p => handlerFactory.Invoke());
        }

        /// <summary>
        /// 配置HttpMessageHandler的创建
        /// </summary>
        /// <param name="handlerFactory">创建委托</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public HttpApiFactoryBuilder<TInterface> ConfigureHttpMessageHandler(Func<IServiceProvider, HttpMessageHandler> handlerFactory)
        {
            this._handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            return this;
        }

        /// <summary>
        /// 置HttpApi实例的生命周期
        /// </summary>
        /// <param name="lifeTime">生命周期</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        public HttpApiFactoryBuilder<TInterface> SetLifetime(TimeSpan lifeTime)
        {
            if (lifeTime <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(lifeTime));
            }
            _lifeTime = lifeTime;
            return this;
        }


        /// <summary>
        /// 获取或设置清理过期的HttpApi实例的时间间隔
        /// </summary>
        /// <param name="interval">时间间隔</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <returns></returns>
        public HttpApiFactoryBuilder<TInterface> SetCleanupInterval(TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval));
            }
            this._cleanupInterval = interval;
            return this;
        }

        /// <summary>
        /// 设置是否维护使用一个CookieContainer实例 该实例为首次创建时的CookieContainer
        /// </summary>
        /// <param name="keep">true维护使用一个CookieContainer实例</param>
        /// <returns></returns>
        public HttpApiFactoryBuilder<TInterface> SetKeepCookieContainer(bool keep)
        {
            _keepCookieContainer = keep;
            return this;
        }
    }
}
