using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GS.Common.Utils;
using GS.Interface.Customer;
using GS.Interface.Customer.User;
using GS.Interface.Id;
using GS.Interface.Msg;
using GS.Tookits.Base;
using GS.Tookits.Extension;
using GS.WebApi.Customer.Extention;
using GS.WebApi.Customer.Models.User;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;

namespace GS.WebApi.Customer.Controllers
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
        public async Task<ServiceResult<UserLogonResponse>> Logon(UserLogonRequest logonRequest)
        {
            var logonResult = await _iUser.LogonCheck(logonRequest.MapTo<LogonCheckRequest>());
            if (logonResult.Success)
            {
                var token = BuildJwt(logonResult.Data.MapTo<AdministratorData>());
                var response = logonResult.Data.MapTo<UserLogonResponse>();
                response.Token = token;

                return ServiceResult<UserLogonResponse>.IsSuccess("登录成功", response);
            }

            return ServiceResult<UserLogonResponse>.IsFailed(logonResult.Message);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ServiceResult<UserLogonResponse>> RegisterUser(UserRegisterRequest registerRequest)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(registerRequest.CountryCode + registerRequest.Phone, registerRequest.Code);
            if (codeVaildResult.Failed)
                return ServiceResult<UserLogonResponse>.IsFailed(codeVaildResult.Message);

            registerRequest.UserNo = await _id.Create("D4");
            registerRequest.UserName = "GS" + registerRequest.UserName;
            var result = await _iUser.RegisterUser(registerRequest.MapTo<RegisterUserRequest>());
            if (result.Success)
            {
                var token = BuildJwt(result.Data.MapTo<AdministratorData>());
                var response = result.Data.MapTo<UserLogonResponse>();
                response.Token = token;

                return ServiceResult<UserLogonResponse>.IsSuccess("登录成功", response);
            }

            return ServiceResult<UserLogonResponse>.IsFailed(result.Message);
        }

        /// <summary>
        /// 根据用户名或者手机获取用户手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetPhoneUser")]
        [AllowAnonymous]
        public async Task<ServiceResult> GetPhoneUser(UserGetPhoneResponse request)
        {
            var item = await _iUser.GetPhoneUser(request.MapTo<GetPhoneUserRequest>());
            if (item.Success)
            {
                return ServiceResult.IsSuccess("获取成功", item.Data);
            }
            else
            {
                return ServiceResult.IsFailed(item.Message);
            }
        }

        /// <summary>
        /// 发送验证短信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SendSms")]
        [AllowAnonymous]
        public async Task<ServiceResult> SendSms(SendSmsRequest request)
        {
            return await _iCode.Create(request.Phone, 300, _httpContextAccessor.HttpContext.Request.GetClientIp());
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("VaildSms")]
        [AllowAnonymous]
        public async Task<ServiceResult> VaildSms(VaildSmsRequest request)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(request.Phone, request.Code);
            if (codeVaildResult.Success)
            {
                return ServiceResult.IsSuccess("验证成功");
            }

            return ServiceResult.IsFailed("验证失败");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdatePwdUser")]
        [AllowAnonymous]
        public async Task<ServiceResult> UpdatePwdUser(UserUpdatePwdRequest request)
        {
            return await _iUser.ChangePassword(request.MapTo<UpdatePwdUserRequest>());
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("WxLogonCheck")]
        [AllowAnonymous]
        public async Task<ServiceResult<UserLogonResponse>> WxLogonCheck(UserWxLogonCheckRequest request)
        {
            //获取OPENID
            var result = OAuthApi.GetAccessToken(_appId, _appSecret, request.WxCode);

            if (result.errcode != ReturnCode.请求成功)
                return ServiceResult<UserLogonResponse>.IsFailed(result.errmsg);
            var logonResult = await _iUser.WxLogonCheck(new WxLogonCheckRequest() { OpenId = result.openid, CompanyId = request.CompanyId });
            if (logonResult.Success)
            {
                var token = BuildJwt(logonResult.Data.MapTo<AdministratorData>());
                var response = logonResult.Data.MapTo<UserLogonResponse>();
                response.Token = token;

                return ServiceResult<UserLogonResponse>.IsSuccess("登录成功", response);
            }

            return ServiceResult<UserLogonResponse>.IsFailed(logonResult.Message, new UserLogonResponse() { AccessToken = result.access_token, OpenId = result.openid });
        }
        /// <summary>
        /// 微信注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("WxRegister")]
        [AllowAnonymous]
        public async Task<ServiceResult<UserLogonResponse>> WxRegister(UserWxRegisterRequest request)
        {
            //获取微信用户信息
            var userInfo = OAuthApi.GetUserInfo(request.AccessToken, request.OpenId);

            //手机验证
            var codeVaildResult = await _iCode.Vaild(request.CountryCode + request.Phone, request.Code);
            if (codeVaildResult.Failed)
                return ServiceResult<UserLogonResponse>.IsFailed(codeVaildResult.Message);
            var user = request.MapTo<RegisterWxUserRequest>();
            user.UserName = "gs" + user.Phone;
            user.Password = "123456789";
            user.UserNo = await _id.Create("D4");
            user.OpenId = request.OpenId;
            user.WxName = userInfo.nickname;
            user.ImgUrl = userInfo.headimgurl;
            var registerResult = await _iUser.RegisterWxUser(user);
            if (registerResult.Success)
            {
                var token = BuildJwt(registerResult.Data.MapTo<AdministratorData>());
                var response = registerResult.Data.MapTo<UserLogonResponse>();
                response.Token = token;

                return ServiceResult<UserLogonResponse>.IsSuccess("登录成功", response);
            }

            return ServiceResult<UserLogonResponse>.IsFailed(registerResult.Message);
        }

        #endregion

        #region 需登录验证请求

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUser")]
        public async Task<ServiceResult<UserGetResponse>> GetUser()
        {
            var user = await _iUser.GetUser(new GetUserRequest()
            { CompanyId = CurrentUserData.CompanyId, UserNo = CurrentUserData.UserNo });
            var userInfo = new UserGetResponse();
            if (user.Success)
            {
                userInfo = user.Data.MapTo<UserGetResponse>();
                userInfo.Id = user.Data.UserId;
                userInfo.UserLogo = user.Data.ImgUrl;
            }
            return ServiceResult<UserGetResponse>.IsSuccess("获取成功", userInfo);
        }

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdatePhone")]
        public async Task<ServiceResult> UpdatePhone(UserUpdatePhoneRequest request)
        {
            //手机验证
            var codeVaildResult = await _iCode.Vaild(request.CountryCode + request.Phone, request.Code);
            if (codeVaildResult.Failed)
                return ServiceResult.IsFailed(codeVaildResult.Message);
            var ret = request.MapTo<UpdatePhoneUserRequest>();
            ret.UserId = CurrentUserData.UserId;
            ret.CompanyId = CurrentUserData.CompanyId;
            return await _iUser.UpdatePhone(ret);
        }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("SetPayPassword")]
        public async Task<ServiceResult> SetPayPassword(UserSetPayPwdRequest request)
        {
            var ret = request.MapTo<SetPayPwdUserRequest>();
            ret.UserId = CurrentUserData.UserId;
            return await _iUser.SetPayPassword(ret);
        }

        /// <summary>
        /// 修改支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ChangePayPassword")]
        public async Task<ServiceResult> ChangePayPassword(UserChangePayPwdRequest request)
        {
            var ret = request.MapTo<ChangePayPwdUserRequest>();
            ret.UserId = CurrentUserData.UserId;
            return await _iUser.ChangePayPassword(ret);
        }
        /// <summary>
        /// 验证支付密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CheckingPayPassword")]
        public async Task<ServiceResult> CheckingPayPassword(UserCheckingPayPasswordRequest request)
        {
            var ret = request.MapTo<CheckingPayPasswordRequest>();
            ret.UserId = CurrentUserData.UserId;
            return await _iUser.CheckingPayPassword(ret);
        }

        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("BindingWx")]
        public async Task<ServiceResult> BindingWx(UserBindingWxRequest request)
        {
            //获取OPENID
            var result = OAuthApi.GetAccessToken(_appId, _appSecret, request.WxCode);

            if (result.errcode != ReturnCode.请求成功)
                return ServiceResult.IsFailed(result.errmsg);
            //获取微信用户信息
            var userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
            return await _iUser.BindingWx(new BindingWxRequest() { OpenId = result.openid, UserId = CurrentUserData.UserId, WxName = userInfo.nickname, CompanyId = CurrentUserData.CompanyId });
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
        public async Task<ServiceResult> EditUserLogo(UserLogoRequest request)
        {
            var model = request.MapTo<UpdateUserLogoRequest>();
            model.ImgUrl = request.UserLogo;
            var result = await _iUser.UpdateUserLogo(model);
            if (result.Success)
            {
                return ServiceResult.IsSuccess("修改成功");
            }
            return ServiceResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改昵称
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditNickName")]
        public async Task<ServiceResult> EditNickName(NickNameRequest request)
        {
            var result = await _iUser.EditNickName(request.MapTo<UpdateNickNameRequest>());
            if (result.Success)
            {
                return ServiceResult.IsSuccess("修改成功");
            }
            return ServiceResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditUserName")]
        public async Task<ServiceResult> EditUserName(UserNameRequest request)
        {
            var model = request.MapTo<UpdateUserNameRequest>();
            model.CompanyId = CurrentUserData.CompanyId;
            var result = await _iUser.EditUserName(model);
            if (result.Success)
            {
                return ServiceResult.IsSuccess("修改成功");
            }
            return ServiceResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改姓名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditRealName")]
        public async Task<ServiceResult> EditRealName(RealNameRequest request)
        {
            var result = await _iUser.EditRealName(request.MapTo<UpdateRealNameRequest>());
            if (result.Success)
            {
                return ServiceResult.IsSuccess("修改成功");
            }
            return ServiceResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改Email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditEmail")]
        public async Task<ServiceResult> EditEmail(EmailRequest request)
        {
            var result = await _iUser.EditEmail(request.MapTo<UpdateEmailRequest>());
            if (result.Success)
            {
                return ServiceResult.IsSuccess("修改成功");
            }
            return ServiceResult.IsFailed(result.Message);
        }

        /// <summary>
        /// 修改地区
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("EditArea")]
        public async Task<ServiceResult> EditArea(AreaRequest request)
        {
            var vArea = new UpdateAreaRequest { Id = request.Id };
            UpdateArea updateArea = new UpdateArea
            {
                CityId = request.CityId,
                CityName = request.CityName,
                CountryId = request.CountryId,
                CountryName = request.CountryName
            };
            vArea.AreaRequest = updateArea;
            var result = await _iUser.EditArea(vArea);
            if (result.Success)
            {
                return ServiceResult.IsSuccess("修改成功");
            }
            return ServiceResult.IsFailed(result.Message);
        }

        #endregion

    }
}