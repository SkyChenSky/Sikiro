using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Service.Customer.Bo
{
    public class OrderNoForecastDataBo
    {
        /// <summary>
        /// 预报单主键
        /// </summary>
        public string ForecastId { get; set; }

        /// <summary>
        /// 预报单号类型:1无快递单号2有快递单号
        /// </summary>
        public int ForecastType { get; set; }

        /// <summary>
        /// 预报单号
        /// </summary>
        public string ForecastNo { get; set; }

        /// <summary>
        /// 快递单
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 是否到齐
        /// </summary>
        public bool IsHere { get; set; }

        /// <summary>
        /// 预报单状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 总票数
        /// </summary>
        public int TotalVotes { get; set; }

        /// <summary>
        /// 发货时效id
        /// </summary>
        public string ShipmentTimeId { get; set; }

        /// <summary>
        /// 发货时效名称
        /// </summary>
        public string ShipmentTimeName { get; set; }

        /// <summary>
        /// 会员id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 会员昵称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 会员No
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 预报方式:0.集运预报;1.快运预报
        /// </summary>
        public int ForecastWay { get; set; }

        /// <summary>
        /// 运输方案:1.陆运C1;2.人工下单
        /// </summary>
        public int? TransportScheme { get; set; }

        /// <summary>
        /// 是否计算退税手续费
        /// </summary>
        public bool IsAutodrawbackservicefee { get; set; }

        /// <summary>
        /// 是否计算商检费
        /// </summary>
        public bool IsAutogoodcheckfee { get; set; }

        /// <summary>
        /// 货物品名
        /// </summary>
        public string CargoName { get; set; }

        /// <summary>
        /// 货物类型ID
        /// </summary>
        public string CargoTypeId { get; set; }

        /// <summary>
        /// 货物类型名称
        /// </summary>
        public string CargoTypeName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public decimal? Long { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public decimal? Wide { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public decimal? Hight { get; set; }

        /// <summary>
        /// 其他要求
        /// </summary>
        public string OtherRequirement { get; set; }

        /// <summary>
        /// 付款方式1:到付 2寄付
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 垫付费用
        /// </summary>
        public decimal DisbursementFee { get; set; }

        /// <summary>
        /// 货物价值
        /// </summary>
        public decimal CargoValue { get; set; }

        /// <summary>
        /// 代收费用
        /// </summary>
        public decimal SubstitutionFee { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string RecevierName { get; set; }

        /// <summary>
        /// 收件人手机号
        /// </summary>
        public string RecevierMobile { get; set; }

        /// <summary>
        /// 收件人详细地址
        /// </summary>
        public string RecevierAddress { get; set; }

        /// <summary>
        /// 收货者国家
        /// </summary>
        public string RecevierCountryId { get; set; }

        /// <summary>
        /// 收货者国家名称
        /// </summary>
        public string RecevierCountryName { get; set; }

        /// <summary>
        /// 收货者城市id
        /// </summary>
        public string RecevierCityId { get; set; }

        /// <summary>
        /// 收货者城市名称
        /// </summary>
        public string RecevierCityName { get; set; }

        /// <summary>
        /// 收货者省份id
        /// </summary>
        public string ReceiverProvinceId { get; set; }

        /// <summary>
        /// 收货者省份名称
        /// </summary>
        public string ReceiverProvinceName { get; set; }

        /// <summary>
        /// 收件人主键
        /// </summary>
        public string CustomerRecevierAddressId { get; set; }

        /// <summary>
        /// 收货者地址照片
        /// </summary>
        public string CustomerReceiverAddressPicUrl { get; set; }

        /// <summary>
        /// 收件人类型
        /// </summary>
        public int RecevierType { get; set; }

        /// <summary>
        /// 收件人自提点
        /// </summary>
        public string RecevierSiteId { get; set; }
    }
}
