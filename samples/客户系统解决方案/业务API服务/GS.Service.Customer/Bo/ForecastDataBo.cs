using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Service.PersonPlatform.Bo
{
    /// <summary>
    /// 预报信息
    /// </summary>
  public  class ForecastDataBo
    {
        public string ForecastOrderNoId { get; set; }


        /// <summary>
        /// 总票数
        /// </summary>
        public int TotalVotes { get; set; }


        /// <summary>
        /// 会员id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 预报单号类型
        /// </summary>
        public int ForecastType { get; set; }

        /// <summary>
        /// 预报单号关系主键
        /// </summary>
        public string ForecastId { get; set; }

        /// <summary>
        /// 预报单号
        /// </summary>
        public string ForecastNo { get; set; }

        /// <summary>
        /// 快递单
        /// </summary>
        public string OrderNo { get; set; }



        /// <summary>
        /// 运输方案:1.陆运C1;2.人工下单
        /// </summary>
        public int? TransportScheme { get; set; }

        /// <summary>
        /// 是否授权
        /// </summary>
        public bool IsEmpower { get; set; }

        /// <summary>
        /// 下单信息
        /// </summary>
        public PlaceAnOrderData PlaceAnOrderData { get; set; }
    }

    public class PlaceAnOrderData
    {
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
        /// 收货者地址照片
        /// </summary>
        public string ReceiverAddressPicUrl { get; set; }


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
        ///发运时效id
        /// </summary>

        public string ShipmentTimeId { get; set; }

        /// <summary>
        /// 发运时效名
        /// </summary>
        public string ShipmentTimeName { get; set; }

        /// <summary>
        /// 承运商id
        /// </summary>
  
        public string CarrierId { get; set; }

        /// <summary>
        /// 承运商名
        /// </summary>
        public string CarrierName { get; set; }


        /// <summary>
        /// 收货时间id
        /// </summary>
  
        public string ReceivingTimeId { get; set; }

        /// <summary>
        /// 收货时间值
        /// </summary>

        public string ReceivingTimeText { get; set; }



        /// <summary>
        /// 配送方式id
        /// </summary>

        public int EnumDistributionId { get; set; }

        /// <summary>
        /// 配送方式值
        /// </summary>

        public string EnumDistributionText { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string OrderRemark { get; set; }


        /// <summary>
        /// 垫付费备注
        /// </summary>
        public string DisbursementRemark { get; set; }



        /// <summary>
        /// 付款方式id
        /// </summary>
        public int EnumPaymentId { get; set; }

        /// <summary>
        /// 付款方式方式值
        /// </summary>
 
        public string EnumPaymentText { get; set; }

        /// <summary>
        /// 支付方式id
        /// </summary>
  
        public int PackagingPayWayId { get; set; }

        /// <summary>
        /// 支付方式值
        /// </summary>

        public string PackagingPayWayText { get; set; }


        /// <summary>
        /// 自提站点ID
        /// </summary>
        public string WharehouseSelfTakeSiteId { get; set; }



        /// <summary>
        /// 货物价值
        /// </summary>
        public decimal CargoValue { get; set; }

        /// <summary>
        /// 垫付费用
        /// </summary>
        public decimal DisbursementFee { get; set; }


        /// <summary>
        /// 代收费用
        /// </summary>
        public decimal SubstitutionFee { get; set; }

        /// <summary>
        /// 是否计算退税手续费
        /// </summary>
        public bool IsAutodrawbackservicefee { get; set; }

        /// <summary>
        /// 是否计算商检费
        /// </summary>
        public bool IsAutogoodcheckfee { get; set; }



    }
}
