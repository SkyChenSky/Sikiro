using System;
using Sikiro.Tookits.Base.Enum;
using Sikiro.Tookits.Extension;

namespace Sikiro.Tookits.Base
{
    #region 服务层响应实体
    /// <summary>
    /// 服务层响应实体
    /// </summary>
    public class ServiceResult
    {
        #region 初始化
        public ServiceResultCode ResultCode { set; get; }

        private string _message;

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message
        {
            set => _message = value;
            get
            {
                if (!_message.IsNullOrEmpty())
                    return _message;

                if (Exception.IsNotNull())
                    return Exception.Message;

                return null;
            }
        }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        public ServiceResult()
        {
        }

        public ServiceResult(ServiceResultCode resultCode)
        {
            ResultCode = resultCode;
        }

        public ServiceResult(string msg, ServiceResultCode resultCode)
        {
            Message = msg;
            ResultCode = resultCode;
        }

        public ServiceResult(Exception ex, ServiceResultCode resultCode)
        {
            Exception = ex;
            ResultCode = resultCode;
        }

        public ServiceResult(Exception ex, string msg, ServiceResultCode resultCode, object data)
        {
            Message = msg;
            Exception = ex;
            ResultCode = resultCode;
            Data = data;
        }
        #endregion

        /// <summary>
        /// 成功
        /// </summary>
        public bool Success => ResultCode == ServiceResultCode.Succeed;

        /// <summary>
        /// 错误
        /// </summary>
        public bool Error => ResultCode == ServiceResultCode.Error;

        /// <summary>
        /// 失败
        /// </summary>
        public bool Failed => ResultCode == ServiceResultCode.Error || ResultCode == ServiceResultCode.Failed;

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult IsSuccess(string msg, object data = null)
        {
            return new ServiceResult(msg, ServiceResultCode.Succeed) { Data = data };
        }

        /// <summary>
        /// 响应错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult IsError(Exception ex, object data = null)
        {
            return new ServiceResult(ex, ServiceResultCode.Error) { Data = data };
        }

        /// <summary>
        /// 响应错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult IsError(string msg, object data = null)
        {
            return new ServiceResult(msg, ServiceResultCode.Error) { Data = data };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult IsFailed(string msg, object data = null)
        {
            return new ServiceResult(msg, ServiceResultCode.Failed) { Data = data };
        }
    }
    #endregion

    #region 服务层响应实体（泛型）
    /// <summary>
    /// 服务层响应实体（泛型）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T> where T : class, new()
    {
        #region 初始化
        public ServiceResultCode ResultCode { set; get; }

        private string _message;

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message
        {
            set => _message = value;
            get
            {
                if (!_message.IsNullOrEmpty())
                    return _message;

                if (Exception.IsNotNull())
                    return Exception.Message;

                return null;
            }
        }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }

        public ServiceResult()
        {
        }

        public ServiceResult(string msg, ServiceResultCode resultCode)
        {
            Message = msg;
            ResultCode = resultCode;
        }

        public ServiceResult(Exception ex, ServiceResultCode resultCode)
        {
            Exception = ex;
            ResultCode = resultCode;
        }

        #endregion
        /// <summary>
        /// 成功
        /// </summary>
        public bool Success => ResultCode == ServiceResultCode.Succeed;

        /// <summary>
        /// 错误
        /// </summary>
        public bool Error => ResultCode == ServiceResultCode.Error;

        /// <summary>
        /// 失败
        /// </summary>
        public bool Failed => ResultCode == ServiceResultCode.Error || ResultCode == ServiceResultCode.Failed;

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> IsSuccess(string msg, T data = null)
        {
            return new ServiceResult<T>(msg, ServiceResultCode.Succeed) { Data = data };
        }

        /// <summary>
        /// 响应错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> IsError(Exception ex, T data = null)
        {
            return new ServiceResult<T>(ex, ServiceResultCode.Error) { Data = data };
        }

        /// <summary>
        /// 响应错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> IsError(string msg, T data = null)
        {
            return new ServiceResult<T>(msg, ServiceResultCode.Error) { Data = data };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> IsFailed(string msg, T data = null)
        {
            return new ServiceResult<T>(msg, ServiceResultCode.Failed) { Data = data };
        }
    }
    #endregion
}