using System.ComponentModel.DataAnnotations;

namespace GS.Interface.Customer.User
{
    public class WxLogonCheckRequest
    {
        [Required(ErrorMessage = "openid不能为空")]
        public string OpenId { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        [Required(ErrorMessage = "企业ID必传")]
        public string CompanyId { get; set; }
    }
}
