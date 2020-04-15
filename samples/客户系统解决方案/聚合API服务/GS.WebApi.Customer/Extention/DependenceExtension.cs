using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using GS.Interface.Capital;
using GS.Interface.CMS;
using GS.Interface.Customer;
using GS.Interface.Id;
using GS.Interface.Msg;
using GS.Interface.Warehouse;
using GS.MicroService.Extension.Rpc;
using GS.Tookits.Extension;
using GS.Tookits.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using WebApiClient.Extensions.DependencyInjection;

namespace GS.WebApi.Customer.Extention
{
    public static class DependenceExtension
    {
        public static List<Type> GetTypeOfISerice(this AssemblyName assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName).ExportedTypes.Where(b => b.GetInterfaces().Contains(typeof(IDepend))).ToList();
        }

        public static void AddService(this IServiceCollection services)
        {
            var defaultAssemblyNames = DependencyContext.Default.GetDefaultAssemblyNames().Where(a => a.FullName.Contains("GS.")).ToList();

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
        /// 注册仓储服务内部api
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddWarehouseOrderApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpApis<IWarehouse>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri(configuration["WarehouseInnerApiUrl"]);
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
        /// 注册企业信息服务内部api
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddCmsApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpApis<ICMS>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri(configuration["CmsInnerApiUrl"]);
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            });
        }
        /// <summary>
        /// 注册资金服务Api
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        private static void AddCapitalApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpApi<IPay>().ConfigureHttpApiConfig(c =>
            {
                c.HttpHost = new Uri(configuration["CapitalInnerApiUrl"]);
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                c.HttpClient.Timeout = TimeSpan.FromSeconds(configuration["TimeoutSeconds"].TryDouble(3));
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
            services.AddWarehouseOrderApi(configuration);
            services.AddMsgApi(configuration);
            services.AddInfrastructureApi(configuration);
            services.AddCmsApi(configuration);
            services.AddCapitalApi(configuration);
        }
    }
}
