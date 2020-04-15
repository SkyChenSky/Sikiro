using System.ComponentModel.DataAnnotations;

namespace GS.WebApi.Customer.Models.User
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
