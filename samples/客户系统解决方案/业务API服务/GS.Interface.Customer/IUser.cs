using Sikiro.Interface.Customer.User;
using Sikiro.Tookits.Base;
using WebApiClient;
using WebApiClient.Attributes;

namespace Sikiro.Interface.Customer
{
    /// <summary>
    /// 用户操作接口
    /// </summary>
    public interface IUser : ICustomer
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        [HttpPost("User/GetUserForOpenByUserId")]
        ITask<ApiResult<GetUserInfoResponse>> GetUserForOpenByUserId(string userId);

        /// <summary>
        /// 根据登录名称和手机获取用户信息
        /// </summary>
        [HttpPost("User/GetPhoneUser")]
        ITask<ApiResult<GetPhoneUserResponse>> GetPhoneUser([JsonContent] GetPhoneUserRequest request);

        /// <summary>
        /// 登录判断
        /// </summary>
        /// <returns></returns>
        [HttpPost("User/LogonCheck")]
        ITask<ApiResult<LogonCheckResponse>> LogonCheck([JsonContent] LogonCheckRequest request);


        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/RegisterUser")]
        ITask<ApiResult<LogonCheckResponse>> RegisterUser([JsonContent] RegisterUserRequest request);

        /// <summary>
        /// 微信注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/RegisterWxUser")]
        ITask<ApiResult<LogonCheckResponse>> RegisterWxUser([JsonContent] RegisterWxUserRequest request);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/ChangePassword")]
        ITask<ApiResult> ChangePassword([JsonContent] UpdatePwdUserRequest request);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/RetrievePassword")]
        ITask<ApiResult> RetrievePassword([JsonContent] UpdateRetrievePwdUserRequest request);

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/UpdatePhone")]
        ITask<ApiResult> UpdatePhone([JsonContent] UpdatePhoneUserRequest request);

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/SetPayPassword")]
        ITask<ApiResult> SetPayPassword([JsonContent] SetPayPwdUserRequest request);

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/ChangePayPassword")]
        ITask<ApiResult> ChangePayPassword([JsonContent] ChangePayPwdUserRequest request);

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/CheckingPayPassword")]
        ITask<ApiResult> CheckingPayPassword([JsonContent] CheckingPayPasswordRequest request);

        /// <summary>
        /// 微信登录验证
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/WxLogonCheck")]
        ITask<ApiResult<LogonCheckResponse>> WxLogonCheck([JsonContent] WxLogonCheckRequest request);


        /// <summary>
        /// 更换或绑定微信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/BindingWx")]
        ITask<ApiResult> BindingWx([JsonContent] BindingWxRequest request);

        #region 修改用户信息

        /// <summary>
        /// 更换用户头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/UpdateUserLogo")]
        ITask<ApiResult> UpdateUserLogo([JsonContent] UpdateUserLogoRequest request);

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditNickName")]
        ITask<ApiResult> EditNickName([JsonContent] UpdateNickNameRequest request);

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditUserName")]
        ITask<ApiResult> EditUserName([JsonContent] UpdateUserNameRequest request);

        /// <summary>
        /// 修改姓名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditRealName")]
        ITask<ApiResult> EditRealName([JsonContent] UpdateRealNameRequest request);

        /// <summary>
        /// 修改Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditEmail")]
        ITask<ApiResult> EditEmail([JsonContent] UpdateEmailRequest request);

        #endregion

    }
}
