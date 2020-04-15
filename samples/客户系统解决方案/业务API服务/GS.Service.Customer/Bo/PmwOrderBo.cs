using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Service.Customer.Bo
{
    /// <summary>
    /// 旧系统的数据
    /// </summary>
    public class PmwOrderBo
    {
        /// <summary>
        ///集运单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 收货人信息
        /// </summary>
        public Consignee ConsigneeModel { get; set; }

        /// <summary>
        /// 货物信息
        /// </summary>
        public List<CargoInformation> CargoInformationList { get; set; }
        /// <summary>
        /// 合计重量
        /// </summary>
        public decimal ZWeight { get; set; }

        /// <summary>
        /// 合计体积
        /// </summary>
        public decimal CubeNum { get; set; }

        /// <summary>
        /// 实付金额
        /// </summary>
        public decimal FreeAndPackagingFeeRMB { get; set; }

    }

    /// <summary>
    /// 收货人信息
    /// </summary>
    public class Consignee
    {
        /// <summary>
        /// 收货人
        /// </summary>
        public string Cname { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 收件地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public  string PayStr { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayInt { get; set; }

        /// <summary>
        /// 收款方式
        /// </summary>
        public int ServiceInt { get; set; }


        /// <summary>
        /// 收款方式
        /// </summary>
        public string ServiceStr { get; set; }
    }

    /// <summary>
    /// 货物信息
    /// </summary>
    public class CargoInformation
    {
        /// <summary>
        /// 快递单号
        /// </summary>
        public string MainBillCode { get; set; }

        /// <summary>
        /// 货物名称
        /// </summary>
        public  string Goods { get; set; }
        /// <summary>
        /// 实际重量
        /// </summary>
        public  double DDWeight { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        public decimal Cubes { get; set; }

    }


}
