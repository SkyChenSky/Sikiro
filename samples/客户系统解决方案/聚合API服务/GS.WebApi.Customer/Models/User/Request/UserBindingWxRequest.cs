using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User.Request
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserBindingWxRequest
    {
        /// <summary>
        /// 微信授权CODE
        /// </summary>
        [Required(ErrorMessage = "微信授权CODE必传")]
        public string WxCode { get; set; }
    }
}
