using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Sikiro.Application.Customer.Ao;
using Sikiro.Common.Utils;
using Sikiro.Entity.Customer;
using Sikiro.Interface.Id;
using Sikiro.Interface.Msg;
using Sikiro.Service.Customer;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Interfaces;
using WebApiClient.Attributes;

namespace Sikiro.Application.Customer
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class UserApplication : IDepend
    {
        private readonly ICode _iCode;
        private readonly IId _id;
        private readonly UserService _iUser;

        public UserApplication(ICode iCode, IId id, UserService iUser)
        {
            _iCode = iCode;
            _id = id;
            _iUser = iUser;
        }

        #region 无登录验证请求

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="logonRequest"></param>
        /// <returns></returns>
        public ServiceResult<AdministratorData> Logon(UserLogonInputAo logonRequest)
        {
            var logonResult = _iUser.LogonCheck(logonRequest.UserName, logonRequest.Password, logonRequest.CompanyId);
            return logonResult;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        public async Task<ServiceResult<AdministratorData>> RegisterUser(UserRegisterInputAo registerRequest)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(registerRequest.CountryCode + registerRequest.Phone, registerRequest.Code);
            if (codeVaildResult.Failed)
                return ServiceResult<AdministratorData>.IsFailed(codeVaildResult.Message);

            registerRequest.UserNo = await _id.Create("D4");
            registerRequest.UserName = "GS" + registerRequest.UserName;
            var registerResult = _iUser.RegisterUser(registerRequest.MapTo<User>());

            return registerResult;
        }

        /// <summary>
        /// 发送验证短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult> SendSms(SendSmsAo request)
        {
            return await _iCode.Create(request.Phone, 300, request.Ip);
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult> VaildSms(VaildSmsAo request)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(request.Phone, request.Code);

            return codeVaildResult;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ServiceResult UpdatePwdUser(UserUpdatePwdAo request)
        {
            var result = _iUser.ChangePassword(request.UserId, request.OldPassword, request.OldPassword);

            return result;
        }

        #endregion

        #region 需登录验证请求

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public ServiceResult<User> GetUser(string userId)
        {
            var user = _iUser.GetUserForOpenByUserId(userId);
            return user;
        }

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdatePhone(UserUpdatePhoneAo request)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(request.CountryCode + request.Phone, request.Code);
            if (codeVaildResult.Failed)
                return codeVaildResult;

            var result = _iUser.UpdatePhone(request.UserId, request.Phone, request.CountryCode, request.CompanyId);

            return result;
        }

        #endregion
    }
}