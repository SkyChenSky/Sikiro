using System;
using Microsoft.Extensions.DependencyInjection;
using SkyApm;
using SkyApm.Utilities.DependencyInjection;

namespace Sikiro.MicroService.Extension.SkyApm.Diagnostics
{
    public static class SkyWalkingBuilderExtensions
    {
        public static SkyApmExtensions AddMongo(this SkyApmExtensions extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            extensions.Services.AddMongo();

            return extensions;
        }

        public static IServiceCollection AddMongo(this IServiceCollection isc)
        {
            if (isc == null)
            {
                throw new ArgumentNullException(nameof(isc));
            }

            isc.AddSingleton<ITracingDiagnosticProcessor, MongoTracingDiagnosticProcessor>();

            return isc;
        }
    }
}
