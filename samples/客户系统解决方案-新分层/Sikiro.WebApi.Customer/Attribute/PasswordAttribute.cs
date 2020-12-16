using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Attribute
{
    ///<summary>
    /// 密码验证特性
    /// </summary>
    public class PasswordAttribute : RegularExpressionAttribute
    {
        private const string RegexPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,18}$";
        public PasswordAttribute(bool isFlag, string msg) : base(RegexPattern)
        {
            if (isFlag)
            {
                ErrorMessage = msg;
            }
            else
            {
                ErrorMessage = "请输入6-18位字母与数字组合的密码";
            }
          
        }
    }
}
