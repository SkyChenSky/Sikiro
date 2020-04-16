using System.Collections.Generic;

namespace Sikiro.Infrastructure.Id.Models
{
    public static class FormatType
    {
        public static Dictionary<string, string> DicType = new Dictionary<string, string>
        {
            { "yyyyMMdd","DateTime" },
            { "yyyyMMddHHmmss","DateTime"  },
            { "D","int" }
        };

        static FormatType()
        {
            for (var i = 1; i <= 30; i++)
            {
                DicType.Add("D" + i, "int");
            }
        }
    }
}
