using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sikiro.WebApi.Customer.Attribute
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
                context.Result = new OkObjectResult(ServiceResult.IsError("服务器正在开小差～请稍后重试～"));
            }
        }
    }
}
