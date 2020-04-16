using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Sikiro.Tookits.Base;
using Sikiro.Tookits.Extension;

namespace Sikiro.Common.Utils
{
   public static class EnumberHelper
    {
        /// <summary>
        /// 枚举转下拉框
        /// </summary>
        /// <param name="type"></param>
        /// <param name="defaultText">默认文本</param>
        /// <returns></returns>
        public static List<DropDownItem> ToDropDownList(this Type type, string defaultText = null)
        {
            var result = (from object item in Enum.GetValues(type)
                    select new DropDownItem
                    {
                        Text = item.GetType().GetField(item.ToString()).GetDisplayName(),
                        Value = item,
                        Description = item.GetType().GetField(item.ToString()).GetDisplayDescription(),
                        Prompt = item.GetType().GetField(item.ToString()).GetDisplayPrompt(),
                    })
                .ToList();

            if (!defaultText.IsNullOrWhiteSpace())
                result.Insert(0, new DropDownItem
                {
                    Text = defaultText,
                    Value = ""
                });

            return result;
        }

        private static string GetDisplayName(this FieldInfo field)
        {
            var att = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute), false);
            return att == null ? field.Name : ((DisplayAttribute)att).Name;
        }
        private static string GetDisplayDescription(this FieldInfo field)
        {
            var att = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute), false);
            return att == null ? field.GetDisplayDescription() : ((DisplayAttribute)att).Description;
        }
        private static string GetDisplayPrompt(this FieldInfo field)
        {
            var att = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute), false);
            return att == null ? field.GetDisplayPrompt() : ((DisplayAttribute)att).Prompt;
        }
    }
}
