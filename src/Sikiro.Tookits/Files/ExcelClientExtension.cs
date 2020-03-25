using Microsoft.Extensions.DependencyInjection;

namespace Sikiro.Tookits.Files
{
    public static class ExcelClientExtension
    {
        public static void AddExcelClient(this IServiceCollection services, string fileServerUrl)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddSingleton<ExcelClient>();
            services.AddSingleton(new ExcelOption { Url = fileServerUrl });
        }
    }
}
