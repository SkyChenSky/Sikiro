using System.Collections.Generic;
using System.Linq;
using SkyApm.Tracing;

namespace Sikiro.MicroService.Extension.SkyApm
{
    /// <summary>
    /// 忽略采集终端
    /// </summary>
    public class IgnoreSamplingInterceptor : ISamplingInterceptor
    {
        private readonly List<string> _ignoreUrlList = new List<string>
        {
            "/health",
            "/swagger"
        };
        public int Priority { get; } = 0;

        public bool Invoke(SamplingContext samplingContext, Sampler next)
        {
            if (_ignoreUrlList.Any(b => samplingContext.OperationName.ToLower().Contains(b)))
                return false;

            return next(samplingContext);
        }
    }
}
