using System.ComponentModel.DataAnnotations;
using GS.WebApi.Customer.Attribute;

namespace GS.WebApi.Customer.Models.User
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class SendSmsRequest
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "请输入手机号")]
        public string Phone { get; set; }

    }
}
