using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Sikiro.Common.Utils
{
    public static class ObjectExtension
    {
        public static void ThrowIfNull(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
        }

        public static List<KeyValuePair<string, int>> GetKeyValueList(this Type enumType)
        {
            if (enumType == null)
                return new List<KeyValuePair<string, int>>();

            var kvList = new List<KeyValuePair<string, int>>();
            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            var underlyingType = Enum.GetUnderlyingType(enumType);

            foreach (var field in fields)
            {
                var v = field.GetValue(null);
                var value = Convert.ChangeType(v, underlyingType);
                var display = field.GetCustomAttributes(typeof(DisplayAttribute), true)
                    .Cast<DisplayAttribute>()
                    .FirstOrDefault();
                kvList.Add(new KeyValuePair<string, int>(display != null ? display.Name : field.Name, (int)value));
            }
            return kvList;
        }

        public static string GetDisplayName(this Enum enumSubitem)
        {
            var strValue = enumSubitem.ToString();

            var fieldinfo = enumSubitem.GetType().GetField(strValue);
            var objs = fieldinfo.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();
            return objs?.Name;
        }
    }
}
