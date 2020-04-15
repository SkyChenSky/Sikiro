using GS.Common.Utils;
using GS.Entity.Customer;
using GS.Interface.Customer.User;
using GS.Service.Customer;
using GS.Service.Customer.Bo;
using GS.Service.Customer.Enums;
using GS.Tookits.Base;
using GS.Tookits.Extension;
using Microsoft.AspNetCore.Mvc;

namespace GS.InnerApi.Customer.Controllers
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
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult<GetUserResponse> GetUser(GetUserRequest request)
        {
            var user = _userService.Get(a => a.UserNo == request.UserNo && a.CompanyId == request.CompanyId && a.Status == (int)PersonEnum.Status.Open);
            if (user == null)
                return ServiceResult<GetUserResponse>.IsFailed("账户信息不存在");

            var ret = user.MapTo<GetUserResponse>();
            ret.IsChangedUserName = user.IsChangedUsername;
            ret.IsPayPwd = !string.IsNullOrEmpty(user.PayPassword);

            return ServiceResult<GetUserResponse>.IsSuccess("成功", ret);
        }

        /// <summary>
        /// 获取会员信息接口
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult<GetUserInfoResponse> GetUserInfo(string userId)
        {
            var user = _userService.Get(a => a.UserId == userId && a.Status == (int)PersonEnum.Status.Open);
            if (user == null)
                return ServiceResult<GetUserInfoResponse>.IsFailed("账户信息不存在");

            var ret = user.MapTo<GetUserInfoResponse>();
            return ServiceResult<GetUserInfoResponse>.IsSuccess("成功", ret);
        }

        /// <summary>
        /// 根据用户名获取手机号
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult<GetPhoneUserResponse> GetPhoneUser(GetPhoneUserRequest request)
        {
            var user = _userService.Get(a => (a.UserName == request.UserName || a.Phone == request.UserName) && a.Status == (int)PersonEnum.Status.Open);
            if (user == null)
            {
                return ServiceResult<GetPhoneUserResponse>.IsFailed("账户信息不存在");
            }
            return ServiceResult<GetPhoneUserResponse>.IsSuccess("成功", new GetPhoneUserResponse { Phone = user.Phone, CountryCode = user.CountryCode });
        }

        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult<AdministratorData> LogonCheck(LogonCheckRequest request)
        {
            var result = _userService.LogonCheck(request.UserName, request.Password, request.CompanyId);

            return result;
        }

        /// <summary>
        /// 修改支付流水
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moeny"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult UpdateBalance(string userId, decimal moeny)
        {
            return _userService.UpdateBalance(userId, moeny);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult<AdministratorData> RegisterUser(RegisterUserRequest request)
        {
            var user = request.MapTo<User>();
            var result = _userService.RegisterUser(user);
            return result;
        }

        /// <summary>
        /// 微信注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult<AdministratorData> RegisterWxUser(RegisterWxUserRequest request)
        {
            var user = request.MapTo<User>();
            var result = _userService.RegisterUser(user);
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult ChangePassword(UpdatePwdUserRequest request)
        {
            return _userService.ChangePassword(request.UserId, request.OldPassword, request.NewPassword);
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult RetrievePassword(UpdateRetrievePwdUserRequest request)
        {
            return _userService.SetPassword(request.CompanyId, request.Phone, request.NewPassword);
        }

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult UpdatePhone(UpdatePhoneUserRequest request)
        {
            return _userService.UpdatePhone(request.UserId, request.Phone, request.CountryCode, request.CompanyId);
        }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult SetPayPassword(SetPayPwdUserRequest request)
        {
            return _userService.SetPayPassword(request.UserId, request.PayPassword);
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult ChangePayPassword(ChangePayPwdUserRequest request)
        {
            return _userService.ChangePayPassword(request.UserId, request.OldPassword, request.NewPassword);
        }
        /// <summary>
        /// 验证支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult CheckingPayPassword(CheckingPayPasswordRequest request)
        {
            return _userService.CheckingPayPassword(request.UserId, request.PayPassword);
        }
        /// <summary>
        /// 微信登录验证
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult<AdministratorData> WxLogonCheck(WxLogonCheckRequest request)
        {
            return _userService.WxLogonCheck(request.OpenId, request.CompanyId);
        }

        /// <summary>
        /// 更换或绑定微信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult BindingWx(BindingWxRequest request)
        {
            return _userService.BindingWx(request.UserId, request.OpenId, request.WxName, request.CompanyId);
        }

        /// <summary>
        /// 更换头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult UpdateUserLogo(UpdateUserLogoRequest request)
        {
            return _userService.UpdateUserLogo(request.Id, request.ImgUrl);
        }


        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult EditNickName(UpdateNickNameRequest request)
        {
            return _userService.UpdateNickName(request.Id, request.NickName);
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult EditUserName(UpdateUserNameRequest request)
        {
            return _userService.UpdateUserName(request.Id, request.UserName, request.CompanyId);
        }

        /// <summary>
        /// 修改姓名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult EditRealName(UpdateRealNameRequest request)
        {
            return _userService.UpdateRealName(request.Id, request.RealName);
        }

        /// <summary>
        /// 修改Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult EditEmail(UpdateEmailRequest request)
        {
            return _userService.UpdateEmail(request.Id, request.Email);
        }
    }
}