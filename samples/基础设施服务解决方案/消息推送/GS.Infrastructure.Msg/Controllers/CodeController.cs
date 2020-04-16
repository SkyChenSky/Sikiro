using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Sikiro.Infrastructure.Msg.Entitys;
using Sikiro.Infrastructure.Msg.Service;
using Sikiro.Nosql.Mongo;
using Sikiro.Nosql.Redis;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;

namespace Sikiro.Infrastructure.Msg.Controllers
{
    /// <summary>
    /// 验证码
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class CodeController : ControllerBase
    {
        private readonly RedisRepository _redisRepository;
        private readonly MongoRepository _mongoRepository;
        private readonly SmsService _smsService;
        private const string Key = "gs.infrastructure.msg:smscode:{0}";
        private const string RepeatKey = "gs.infrastructure.msg:repeatseconds:{0}";
        private readonly bool _isDebug;
        private readonly string _defaultCode;
        private readonly int _ipLimitCount;

        public CodeController(RedisRepository redisRepository, SmsService smsService, IConfiguration iConfiguration, MongoRepository mongoRepository)
        {
            _redisRepository = redisRepository;
            _smsService = smsService;
            _mongoRepository = mongoRepository;
            _isDebug = iConfiguration["IsDebug"].TryBool();
            _defaultCode = iConfiguration["DefaultCode"];
            _ipLimitCount = iConfiguration["IpLimitCount"].TryInt(10);
        }

        /// <summary>
        /// 创建验证码
        /// </summary>
        /// <param name="num">号码</param>
        /// <param name="validSeconds">有效期（秒）</param>
        /// <param name="ipAddress">ip地址</param>
        /// <param name="repeatSeconds">重复时间（秒）默认60秒</param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult Create([Required(ErrorMessage = "手机号不能为空")]string num, int validSeconds, [Required(ErrorMessage = "IP地址不能为空")] string ipAddress, int repeatSeconds = 60)
        {
            if (!_isDebug)
            {
                var isIpCheckLimit = IpCheckLimit(ipAddress);
                if (isIpCheckLimit)
                    return ServiceResult.IsFailed("该IP地址发送次数已达上限");
            }

            var isNotRepeat = _redisRepository.Add(RepeatKey.Fmt(num), "1", repeatSeconds);
            if (!isNotRepeat)
                return ServiceResult.IsFailed($"{repeatSeconds}秒内不能重复发送");

            var code = _redisRepository.GetOrAdd(Key.Fmt(num), CreateCode, validSeconds);

            if (code.IsNullOrWhiteSpace())
                return ServiceResult.IsFailed("验证码生成失败");

            _mongoRepository.Add(new SmsRecord
            {
                Content = code,
                PhoneNum = num,
                SendTime = DateTime.Now,
                IpAddress = ipAddress
            });

            if (_isDebug)
                return ServiceResult.IsSuccess("发送成功");

            var sendResult = _smsService.SendSms(num, new { code });
            if (sendResult.Failed)
                return sendResult;

            return ServiceResult.IsSuccess("发送成功");
        }

        private bool IpCheckLimit(string ipAddress)
        {
            var todayBegin = DateTime.Today;
            var todayEnd = todayBegin.AddDays(1);
            return _mongoRepository.Count<SmsRecord>(a =>
                a.IpAddress == ipAddress && todayBegin <= a.SendTime && todayEnd > a.SendTime) > _ipLimitCount;
        }

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="num">号码</param>
        /// <param name="inputCode">验证码</param>
        /// <returns></returns>
        [HttpPost]
        public ServiceResult Vaild([Required(ErrorMessage = "手机号不能为空")]string num, [Required(ErrorMessage = "验证码不能为空")] string inputCode)
        {
            if (_isDebug && _defaultCode == inputCode)
                return ServiceResult.IsSuccess("验证通过");

            var code = _redisRepository.Get(Key.Fmt(num));

            if (code.IsNullOrWhiteSpace())
                return ServiceResult.IsFailed("验证码已失效");

            if (inputCode == code)
            {
                _redisRepository.Remove(Key.Fmt(num));
                return ServiceResult.IsSuccess("验证通过");
            }

            return ServiceResult.IsFailed("验证码不正确");
        }

        private static string CreateCode()
        {
            var sb = new StringBuilder(7);
            for (var i = 0; i < 6; i++)
            {
                var codeChar = ((int)RandomHelper.RandomNext(9)).ToString();
                sb.Append(codeChar);
            }

            return sb.ToString();
        }
    }
}