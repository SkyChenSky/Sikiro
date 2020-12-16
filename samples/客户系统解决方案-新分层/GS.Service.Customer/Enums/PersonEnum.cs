using System.ComponentModel.DataAnnotations;

namespace Sikiro.Service.Customer.Enums
{
    public class PersonEnum
    {
        public enum EStatus
        {
            /// <summary>
            /// 已取消
            /// </summary>
            [Display(Name = "已取消")]
            Stop = 0,
            /// <summary>
            /// 已创建
            /// </summary>
            [Display(Name = "已创建")]
            Open = 1,
            /// <summary>
            /// 已完成
            /// </summary>
            [Display(Name = "已完成")]
            Complete = 2,
        }
    }

}
