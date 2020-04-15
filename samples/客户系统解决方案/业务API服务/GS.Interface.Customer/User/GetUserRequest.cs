using System.ComponentModel.DataAnnotations;

namespace GS.Interface.Customer.User
{
    /// <summary>
    /// 根据企业ID，获取数据
    /// </summary>
    public class GetUserRequest
    {
        [Required(ErrorMessage = "企业id不能为空")]
        [StringLength(32, ErrorMessage = "企业id不能超过32字符")]
        public  string CompanyId { get; set; }

        [Required(ErrorMessage = "用户id不能为空")]
        [StringLength(32, ErrorMessage = "用户id不能超过32字符")]
        public string UserNo { get; set; }
    }
}
