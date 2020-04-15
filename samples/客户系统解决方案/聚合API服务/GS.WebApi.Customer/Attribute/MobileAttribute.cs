using System.ComponentModel.DataAnnotations;

namespace GS.WebApi.Customer.Attribute
{
    ///<summary>
    /// 手机号码
    /// </summary>
    public class MobileAttribute : RegularExpressionAttribute
    {
        private const string RegexPattern = @"^1[0-9]{10}$";
        public MobileAttribute() : base(RegexPattern)
        {
            ErrorMessage = "手机号码不正确";
        }
    }
}
