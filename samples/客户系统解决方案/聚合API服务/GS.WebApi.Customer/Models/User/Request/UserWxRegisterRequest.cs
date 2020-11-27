using System.ComponentModel.DataAnnotations;

namespace Sikiro.WebApi.Customer.Models.User.Request
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserWxRegisterRequest
    {

        /// <summary>
        /// 企业ID
        /// </summary>
        [Required(ErrorMessage = "企业ID必传")]
        public string CompanyId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "手机号必传")]
        public string Phone { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        [Required(ErrorMessage = "区号")]
        public string CountryCode { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户WXid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码必传")]
        public string Code { get; set; }
    }
}
