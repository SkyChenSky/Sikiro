using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Sikiro.Common.Utils;
using Sikiro.Interface.Customer;
using Sikiro.Interface.Customer.User;
using Sikiro.Interface.Id;
using Sikiro.Interface.Msg;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.WebApi.Customer.Extention;
using Sikiro.WebApi.Customer.Models.User;
using Sikiro.WebApi.Customer.Models.User.Request;
using Sikiro.WebApi.Customer.Models.User.Response;

namespace Sikiro.WebApi.Customer.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class UserController : BaseController
    {
        private readonly string _appId;
        private readonly string _appSecret;
        private readonly IUser _iUser;
        private readonly ICode _iCode;
        private readonly IId _id;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IConfiguration iConfiguration, IUser iUser, ICode iCode, IId id, IHttpContextAccessor httpContextAccessor)
        {
            var iConfiguration1 = iConfiguration;
            _appId = iConfiguration1["wechat:appId"];
            _appSecret = iConfiguration1["wechat:appSecret"];
            _iUser = iUser;
            _iCode = iCode;
            _id = id;
            _httpContextAccessor = httpContextAccessor;
        }

        #region 无登录验证请求
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="logonRequest"></param>
        /// <returns></returns>
        [HttpPost("logon")]
        [AllowAnonymous]
        public async Task<ApiResult<UserLogonResponse>> Logon(UserLogonRequest logonRequest)
        {
            var logonResult = await _iUser.LogonCheck(logonRequest.MapTo<LogonCheckRequest>());
            if (logonResult.Failed)
                return ApiResult<UserLogonResponse>.IsFailed(logonResult.Message);

            var token = BuildJwt(logonResult.Data.MapTo<AdministratorData>());
            var response = logonResult.Data.MapTo<UserLogonResponse>();
            response.Token = token;

            return ApiResult<UserLogonResponse>.IsSuccess(logonResult.Message, response);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ApiResult<UserLogonResponse>> RegisterUser(UserRegisterRequest registerRequest)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(registerRequest.CountryCode + registerRequest.Phone, registerRequest.Code);
            if (codeVaildResult.Failed)
                return codeVaildResult.ToApiResult<UserLogonResponse>();

            registerRequest.UserNo = await _id.Create("D4");
            registerRequest.UserName = "GS" + registerRequest.UserName;
            var registerResult = await _iUser.RegisterUser(registerRequest.MapTo<RegisterUserRequest>());

            if (registerResult.Failed)
                return ApiResult<UserLogonResponse>.IsFailed("注册成功");

            var token = BuildJwt(registerResult.Data.MapTo<AdministratorData>());
            var response = registerResult.Data.MapTo<UserLogonResponse>();
            response.Token = token;

            return ApiResult<UserLogonResponse>.IsSuccess("注册成功", response);
        }

        /// <summary>
        /// 根据用户名或者手机获取用户手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetPhoneUser")]
        [AllowAnonymous]
        public async Task<ApiResult> GetPhoneUser(UserGetPhoneResponse request)
        {
            var item = await _iUser.GetPhoneUser(request.MapTo<GetPhoneUserRequest>());
            if (item.Success)
                return ApiResult.IsSuccess(item.Data);

            return ApiResult.IsFailed(item.Message);
        }

        /// <summary>
        /// 发送验证短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SendSms")]
        [AllowAnonymous]
        public async Task<ApiResult> SendSms(SendSmsRequest request)
        {
            var result = await _iCode.Create(request.Phone, 300, _httpContextAccessor.HttpContext.Request.GetClientIp());

            return result.ToApiResult();
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("VaildSms")]
        [AllowAnonymous]
        public async Task<ApiResult> VaildSms(VaildSmsRequest request)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(request.Phone, request.Code);

            return codeVaildResult.ToApiResult();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdatePwdUser")]
        [AllowAnonymous]
        public async Task<ApiResult> UpdatePwdUser(UserUpdatePwdRequest request)
        {
            var result = await _iUser.ChangePassword(request.MapTo<UpdatePwdUserRequest>());

            return result;
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("WxLogonCheck")]
        [AllowAnonymous]
        public async Task<ApiResult<UserLogonResponse>> WxLogonCheck(UserWxLogonCheckRequest request)
        {
            //获取OPENID
            var result = OAuthApi.GetAccessToken(_appId, _appSecret, request.WxCode);

            if (result.errcode != ReturnCode.请求成功)
                return ApiResult<UserLogonResponse>.IsFailed(result.errmsg);

            var logonResult = await _iUser.WxLogonCheck(new WxLogonCheckRequest { OpenId = result.openid, CompanyId = request.CompanyId });
            if (logonResult.Failed)
                return ApiResult<UserLogonResponse>.IsFailed(logonResult.Message);

            var token = BuildJwt(logonResult.Data.MapTo<AdministratorData>());
            var response = logonResult.Data.MapTo<UserLogonResponse>();
            response.Token = token;

            return ApiResult<UserLogonResponse>.IsSuccess(logonResult.Message, response);
        }

        /// <summary>
        /// 微信注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("WxRegister")]
        [AllowAnonymous]
        public async Task<ApiResult<UserLogonResponse>> WxRegister(UserWxRegisterRequest request)
        {
            //获取微信用户信息
            var userInfo = OAuthApi.GetUserInfo(request.AccessToken, request.OpenId);

            //手机验证
            var codeVaildResult = await _iCode.Vaild(request.CountryCode + request.Phone, request.Code);
            if (codeVaildResult.Failed)
                return codeVaildResult.ToApiResult<UserLogonResponse>();

            var user = request.MapTo<RegisterWxUserRequest>();

            user.UserName = "gs" + user.Phone;
            user.Password = Guid.NewGuid().ToString("N");
            user.UserNo = await _id.Create("D4");
            user.OpenId = request.OpenId;
            user.WxName = userInfo.nickname;
            user.ImgUrl = userInfo.headimgurl;

            var registerResult = await _iUser.RegisterWxUser(user);
            if (registerResult.Failed)
                return ApiResult<UserLogonResponse>.IsFailed("登录失败");

            var token = BuildJwt(registerResult.Data.MapTo<AdministratorData>());
            var response = registerResult.Data.MapTo<UserLogonResponse>();
            response.Token = token;

            return ApiResult<UserLogonResponse>.IsSuccess("登录成功", response);
        }

        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("BindingWx")]
        public async Task<ApiResult> BindingWx(UserBindingWxRequest request)
        {
            //获取OPENID
            var token = OAuthApi.GetAccessToken(_appId, _appSecret, request.WxCode);

            if (token.errcode != ReturnCode.请求成功)
                return ApiResult.IsFailed(token.errmsg);

            //获取微信用户信息
            var userInfo = OAuthApi.GetUserInfo(token.access_token, token.openid);
            var result = await _iUser.BindingWx(new BindingWxRequest
            {
                OpenId = token.openid,
                UserId = CurrentUserData.UserId,
                WxName = userInfo.nickname,
                CompanyId = CurrentUserData.CompanyId
            });

            return result;
        }

        #endregion

        #region 需登录验证请求

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUser")]
        public async Task<ApiResult<UserGetResponse>> GetUser()
        {
            var user = await _iUser.GetUserForOpenByUserId(CurrentUserData.UserNo);
            var userInfo = new UserGetResponse();
            if (user.Success)
                userInfo = user.Data.MapTo<UserGetResponse>();

            return ApiResult<UserGetResponse>.IsSuccess("获取成功", userInfo);
        }

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdatePhone")]
        public async Task<ApiResult> UpdatePhone(UserUpdatePhoneRequest request)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(request.CountryCode + request.Phone, request.Code);
            if (codeVaildResult.Failed)
                return codeVaildResult.ToApiResult();

            var ret = request.MapTo<UpdatePhoneUserRequest>();
            ret.UserId = CurrentUserData.UserId;
            ret.CompanyId = CurrentUserData.CompanyId;

            var result = await _iUser.UpdatePhone(ret);

            return result;
        }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SetPayPassword")]
        public async Task<ApiResult> SetPayPassword(UserSetPayPwdRequest request)
        {
            var ret = request.MapTo<SetPayPwdUserRequest>();
            ret.UserId = CurrentUserData.UserId;

            var result = await _iUser.SetPayPassword(ret);

            return result;
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ChangePayPassword")]
        public async Task<ApiResult> ChangePayPassword(UserChangePayPwdRequest request)
        {
            var ret = request.MapTo<ChangePayPwdUserRequest>();
            ret.UserId = CurrentUserData.UserId;

            var result = await _iUser.ChangePayPassword(ret);

            return result;
        }
        /// <summary>
        /// 验证支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CheckingPayPassword")]
        public async Task<ApiResult> CheckingPayPassword(UserCheckingPayPasswordRequest request)
        {
            var ret = request.MapTo<CheckingPayPasswordRequest>();
            ret.UserId = CurrentUserData.UserId;
            var result = await _iUser.CheckingPayPassword(ret);

            return result;
        }
        #endregion

        #region 辅助方法

        /// <summary>
        /// 创建JWT
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        private static string BuildJwt(AdministratorData userData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddYears(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtClaimTypes.Id, userData.UserId),
                    new Claim(ConstString.JwtCompanyId, userData.CompanyId),
                    new Claim(ConstString.KeyName, ConstString.KeyValue),
                    new Claim(ClaimTypes.UserData,userData.ToJson())
                }),
                Expires = expiresAt,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConstString.AuthKey)),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion

        #region 个人信息维护

        /// <summary>
        /// 更换用户头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditUserLogo")]
        public async Task<ApiResult> EditUserLogo(UserLogoRequest request)
        {
            var model = request.MapTo<UpdateUserLogoRequest>();

            var result = await _iUser.UpdateUserLogo(model);
            if (result.Success)
                return ApiResult.IsSuccess("修改成功");

            return ApiResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditNickName")]
        public async Task<ApiResult> EditNickName(NickNameRequest request)
        {
            var result = await _iUser.EditNickName(request.MapTo<UpdateNickNameRequest>());
            if (result.Success)
                return ApiResult.IsSuccess("修改成功");

            return ApiResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditUserName")]
        public async Task<ApiResult> EditUserName(UserNameRequest request)
        {
            var model = request.MapTo<UpdateUserNameRequest>();
            model.CompanyId = CurrentUserData.CompanyId;

            var result = await _iUser.EditUserName(model);
            if (result.Success)
                return ApiResult.IsSuccess("修改成功");

            return ApiResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改姓名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditRealName")]
        public async Task<ApiResult> EditRealName(RealNameRequest request)
        {
            var result = await _iUser.EditRealName(request.MapTo<UpdateRealNameRequest>());
            if (result.Success)
                return ApiResult.IsSuccess("修改成功");

            return ApiResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditEmail")]
        public async Task<ApiResult> EditEmail(EmailRequest request)
        {
            var result = await _iUser.EditEmail(request.MapTo<UpdateEmailRequest>());

            return result;
        }

        #endregion

    }
}