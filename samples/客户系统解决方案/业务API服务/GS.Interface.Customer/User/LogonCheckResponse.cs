namespace GS.Interface.Customer.User
{
    public class LogonCheckResponse
    {
        public string UserId { get; set; }

        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        public string UserNo { get; set; }
    }
}