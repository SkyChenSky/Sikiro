using System;
using Chloe.MySql;
using Microsoft.Extensions.DependencyInjection;

namespace Sikiro.Chloe.Extension
{
    public static class IocExtension
    {
        public static void AddChloeDbContext<T>(this IServiceCollection services, string connectionStr) where T : MySqlContext
        {
            services.AddScoped(serviceProvider => (T)Activator.CreateInstance(typeof(T), new MySqlConnectionFactory(connectionStr)));
        }
    }
}
