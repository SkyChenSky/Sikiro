using System;
using System.Collections.Generic;
using Sikiro.Tookits.Extension;

namespace Sikiro.Tookits.Base
{
    /// <summary>
    /// 系统错误信息
    /// </summary>
    public class ExceptionMsg
    {
        public ExceptionMsg()
        {

        }
        public ExceptionMsg(Exception ex, IDictionary<string, object> actionArguments)
        {
            Exception = ex;
            DateTime = DateTime.Now;
            ActionArguments = actionArguments;
            DotNetVersion = Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;
            OSVersion = Environment.OSVersion.ToString();
        }

        public Exception Exception { get; set; }

        /// <summary>
        /// 异常类型
        /// </summary>
        public string TypeName => Exception.GetType().Name;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message => Exception.GetDeepestException()?.Message ?? Exception.Message;

        /// <summary>
        /// 程序集名称
        /// </summary>
        public string Assembly => Exception.TargetSite.Module.Assembly.FullName;

        /// <summary>
        /// 异常参数
        /// </summary>
        public IDictionary<string, object> ActionArguments { get; private set; }

        /// <summary>
        /// 异常堆栈
        /// </summary>
        public string StackTrace => Exception.StackTrace;

        /// <summary>
        /// 异常源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 服务器IP 端口
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 浏览器名称
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// .NET解释引擎版本
        /// </summary>
        public string DotNetVersion { get; private set; }

        /// <summary>
        /// 操作系统类型
        /// </summary>
        public string OSVersion { get; private set; }

        /// <summary>
        /// 异常发生时间
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// 异常发生方法
        /// </summary>
        public string Method => Exception.TargetSite.Name;

    }
}
