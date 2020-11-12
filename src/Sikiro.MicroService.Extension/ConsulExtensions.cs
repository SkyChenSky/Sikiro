using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sikiro.Tookits.Extension;

namespace Sikiro.MicroService.Extension
{
    /// <summary>
    /// Consul扩展类
    /// </summary>
    public static class ConsulExtensions
    {
        /// <summary>
        /// Consul服务注册
        /// </summary>
        /// <param name="app"></param>
        /// <param name="lifetime"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration configuration)
        {
            var option = configuration.GetSection("Consul").Get<ConsulOption>();
            option.ThrowIfNull();

            //创建Consul客户端
            var consulClient = new ConsulClient(x => x.Address = new Uri(option.ConsulHost));//请求注册的 Consul 地址
            var selfHost = new Uri("http://" + LocalIpAddress + ":" + option.SelfPort);

            //注册服务
            var registration = new AgentServiceRegistration
            {
                Checks = new[] { new AgentServiceCheck
                {
                    Interval = TimeSpan.FromSeconds(option.HealthCheckInterval),
                    HTTP = $"{selfHost.OriginalString}/health",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(3)
                } },
                ID = selfHost.OriginalString.EncodeMd5String(),
                Name = option.ServiceName,
                Address = selfHost.Host,
                Port = selfHost.Port,
                Tags = new[] { $"urlprefix-/{option.ServiceName} strip=/{option.ServiceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别  
            };
            consulClient.Agent.ServiceRegister(registration).Wait();

            //反注册服务
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
            return app;
        }

        private static string LocalIpAddress
        {
            get
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var network in networkInterfaces)
                {
                    if (network.OperationalStatus != OperationalStatus.Up)
                        continue;
                    var properties = network.GetIPProperties();
                    if (properties.GatewayAddresses.Count == 0)
                        continue;

                    foreach (var address in properties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;
                        if (IPAddress.IsLoopback(address.Address))
                            continue;
                        return address.Address.ToString();
                    }
                }
                return "";
            }
        }
    }

    /// <summary>
    /// consul配置
    /// </summary>
    public class ConsulOption
    {
        /// <summary>
        /// Consul地址
        /// </summary>
        public string ConsulHost { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务端口号
        /// </summary>
        private int? _selfPort = 80;
        public int SelfPort
        {
            get => _selfPort ?? 80;
            set => _selfPort = value;
        }

        private int? _healthCheckInterval;
        /// <summary>
        /// 健康检查频率(秒)
        /// </summary>
        public int HealthCheckInterval
        {
            get => _healthCheckInterval ?? 10;
            set => _healthCheckInterval = value;
        }
    }
}
