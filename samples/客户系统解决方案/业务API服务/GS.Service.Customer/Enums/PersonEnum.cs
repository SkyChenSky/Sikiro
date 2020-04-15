using System.ComponentModel.DataAnnotations;

namespace GS.Service.Customer.Enums
{
    public class PersonEnum
    {
        public enum Status
        {
            /// <summary>
            /// 已取消
            /// </summary>
            [Display(Name = "已取消")]
            Stop=0,
            /// <summary>
            /// 已创建
            /// </summary>
            [Display(Name = "已创建")]
            Open=1,
            /// <summary>
            /// 已完成
            /// </summary>
            [Display(Name = "已完成")]
            Complete =2,
        }

        /// <summary>
        /// 会员状态
        /// </summary>
        public enum EUserStatus
        {
            [Display(Name = "正常")]
            正常 = 1,
            [Display(Name = "停用")]
            停用 = 0,
        }
    }

}
