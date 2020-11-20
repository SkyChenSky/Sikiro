using System;
using Sikiro.Tookits.Base.Enum;
using Sikiro.Tookits.Extension;

namespace Sikiro.Tookits.Base
{
    #region 接口层响应实体
    /// <summary>
    /// 接口层响应实体
    /// </summary>
    public class ApiResult
    {
        #region 初始化

        /// <summary>
        /// 处理码
        /// </summary>
        public ApiResultCode Code { get; protected set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; protected set; }

        #endregion

        /// <summary>
        /// 是否请求成功
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// 是否请求失败
        /// </summary>
        public bool Failed { get; protected set; }

        /// <summary>
        /// 是否请求异常
        /// </summary>
        public bool Error { get; protected set; }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult IsSuccess(string msg, ApiResultCode code = ApiResultCode.Succeed, object data = null)
        {
            return new ApiResult { Data = data, Code = code, Message = msg, Success = true };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult IsSuccess(object data)
        {
            return IsSuccess(ApiResultCode.Succeed.GetDescription(), ApiResultCode.Succeed, data);
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <returns></returns>
        public static ApiResult IsSuccess()
        {
            return IsSuccess(ApiResultCode.Succeed.GetDescription());
        }

        /// <summary>
        /// 响应异常
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult IsError(string msg, ApiResultCode code = ApiResultCode.Error, object data = null)
        {
            return new ApiResult { Data = data, Code = code, Message = msg, Error = true, Failed = true };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult IsFailed(string msg, ApiResultCode code = ApiResultCode.Failed, object data = null)
        {
            return new ApiResult { Data = data, Code = code, Message = msg, Failed = true };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <returns></returns>
        public static ApiResult IsFailed(object data)
        {
            return IsSuccess(ApiResultCode.Failed.GetDescription(), ApiResultCode.Failed, data);
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <returns></returns>
        public static ApiResult IsFailed()
        {
            return IsSuccess(ApiResultCode.Failed.GetDescription(), ApiResultCode.Failed);
        }
    }
    #endregion

    #region 接口层响应实体（泛型）
    /// <summary>
    /// 接口层响应实体（泛型）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult where T : class, new()
    {
        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> IsSuccess(string msg, ApiResultCode code = ApiResultCode.Succeed, T data = null)
        {
            return new ApiResult<T> { Data = data, Code = code, Message = msg, Success = true };
        }


        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> IsSuccess(string msg, T data)
        {
            return new ApiResult<T> { Data = data, Code = ApiResultCode.Succeed, Message = msg, Success = true };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> IsSuccess(T data)
        {
            return new ApiResult<T>
            {
                Data = data,
                Code = ApiResultCode.Succeed,
                Message = ApiResultCode.Succeed.GetDescription(),
                Success = true
            };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> IsFailed(string msg, ApiResultCode code = ApiResultCode.Failed, T data = null)
        {
            return new ApiResult<T> { Data = data, Code = code, Message = msg, Failed = true };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> IsFailed(string msg, T data)
        {
            return new ApiResult<T> { Data = data, Code = ApiResultCode.Failed, Message = msg, Failed = true };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResult<T> IsFailed(T data)
        {
            return new ApiResult<T>
            {
                Data = data,
                Code = ApiResultCode.Failed,
                Message = ApiResultCode.Failed.GetDescription(),
                Success = true
            };
        }
    }
    #endregion
}