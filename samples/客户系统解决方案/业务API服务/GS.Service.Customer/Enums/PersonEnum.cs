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
        /// 保险
        /// </summary>
        public enum EnumInsurance
        {
            [Display(Name = "否")]
            No =1,
            [Display(Name = "是")]
            Yes =2
        }

        /// <summary>
        /// 包装类型
        /// </summary>
        public enum EnumPackaging
        {
            [Display(Name = "纸箱")]
             纸箱 = 1,
            [Display(Name = "木架")]
             木架 = 2,
            [Display(Name = "木箱")]
            木箱 = 3,
            [Display(Name = "泡沫棉")]
            泡沫棉 = 4,
            [Display(Name = "木托")]
            木托 = 5,

        }
        /// <summary>
        /// 配送方式
        /// </summary>

        public enum EnumDistribution
        {
            [Display(Name = "自提")]
            自提 = 1,
            [Display(Name = "配送")]
            配送 = 2,
        }

        /// <summary>
        /// 付款方式
        /// </summary>
        public enum EnumPayment
        {
            [Display(Name = "货到付款")]
            货到付款 = 1,
            [Display(Name = "钱包支付")]
            钱包支付 = 2,
            //[Display(Name = "到付")]
            //到付 = 1,
            //[Display(Name = "寄付")]
            //寄付 = 2,
            //[Display(Name = "等通知延时付款")]
            //等通知延时付款 = 3
        }
        /// <summary>
        /// 预报单号类型
        /// </summary>
        public enum EForecastTypeStatus
        {
            [Display(Name = "无快递单号")]
            无快递单号 = 1,
            [Display(Name = "有快递单号")]
            有快递单号 = 2,
        }
        #region 支付方式枚举

        /// <summary>
        /// 支付方式
        /// </summary>
        public enum EpdPayment
        {
            [Display(Name = "现金")]
            现金 = 1,
            [Display(Name = "电子支付")]
            电子支付 = 2,
            [Display(Name = "转账汇款")]
            转账汇款 = 3,
            [Display(Name = "微信支付")]
            微信支付 = 6,
            [Display(Name = "余额支付")]
            余额支付 = 7,
            [Display(Name = "月结")]
            月结 = 8,
        }
        /// <summary>
        ///快运的支付方式-选择支付方式
        /// </summary>
        public enum EWharehousePayWay
        {
            [Display(Name = "现金")]
            现金 = 1,
            [Display(Name = "电子支付")]
            电子支付 = 2,
            [Display(Name = "转账汇款")]
            转账汇款 = 3,
        }
        /// <summary>
        ///集运代客下单-选择支付方式
        /// </summary>
        public enum EPackagingPayWay
        {
            [Display(Name = "现金")]
            现金 = 1,
            [Display(Name = "微信支付")]
            微信支付 = 6,
            [Display(Name = "余额支付")]
            余额支付 = 7,
        }

        #endregion

        /// <summary>
        /// 预报单状态
        /// </summary>
        public enum EForecastStatus
        {
            [Display(Name = "已创建")]
            已创建 = 1,
            [Display(Name = "已取消")]
            已取消 = 2,
            [Display(Name = "已完成")]
            已完成 = 3
        }

        /// <summary>
        /// 单号类型
        /// </summary>
        public enum EForecastOrderType
        {
            [Display(Name = "预报单号")]
            预报单号 = 1,
            [Display(Name = "快递单号")]
            快递单号 = 2
        }

        /// <summary>
        /// 支付流水类型
        /// </summary>
        public enum EPaymentFlowType
        {
            [Display(Name = "充值")]
            充值 = 1,
            [Display(Name = "支出")]
            支出 = 2,
            [Display(Name = "系统退回")]
            系统退回 = 3,
        }

        /// <summary>
        /// 支付流水状态
        /// </summary>
        public enum EPaymentFlowStatus
        {
            [Display(Name = "支付中")]
            支付中 = 0,
            [Display(Name = "成功")]
            成功 = 1,
            [Display(Name = "失败")]
            失败 = 2,
            [Display(Name = "系统退回")]
            系统退回 = 3
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

        /// <summary>
        /// 预报方式
        /// </summary>
        public enum EForecastWay
        {
            [Display(Name = "快运预报")]
            快运预报 = 1,
            [Display(Name = "集运预报")]
            集运预报 = 0,
        }

        /// <summary>
        /// 运输方案
        /// </summary>
        public enum ETransportScheme
        {
            [Display(Name = "陆运c1")]
            陆运c1 = 1,
            [Display(Name = "自动改为人工下单")]
            自动改为人工下单 = 2,
        }
    }

}
