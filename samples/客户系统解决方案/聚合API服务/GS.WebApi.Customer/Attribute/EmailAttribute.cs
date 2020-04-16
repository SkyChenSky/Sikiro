using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Attribute
{
    ///<summary>
    /// 邮箱验证特性
    /// </summary>
    public class EmailAttribute : RegularExpressionAttribute
    {
        private const string RegexPattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public EmailAttribute(): base(RegexPattern)
        {
            ErrorMessage = "邮箱格式不正确";
        }
    }
}
