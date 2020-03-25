using Microsoft.AspNetCore.Http;

namespace Sikiro.Tookits.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpExtension
    {
        public static bool IsAjax(this HttpRequest req)
        {
            bool result = false;

            var xreq = req.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = req.Headers["x-requested-with"] == "XMLHttpRequest";
            }

            return result;
        }
    }
}
