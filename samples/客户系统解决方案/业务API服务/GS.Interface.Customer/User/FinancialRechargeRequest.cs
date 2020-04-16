using System.ComponentModel.DataAnnotations;

namespace Sikiro.Interface.Customer.User
{
    public class FinancialRechargeRequest
    {
        [Required(ErrorMessage = "企业id不能为空")]
        [StringLength(32, ErrorMessage = "企业id不能超过32字符")]
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "用户id不能为空")]
        [StringLength(32, ErrorMessage = "用户id不能超过32字符")]
        public string UserNo { get; set; }

        [Required(ErrorMessage = "充值金额必须大于0")]
        public decimal Money { get; set; }

        public string Remark { get; set; }
    }
}
