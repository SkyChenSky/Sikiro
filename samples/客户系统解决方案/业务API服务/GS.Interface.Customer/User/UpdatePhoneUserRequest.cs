using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdatePhoneUserRequest
    {

        [Required(ErrorMessage = "请输入用户ID")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "请输入手机号码")]
        public string Phone { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        [Required(ErrorMessage = "区号")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "请输入企业Id")]
        public string CompanyId { get; set; }
    }
}
