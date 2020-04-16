using System.ComponentModel.DataAnnotations;
using Sikiro.Common.Utils;

namespace Sikiro.WebApi.Customer.Attribute
{
    ///<summary>
    /// 邮箱验证特性
    /// </summary>
    public class IdCardAttribute : RegularExpressionAttribute
    {
        private const string RegexPattern = RegularExpression.IdNum;
        public IdCardAttribute(): base(RegexPattern)
        {
            ErrorMessage = "身份证格式不正确";
        }
    }
}
