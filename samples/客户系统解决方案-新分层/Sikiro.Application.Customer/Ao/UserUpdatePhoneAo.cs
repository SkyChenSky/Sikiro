using System.ComponentModel.DataAnnotations;

namespace Sikiro.Application.Customer.Ao
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserUpdatePhoneAo
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        public string CompanyId { get; set; }
    }
}
