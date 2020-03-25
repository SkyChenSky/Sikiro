using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Sikiro.Tookits.Extension
{
    public static class HttpContextExtension
    {
        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientIp(this HttpRequest request)
        {
            var ip = request.Headers["X-Real-IP"].FirstOrDefault() ??
                     request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return ip;
        }
    }
}
