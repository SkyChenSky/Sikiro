using GS.Tookits.Base;
using WebApiClient;
using WebApiClient.Attributes;

namespace GS.Interface.Msg
{
    /// <summary>
    /// 验证码
    /// </summary>
    public interface ICode : IMsg
    {
        /// <summary>
        /// 验证码创建
        /// </summary>
        /// <param name="num">手机号</param>
        /// <param name="validSeconds">有效时间（秒）</param>
        /// <param name="ipAddress">ip地址</param>
        /// <returns></returns>
        [HttpPost("Code/Create")]
        ITask<ServiceResult> Create(string num, int validSeconds, string ipAddress);

        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="num">号码</param>
        /// <param name="validSeconds">有效期（秒）</param>
        /// <param name="ipAddress">ip地址</param>
        /// <param name="repeatSeconds">重复时间（秒）默认60秒</param>
        /// <returns></returns>
        [HttpPost("Code/Create")]
        ITask<ServiceResult> Create(string num, int validSeconds, string ipAddress, int repeatSeconds);

        /// <summary>
        /// 验证码创建
        /// </summary>
        /// <param name="num">手机号</param>
        /// <param name="inputCode">验证码</param>
        /// <returns></returns>
        [HttpPost("Code/Vaild")]
        ITask<ServiceResult> Vaild(string num, string inputCode);
    }
}
