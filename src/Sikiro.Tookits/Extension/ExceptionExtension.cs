using System;
using Sikiro.Tookits.Helper;

namespace Sikiro.Tookits.Extension
{
    public static class ExceptionExtension
    {
        #region 获取最底层异常
        /// <summary>
        /// 获取最底层异常
        /// </summary>
        public static Exception GetDeepestException(this Exception ex)
        {
            var innerException = ex.InnerException;
            var resultExcpetion = ex;
            while (innerException != null)
            {
                resultExcpetion = innerException;
                innerException = innerException.InnerException;
            }
            return resultExcpetion;
        }
        #endregion

        #region 异常文本日志

        public static void WriteToFile(this Exception ex, string message)
        {
            LoggerHelper.WriteToFile(message, ex);
        }
        #endregion
    }
}
