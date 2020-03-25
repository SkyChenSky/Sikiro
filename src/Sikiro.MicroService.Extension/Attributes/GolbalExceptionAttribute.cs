using Microsoft.AspNetCore.Mvc.Filters;
using Sikiro.Tookits.Extension;

namespace Sikiro.MicroService.Extension.Attributes
{
    /// <summary>
    /// 全局异常捕获 
    /// </summary>
    public class RpcGolbalExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var exception = context.Exception.GetDeepestException();
                exception.WriteToFile("全局异常捕抓");
            }
        }
    }
}
