using GS.Interface.Customer.User;
using GS.Tookits.Base;
using WebApiClient;
using WebApiClient.Attributes;

namespace GS.Interface.Customer
{
    /// <summary>
    /// 用户操作接口
    /// </summary>
    public interface IUser : ICustomer
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        [HttpPost("User/GetUser")]
        ITask<ServiceResult<GetUserResponse>> GetUser([JsonContent] GetUserRequest request);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        [HttpPost("User/GetUserInfo")]
        ITask<ServiceResult<GetUserInfoResponse>> GetUserInfo(string userId);

        /// <summary>
        /// 根据登录名称和手机获取用户信息
        /// </summary>
        [HttpPost("User/GetPhoneUser")]
        ITask<ServiceResult<GetPhoneUserResponse>> GetPhoneUser([JsonContent] GetPhoneUserRequest request);

        /// <summary>
        /// 登录判断
        /// </summary>
        /// <returns></returns>
        [HttpPost("User/LogonCheck")]
        ITask<ServiceResult<LogonCheckResponse>> LogonCheck([JsonContent] LogonCheckRequest request);


        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/RegisterUser")]
        ITask<ServiceResult<LogonCheckResponse>> RegisterUser([JsonContent]RegisterUserRequest request);

        /// <summary>
        /// 微信注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/RegisterWxUser")]
        ITask<ServiceResult<LogonCheckResponse>> RegisterWxUser([JsonContent]RegisterWxUserRequest request);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/ChangePassword")]
        ITask<ServiceResult> ChangePassword([JsonContent]UpdatePwdUserRequest request);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/RetrievePassword")]
        ITask<ServiceResult> RetrievePassword([JsonContent]UpdateRetrievePwdUserRequest request);

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/UpdatePhone")]
        ITask<ServiceResult> UpdatePhone([JsonContent]UpdatePhoneUserRequest request);

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/SetPayPassword")]
        ITask<ServiceResult> SetPayPassword([JsonContent]SetPayPwdUserRequest request);

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/ChangePayPassword")]
        ITask<ServiceResult> ChangePayPassword([JsonContent]ChangePayPwdUserRequest request);

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/CheckingPayPassword")]
        ITask<ServiceResult> CheckingPayPassword([JsonContent]CheckingPayPasswordRequest request);

        /// <summary>
        /// 微信登录验证
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/WxLogonCheck")]
        ITask<ServiceResult<LogonCheckResponse>> WxLogonCheck([JsonContent]WxLogonCheckRequest request);


        /// <summary>
        /// 更换或绑定微信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/BindingWx")]
        ITask<ServiceResult> BindingWx([JsonContent]BindingWxRequest request);

        #region 修改用户信息

        /// <summary>
        /// 更换用户头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/UpdateUserLogo")]
        ITask<ServiceResult> UpdateUserLogo([JsonContent] UpdateUserLogoRequest request);

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditNickName")]
        ITask<ServiceResult> EditNickName([JsonContent] UpdateNickNameRequest request);

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditUserName")]
        ITask<ServiceResult> EditUserName([JsonContent] UpdateUserNameRequest request);

        /// <summary>
        /// 修改姓名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditRealName")]
        ITask<ServiceResult> EditRealName([JsonContent] UpdateRealNameRequest request);

        /// <summary>
        /// 修改Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditEmail")]
        ITask<ServiceResult> EditEmail([JsonContent] UpdateEmailRequest request);

        /// <summary>
        /// 修改地区
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("User/EditArea")]
        ITask<ServiceResult> EditArea([JsonContent] UpdateAreaRequest request);

        #endregion

    }
}
