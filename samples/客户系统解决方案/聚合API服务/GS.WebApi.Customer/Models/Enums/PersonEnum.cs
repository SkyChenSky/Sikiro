using System.ComponentModel.DataAnnotations;

namespace GS.WebApi.Customer.Models.Enums
{
    public class PersonEnum
    {
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
        public enum Status
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
        /// <summary>
        /// 收货入库-支付方式
        /// </summary>
        public enum EnumPayment
        {
            [Display(Name = "到付")]
            到付 = 1,
            [Display(Name = "寄付")]
            寄付 = 2,
            [Display(Name = "等通知延时付款")]
            等通知延时付款 = 3
        }

        /// <summary>
        /// 发货时效-运输方式
        /// </summary>
        public enum EShipmentTimeTransport
        {
            [Display(Name = "航空")]
            航空 = 1,
            [Display(Name = "陆运")]
            陆运 = 5,
            [Display(Name = "海运")]
            海运 = 3
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public enum EpdPayment
        {
            [Display(Name = "余额支付")]
            余额支付 = 7,
            [Display(Name = "微信支付")]
            微信支付 = 6,
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
        }
        /// <summary>
        /// 订单支付方式 1客户支付 2签收支付 3 财务支付
        /// </summary>
        public enum EPayMethod
        {
            [Display(Name = "客户支付")]
            客户支付 = 1,
            [Display(Name = "签收支付")]
            签收支付 = 2,
            [Display(Name = "财务支付")]
            财务支付 = 3
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
        /// <summary>
        /// 入库单状态 0:收货入库 10:入库上架 20:拣货下架 30:发货出库 40:中转 50:到点入库 60自提出库 70 自提签收 80 配送出库 90配送签收，配送失败签入
        /// </summary>
        public enum EWharehouseOrderStatus
        {

            [Display(Name = "收货入库")]
            收货入库 = 0,
            [Display(Name = "入库上架")]
            入库上架 = 10,
            [Display(Name = "拣货下架")]
            拣货下架 = 20,
            [Display(Name = "发货出库")]
            发货出库 = 30,
            [Display(Name = "中转入库")]
            中转入库 = 40,
            [Display(Name = "中转出库")]
            中转出库 = 45,
            [Display(Name = "到达入库")]
            到达入库 = 50,
            [Display(Name = "自提出库")]
            自提出库 = 60,
            [Display(Name = "自提签收")]
            自提签收 = 70,
            [Display(Name = "配送出库")]
            配送出库 = 80,
            [Display(Name = "配送签收")]
            配送签收 = 90,
            [Display(Name = "配送失败签入")]
            配送失败签入 = 100,
            [Display(Name = "生成订单")]
            生成订单 = 110,
            [Display(Name = "创建待确认")]
            创建待确认 = 120,
            [Display(Name = "取消订单")]
            取消订单 = 130
        }
        /// <summary>
        /// 支付状态
        /// </summary>
        public enum EWharehousePayStatus
        {
            [Display(Name = "支付失败")]
            支付失败 = -1,
            [Display(Name = "等待支付")]
            等待支付 = 0,
            [Display(Name = "支付成功")]
            支付成功 = 1,
            [Display(Name = "支付中")]
            支付中 = 2,
            [Display(Name = "部分支付")]
            部分支付 = 3,
        }
        /// <summary>
        ///代收货款手续费类型(0:代收款总额，1单票) 1是元 0是百分比
        /// </summary>
        public enum ECollectionGoodFeeType
        {
            [Display(Name = "代收款总额", Description = "百分比", Prompt = "%")]
            代收款总额 = 0,
            [Display(Name = "单票", Description = "金额", Prompt = "元")]
            单票 = 1,
        }
        /// <summary>
        ///商检费类型(单票/单件)
        /// </summary>
        public enum EGoodCheckFeeType
        {
            [Display(Name = "单票")]
            单票 = 0,
            [Display(Name = "单件")]
            单件 = 1,
        }
        /// <summary>
        /// 退税手续费类型(0单票，1单件)
        /// </summary>
        public enum EDrawbackServiceFeeType
        {
            [Display(Name = "单票")]
            单票 = 0,
            [Display(Name = "单件")]
            单件 = 1,
        }

        public enum SiteStatus
        {
            /// <summary>
            /// 停用
            /// </summary>
            [Display(Name = "停用")]
            Stop,
            /// <summary>
            /// 启用
            /// </summary>
            [Display(Name = "启用")]
            Open
        }

    }

}
