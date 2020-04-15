using System.ComponentModel.DataAnnotations;
using GS.Common.Utils;

namespace GS.WebApi.Customer.Attribute
{
    ///<summary>
    /// 中文
    /// </summary>
    public class ChineseAttribute : RegularExpressionAttribute
    {
        private const string RegexPattern = RegularExpression.Chinese;
        public ChineseAttribute()
            : base(RegexPattern)
        {
            ErrorMessage = "请输入中文";
        }
    }
}
