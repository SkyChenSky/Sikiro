using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class BindingWxRequest
    {
        [Required(ErrorMessage = "请输入用户ID")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "openid不能为空")]
        public string OpenId { get; set; }

        [Required(ErrorMessage = "微信昵称不能为空")]
        public string WxName { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        [Required(ErrorMessage = "企业ID必传")]
        public string CompanyId { get; set; }

    }
}
