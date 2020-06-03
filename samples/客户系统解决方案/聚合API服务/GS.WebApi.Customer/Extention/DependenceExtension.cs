using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Sikiro.Interface.Customer;
using Sikiro.Interface.Id;
using Sikiro.Interface.Msg;
using Sikiro.MicroService.Extension.Rpc;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Interfaces;
using WebApiClient.Extensions.DependencyInjection;

namespace Sikiro.WebApi.Customer.Extention
{
    public static class DependenceExtension
    {
        public static List<Type> GetTypeOfISerice(this AssemblyName assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName).ExportedTypes.Where(b => b.GetInterfaces().Contains(typeof(IDepend))).ToList();
        }

        public static void AddService(this IServiceCollection services)
        {
            var defaultAssemblyNames = DependencyContext.Default.GetDefaultAssemblyNames().Where(a => a.FullName.Contains("Sikiro.")).ToList();

            var assemblies = defaultAssemblyNames.SelectMany(a => a.GetTypeOfISerice()).ToList();

            assemblies.ForEach(assemble =>
            {
                services.AddScoped(assemble);
            });
        }

        /// <summary>
        /// 注册个人平台内部api
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddCustomerApi(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHttpApis<ICustomer>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri(configuration["CustomerInnerApiUrl"]);
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            });
        }

        /// <summary>
        /// 注册基础设施服务Api
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddInfrastructureApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpApi<IId>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri(configuration["IdServerUrl"]);
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                c.HttpClient.Timeout = TimeSpan.FromSeconds(configuration["TimeoutSeconds"].TryDouble(3));
            });
        }

        /// <summary>
        /// 注册消息服务内部api
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddMsgApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpApis<IMsg>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri(configuration["MsgInnerApiUrl"]);
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            });
        }

        /// <summary>
        /// 注册Rpc服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddRpc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCustomerApi(configuration);
            services.AddMsgApi(configuration);
            services.AddInfrastructureApi(configuration);
        }
    }
}
