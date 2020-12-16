using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Sikiro.Tookits.Extension;

namespace Sikiro.Common.Utils
{
    public static class StringExtension
    {
        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string EncodePassword(this string password, string userId)
        {
            return (password.EncodeMd5String() + userId).EncodeMd5String();
        }

        /// <summary>
        /// 转换成小驼峰
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string s)
        {
            if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
            {
                return s;
            }

            var chars = s.ToCharArray();

            for (var i = 0; i < chars.Length; i++)
            {
                if (i == 1 && !char.IsUpper(chars[i]))
                {
                    break;
                }

                var hasNext = i + 1 < chars.Length;
                if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
                {
                    break;
                }

                char c;

                c = char.ToLowerInvariant(chars[i]);

                chars[i] = c;
            }

            return new string(chars);
        }

        /// <summary>
        /// 获取IHtmlBuilder
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetString(this IHtmlContent content)
        {
            var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
