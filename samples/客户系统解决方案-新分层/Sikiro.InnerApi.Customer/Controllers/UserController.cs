using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Common.Utils;
using Sikiro.Entity.Customer;
using Sikiro.Interface.Customer.User;
using Sikiro.Service.Customer;
using Sikiro.Service.Customer.Enums;

namespace Sikiro.InnerApi.Customer.Controllers
{
    /// <summary>
    /// 快运单接口
    /// </summary>
    public class UserController : BaseController
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 获取会员信息接口
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<GetUserInfoResponse> GetUserForOpenByUserId(string userId)
        {
            var userResult = _userService.GetUserForOpenByUserId(userId);

            return userResult.ToApiResult<GetUserInfoResponse>();
        }

        /// <summary>
        /// 根据用户名获取手机号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<GetPhoneUserResponse> GetPhoneUser(GetPhoneUserRequest request)
        {
            var user = _userService.GetUserForOpenByUserNameOrPhone(request.UserName);

            return user.ToApiResult(new GetPhoneUserResponse
            {
                Phone = user.Data.Phone,
                CountryCode = user.Data.CountryCode
            });
        }

        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<AdministratorData> LogonCheck(LogonCheckRequest request)
        {
            var result = _userService.LogonCheck(request.UserName, request.Password, request.CompanyId);

            return result.ToApiResult<AdministratorData>();
        }

        /// <summary>
        /// 修改支付流水
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moeny"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult UpdateBalance(string userId, decimal moeny)
        {
            return _userService.UpdateBalance(userId, moeny).ToApiResult();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<AdministratorData> RegisterUser(RegisterUserRequest request)
        {
            var user = request.MapTo<User>();
            var result = _userService.RegisterUser(user);
            return result.ToApiResult<AdministratorData>();
        }

        /// <summary>
        /// 微信注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<AdministratorData> RegisterWxUser(RegisterWxUserRequest request)
        {
            var user = request.MapTo<User>();
            var result = _userService.RegisterUser(user);
            return result.ToApiResult<AdministratorData>();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult ChangePassword(UpdatePwdUserRequest request)
        {
            return _userService.ChangePassword(request.UserId, request.OldPassword, request.NewPassword).ToApiResult();
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult RetrievePassword(UpdateRetrievePwdUserRequest request)
        {
            return _userService.SetPassword(request.CompanyId, request.Phone, request.NewPassword).ToApiResult();
        }

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult UpdatePhone(UpdatePhoneUserRequest request)
        {
            return _userService.UpdatePhone(request.UserId, request.Phone, request.CountryCode, request.CompanyId).ToApiResult();
        }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult SetPayPassword(SetPayPwdUserRequest request)
        {
            return _userService.SetPayPassword(request.UserId, request.PayPassword).ToApiResult();
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult ChangePayPassword(ChangePayPwdUserRequest request)
        {
            return _userService.ChangePayPassword(request.UserId, request.OldPassword, request.NewPassword).ToApiResult();
        }
        /// <summary>
        /// 验证支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult CheckingPayPassword(CheckingPayPasswordRequest request)
        {
            return _userService.CheckingPayPassword(request.UserId, request.PayPassword).ToApiResult();
        }
        /// <summary>
        /// 微信登录验证
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult<AdministratorData> WxLogonCheck(WxLogonCheckRequest request)
        {
            return _userService.WxLogonCheck(request.OpenId, request.CompanyId).ToApiResult<AdministratorData>();
        }

        /// <summary>
        /// 更换或绑定微信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult BindingWx(BindingWxRequest request)
        {
            return _userService.BindingWx(request.UserId, request.OpenId, request.WxName, request.CompanyId).ToApiResult();
        }

        /// <summary>
        /// 更换头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult UpdateUserLogo(UpdateUserLogoRequest request)
        {
            return _userService.UpdateUserLogo(request.Id, request.ImgUrl).ToApiResult();
        }


        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult EditNickName(UpdateNickNameRequest request)
        {
            return _userService.UpdateNickName(request.Id, request.NickName).ToApiResult();
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult EditUserName(UpdateUserNameRequest request)
        {
            return _userService.UpdateUserName(request.Id, request.UserName, request.CompanyId).ToApiResult();
        }

        /// <summary>
        /// 修改姓名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult EditRealName(UpdateRealNameRequest request)
        {
            return _userService.UpdateRealName(request.Id, request.RealName).ToApiResult();
        }

        /// <summary>
        /// 修改Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResult EditEmail(UpdateEmailRequest request)
        {
            return _userService.UpdateEmail(request.Id, request.Email).ToApiResult();
        }
    }
}