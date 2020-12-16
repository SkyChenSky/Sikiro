using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sikiro.WebApi.Customer.Extention
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// 获取模型验证错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string GetModelStateMsg(this ModelStateDictionary modelState)
        {
            var builder = new StringBuilder();

            foreach (var key in modelState.Keys)
            {
                var errors = modelState[key].Errors;

                foreach (var error in errors)
                {
                    if (builder.Length > 0)
                        builder.Append("<br/>");

                    builder.Append(error.ErrorMessage);
                }
            }

            return builder.ToString();
        }
    }
}