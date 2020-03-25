using System;
using System.Globalization;

namespace Sikiro.Tookits.Extension
{
    public static class TryConvertExtension
    {
        #region 类型转换

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultNum">转换失败默认</param>
        /// <returns></returns>
        public static int TryInt(this object input, int defaultNum = 0)
        {
            if (input == null)
                return defaultNum;

            return int.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }

        /// <summary>
        /// string转long
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultNum">转换失败默认</param>
        /// <returns></returns>
        public static long TryLong(this object input, long defaultNum = 0)
        {
            if (input == null)
                return defaultNum;

            return long.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }

        /// <summary>
        /// string转double
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultNum">转换失败默认值</param>
        /// <returns></returns>
        public static double TryDouble(this object input, double defaultNum = 0)
        {
            if (input == null)
                return defaultNum;

            return double.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }

        /// <summary>
        /// string转decimal
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultNum">转换失败默认值</param>
        /// <returns></returns>
        public static decimal TryDecimal(this object input, decimal defaultNum = 0)
        {
            if (input == null)
                return defaultNum;

            return decimal.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }

        /// <summary>
        /// string转decimal
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="defaultNum">转换失败默认值</param>
        /// <returns></returns>
        public static float TryFloat(this object input, float defaultNum = 0)
        {
            if (input == null)
                return defaultNum;

            return float.TryParse(input.ToString(), out var num) ? num : defaultNum;
        }

        /// <summary>
        /// string转bool
        /// </summary>
        /// <param name="input">输入</param>
        /// <param name="falseVal"></param>
        /// <param name="defaultBool">转换失败默认值</param>
        /// <param name="trueVal"></param>
        /// <returns></returns>
        public static bool TryBool(this object input, bool defaultBool = false, string trueVal = "1", string falseVal = "0")
        {
            if (input == null)
                return defaultBool;

            var str = input.ToString();
            if (bool.TryParse(str, out var outBool))
                return outBool;

            outBool = defaultBool;

            if (trueVal == str)
                return true;
            if (falseVal == str)
                return false;

            return outBool;
        }

        /// <summary>
        /// 值类型转string
        /// </summary>
        /// <param name="inputObj">输入</param>
        /// <param name="defaultStr">转换失败默认值</param>
        /// <returns></returns>
        public static string TryString(this ValueType inputObj, string defaultStr = "")
        {
            var output = inputObj.IsNull() ? defaultStr : inputObj.ToString();
            return output;
        }

        /// <summary>
        /// 字符串转时间
        /// </summary>
        /// <param name="inputStr">输入</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime TryDateTime(this string inputStr, DateTime defaultValue = default(DateTime))
        {
            if (inputStr.IsNullOrEmpty())
                return defaultValue;

            return DateTime.TryParse(inputStr, out var outPutDateTime) ? outPutDateTime : defaultValue;
        }

        /// <summary>
        /// 字符串转时间
        /// </summary>
        /// <param name="inputStr">输入</param>
        /// <param name="formater"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime TryDateTime(this string inputStr, string formater, DateTime defaultValue = default(DateTime))
        {
            if (inputStr.IsNullOrEmpty())
                return defaultValue;

            return DateTime.TryParseExact(inputStr, formater, CultureInfo.InvariantCulture, DateTimeStyles.None, out var outPutDateTime) ? outPutDateTime : defaultValue;
        }

        /// <summary>
        /// 字符串去空格
        /// </summary>
        /// <param name="inputStr">输入</param>
        /// <returns></returns>
        public static string TryTrim(this string inputStr)
        {
            var output = inputStr.IsNullOrEmpty() ? inputStr : inputStr.Trim();
            return output;
        }

        /// <summary>
        /// 字符串转枚举
        /// </summary>
        /// <typeparam name="T">输入</typeparam>
        /// <param name="str"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T TryEnum<T>(this string str, T t = default(T)) where T : struct
        {
            return Enum.TryParse<T>(str, out var result) ? result : t;
        }
        #endregion
    }
}
