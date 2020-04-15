using System.ComponentModel.DataAnnotations;
using GS.Common.Utils;
using GS.WebApi.Customer.Attribute;

namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 注册请求
    /// </summary>
    public class UserRegisterRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "请输入用户名")]
        [RegularExpression(RegularExpression.UserName, ErrorMessage = "请输入符合规则的用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        [Required(ErrorMessage = "注册的企业ID为必传")]
        public string CompanyId { get; set; }

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
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "请输入验证码")]
        public string Code { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        public string UserNo { get; set; }
    }
}
