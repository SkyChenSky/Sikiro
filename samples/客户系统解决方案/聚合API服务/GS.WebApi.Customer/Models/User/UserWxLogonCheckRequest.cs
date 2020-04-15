using System.ComponentModel.DataAnnotations;

namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserWxLogonCheckRequest
    {
        /// <summary>
        /// 微信授权CODE
        /// </summary>
        [Required(ErrorMessage = "微信授权CODE必传")]
        public string WxCode { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        [Required(ErrorMessage = "企业ID必传")]
        public string CompanyId { get; set; }
    }
}
