using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sikiro.Tookits.Base;
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

                context.ExceptionHandled = true;
                context.Result = new ObjectResult(ApiResult.IsError(exception.ToString()));
                context.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}
