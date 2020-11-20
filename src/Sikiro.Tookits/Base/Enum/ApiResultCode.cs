using System.ComponentModel;

namespace Sikiro.Tookits.Base.Enum
{
    public enum ApiResultCode
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        Succeed = 200,

        /// <summary>
        /// 请求失败
        /// </summary>
        [Description("请求失败")]
        Failed = 400,

        /// <summary>
        /// 服务执行异常
        /// </summary>
        [Description("服务执行异常")]
        Error = 500
    }
}
