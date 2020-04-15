using System.ComponentModel.DataAnnotations;
using GS.WebApi.Customer.Attribute;

namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRetrievePwdRequest
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "请输入手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        [Required(ErrorMessage = "区号")]
        public string CountryCode { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "请输入新登录密码")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "请输入验证码")]
        public string Code { get; set; }

        /// <summary>
        /// 企业Id
        /// </summary>
        public string CompanyId { get; set; }
    }
}
