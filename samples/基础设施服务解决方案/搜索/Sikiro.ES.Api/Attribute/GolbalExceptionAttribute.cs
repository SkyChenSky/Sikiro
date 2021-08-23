using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.ES.Api.Attribute
{
    /// <summary>
    /// 全局异常捕获
    /// </summary>
    public class GolbalExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)
            {
                var exception = context.Exception.GetDeepestException();
                exception.WriteToFile("全局异常捕抓");

                context.ExceptionHandled = true;
                context.Result = new ObjectResult(ApiResult.IsError(exception.Message));
                context.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}
