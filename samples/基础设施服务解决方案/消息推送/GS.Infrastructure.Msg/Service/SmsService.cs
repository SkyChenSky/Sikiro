using System;
using System.Text;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.Infrastructure.Msg.Service
{
    public class SmsService
    {
        private readonly SmsConfig _cfg;
        private const string Domain = "dysmsapi.aliyuncs.com";
        public SmsService(string key, string secret, string sign, string code, string foreignsign, string foreigncode)
        {
            _cfg = new SmsConfig
            {
                AccessKeyId = key,
                AccessSecret = secret,
                SignName = sign,
                TemplateCode = code,
                ForeignSignName = foreignsign,
                ForeignTemplateCode = foreigncode
            };

            if (string.IsNullOrEmpty(_cfg.AccessKeyId) || string.IsNullOrEmpty(_cfg.AccessSecret) ||
                string.IsNullOrEmpty(_cfg.SignName) || string.IsNullOrEmpty(_cfg.TemplateCode) || string.IsNullOrEmpty(_cfg.ForeignSignName) || string.IsNullOrEmpty(_cfg.ForeignTemplateCode))
                throw new Exception("配置信息不能为空");
        }

        public ServiceResult SendSms(string phone, object captchaJson)
        {
            var templateCode = _cfg.TemplateCode;
            var signName = _cfg.SignName;
            if (phone.StartsWith("+86", StringComparison.CurrentCultureIgnoreCase))
            {
                phone = phone.Substring(3);
            }
            else if (phone.StartsWith("+", StringComparison.CurrentCultureIgnoreCase))
            {
                templateCode = _cfg.ForeignTemplateCode;
                signName = _cfg.ForeignSignName;
            }
            phone = phone.TrimStart('+');

            var request = new CommonRequest
            {
                Method = MethodType.POST,
                Domain = Domain,
                Version = "2017-05-25",
                Action = "SendSms"
            };
            request.AddQueryParameters("PhoneNumbers", phone);
            request.AddQueryParameters("SignName", signName);
            request.AddQueryParameters("TemplateCode", templateCode);
            request.AddQueryParameters("TemplateParam", captchaJson.ToJson());

            IClientProfile profile = DefaultProfile.GetProfile("default", _cfg.AccessKeyId, _cfg.AccessSecret);
            var client = new DefaultAcsClient(profile);
            var response = client.GetCommonResponse(request);
            var result = Encoding.UTF8.GetString(response.HttpResponse.Content);
            var sendSmsResponse = result.FromJson<SendSmsResponse>();

            var code = sendSmsResponse.Code;
            if (code.Equals("isv.BUSINESS_LIMIT_CONTROL"))
                return ServiceResult.IsFailed("该手机号发送次数已达上限");
            if (code.Equals("isv.MOBILE_NUMBER_ILLEGAL"))
                return ServiceResult.IsFailed("该手机号无效，请检查国区号");

            var returnResult = sendSmsResponse.Code == "OK";
            return returnResult ? ServiceResult.IsSuccess("") : ServiceResult.IsFailed(sendSmsResponse.Message);
        }
    }

    public class SmsConfig
    {
        public string AccessKeyId { set; get; }
        public string AccessSecret { set; get; }
        public string SignName { set; get; }
        public string TemplateCode { set; get; }
        public string ForeignSignName { set; get; }
        public string ForeignTemplateCode { set; get; }
    }


    public class SendSmsResponse : AcsResponse
    {
        public new string RequestId { get; set; }

        public string BizId { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
