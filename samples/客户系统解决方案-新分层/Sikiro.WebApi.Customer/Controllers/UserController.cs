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
using Sikiro.Application.Customer;
using Sikiro.Application.Customer.Ao;
using Sikiro.Common.Utils;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.WebApi.Customer.Extention;
using Sikiro.WebApi.Customer.Models.User.Request;
using Sikiro.WebApi.Customer.Models.User.Response;

namespace Sikiro.WebApi.Customer.Controllers
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserApplication _userApplication;

        public UserController(IHttpContextAccessor httpContextAccessor, UserApplication userApplication)
        {
            _httpContextAccessor = httpContextAccessor;
            _userApplication = userApplication;
        }

        #region 无登录验证请求
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="logonRequest"></param>
        /// <returns></returns>
        [HttpPost("logon")]
        [AllowAnonymous]
        public ApiResult<UserLogonResponse> Logon(UserLogonRequest logonRequest)
        {
            var result = _userApplication.Logon(logonRequest.MapTo<UserLogonInputAo>());
            if (result.Failed)
                return ApiResult<UserLogonResponse>.IsFailed(result.Message);

            var token = BuildJwt(result.Data);
            var response = result.Data.MapTo<UserLogonResponse>();
            response.Token = token;

            return ApiResult<UserLogonResponse>.IsSuccess(result.Message, response);
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
            var result = await _userApplication.RegisterUser(registerRequest.MapTo<UserRegisterInputAo>());

            if (result.Failed)
                return ApiResult<UserLogonResponse>.IsFailed("注册成功");

            var token = BuildJwt(result.Data);
            var response = result.Data.MapTo<UserLogonResponse>();
            response.Token = token;
            return ApiResult<UserLogonResponse>.IsSuccess(response);
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
            var ao = new SendSmsAo
            {
                Ip = _httpContextAccessor.HttpContext.Request.GetClientIp(),
                Phone = request.Phone
            };
            var result = await _userApplication.SendSms(ao);

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
            var result = await _userApplication.VaildSms(request.MapTo<VaildSmsAo>());

            return result.ToApiResult();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdatePwdUser")]
        [AllowAnonymous]
        public ApiResult UpdatePwdUser(UserUpdatePwdRequest request)
        {
            var result = _userApplication.UpdatePwdUser(request.MapTo<UserUpdatePwdAo>());

            return result.ToApiResult();
        }

        #endregion

        #region 需登录验证请求

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetUser")]
        public ApiResult<UserGetResponse> GetUser()
        {
            var user = _userApplication.GetUser(CurrentUserData.UserNo);

            return user.ToApiResult<UserGetResponse>();
        }

        /// <summary>
        /// 更换绑定手机
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UpdatePhone")]
        public async Task<ApiResult> UpdatePhone(UserUpdatePhoneRequest request)
        {
            var result = await _userApplication.UpdatePhone(request.MapTo<UserUpdatePhoneAo>());

            return result.ToApiResult();
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

    }
}