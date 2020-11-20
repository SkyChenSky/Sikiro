using System.ComponentModel;

namespace Sikiro.Tookits.Base.Enum
{
    /// <summary>
    /// 服务层响应码枚举
    /// </summary>
    public enum ServiceResultCode
    {
        [Description("处理成功")]
        Succeed = 0,
        [Description("处理失败")]
        Failed = 1,
    }
}
