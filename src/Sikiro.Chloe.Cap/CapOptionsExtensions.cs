using System;
using System.Linq;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Sikiro.Chloe.Cap.ConnectionParser;

namespace Sikiro.Chloe.Cap
{
    public static class CapOptionsExtensions
    {
        public static CapOptions UseRabbitMq(this CapOptions options, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            var connectionStringParser = new ConnectionStringParser().Parse(connectionString);

            Console.WriteLine($"cap connet info.Host:{connectionStringParser.Hosts.First().Host}.Port:{connectionStringParser.Port}.UserName:{connectionStringParser.UserName}.Password:{connectionStringParser.Password}.");
            options.UseRabbitMQ(option =>
            {
                option.HostName = connectionStringParser.Hosts.First().Host;
                option.Port = connectionStringParser.Port;
                option.UserName = connectionStringParser.UserName;
                option.Password = connectionStringParser.Password;
            });
            return options;
        }
    }
}