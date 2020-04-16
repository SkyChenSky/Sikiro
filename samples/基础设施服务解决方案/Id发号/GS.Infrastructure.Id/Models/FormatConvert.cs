using System;
using System.Text;
using GS.Nosql.Redis;

namespace Sikiro.Infrastructure.Id.Models
{
    public class FormatConvert
    {
        private string _fmtStr;

        private string[] FmtArray => _fmtStr.Split("|");

        private readonly RedisRepository _redisRepository;

        public FormatConvert(RedisRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }


        public string Convert(string fmtStr)
        {
            _fmtStr = fmtStr;

            var sb = new StringBuilder(32);
            foreach (var item in FmtArray)
            {
                var isExist = FormatType.DicType.TryGetValue(item, out var type);
                if (isExist)
                {
                    switch (type)
                    {
                        case "DateTime":
                            sb.Append(DateTime.Now.ToString(item));
                            break;
                        case "int":
                            var num = _redisRepository.Increment(sb + item);
                            sb.Append(num.ToString(item));
                            break;
                    }
                }
                else
                {
                    sb.Append(item);
                }
            }

            return sb.ToString();
        }
    }
}
