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
        public ServiceResultCode Code { get; protected set; }

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
        /// 成功
        /// </summary>
        public bool Success => Code == ServiceResultCode.Succeed;

        /// <summary>
        /// 失败
        /// </summary>
        public bool Failed => Code == ServiceResultCode.Failed;

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <returns></returns>
        public static ServiceResult IsSuccess()
        {
            return new ServiceResult { Message = ServiceResultCode.Succeed.GetDescription(), Code = ServiceResultCode.Succeed };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <returns></returns>
        public static ServiceResult IsSuccess(string message)
        {
            return new ServiceResult { Message = message, Code = ServiceResultCode.Succeed };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <returns></returns>
        public static ServiceResult IsFailed()
        {
            return new ServiceResult { Message = ServiceResultCode.Succeed.GetDescription(), Code = ServiceResultCode.Failed };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <returns></returns>
        public static ServiceResult IsFailed(string message)
        {
            return new ServiceResult { Message = message, Code = ServiceResultCode.Failed };
        }

        /// <summary>
        /// 转换成ToApiResult
        /// </summary>
        /// <returns></returns>
        public ApiResult ToApiResult()
        {
            return this.Success ?
                ApiResult.IsSuccess(Message) :
                ApiResult.IsFailed(Message);
        }

        /// <summary>
        /// 转换成ToApiResult
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ApiResult ToApiResult(string message)
        {
            return Success ?
                ApiResult.IsSuccess(message) :
                ApiResult.IsFailed(message);
        }

        /// <summary>
        /// 转换成ToApiResult
        /// </summary>
        /// <returns></returns>
        public ApiResult<TO> ToApiResult<TO>() where TO : class, new()
        {
            var data = Data?.MapTo<TO>();
            return Success ?
                ApiResult<TO>.IsSuccess(data) :
                ApiResult<TO>.IsFailed(data);
        }

        /// <summary>
        /// 转换成ToApiResult
        /// </summary>
        /// <returns></returns>
        public ApiResult<TO> ToApiResult<TO>(TO data) where TO : class, new()
        {
            return Success ?
                ApiResult<TO>.IsSuccess(data) :
                ApiResult<TO>.IsFailed(data);
        }
    }
    #endregion

    #region 服务层响应实体（泛型）
    /// <summary>
    /// 服务层响应实体（泛型）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T> : ServiceResult where T : class, new()
    {
        /// <summary>
        /// 数据
        /// </summary>
        public new T Data { get; private set; }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> IsSuccess(string msg, T data = null)
        {
            return new ServiceResult<T> { Data = data, Message = msg, Code = ServiceResultCode.Succeed };
        }

        /// <summary>
        /// 响应成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> IsSuccess(T data)
        {
            return new ServiceResult<T> { Data = data, Message = ServiceResultCode.Succeed.GetDescription(), Code = ServiceResultCode.Succeed };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> IsFailed(string msg, T data = null)
        {
            return new ServiceResult<T> { Data = data, Message = msg, Code = ServiceResultCode.Failed };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResult<T> IsFailed(T data)
        {
            return new ServiceResult<T> { Data = data, Message = ServiceResultCode.Succeed.GetDescription(), Code = ServiceResultCode.Failed };
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <returns></returns>
        public new static ServiceResult<T> IsFailed()
        {
            return IsFailed(null);
        }

        /// <summary>
        /// 转换成ToApiResult
        /// </summary>
        /// <returns></returns>
        public new ApiResult<TO> ToApiResult<TO>() where TO : class, new()
        {
            var data = Data?.MapTo<TO>();
            return Success ?
                ApiResult<TO>.IsSuccess(data) :
                ApiResult<TO>.IsFailed(data);
        }

        /// <summary>
        /// 转换成ToApiResult
        /// </summary>
        /// <returns></returns>
        public new ApiResult<TO> ToApiResult<TO>(TO data) where TO : class, new()
        {
            return Success ?
                ApiResult<TO>.IsSuccess(data) :
                ApiResult<TO>.IsFailed(data);
        }
    }
    #endregion
}