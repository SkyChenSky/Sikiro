using System;
using Microsoft.AspNetCore.Mvc;
using Sikiro.Infrastructure.Id.Models;
using Sikiro.Tookits.Extension;
using Sikiro.Tookits.Helper;

namespace Sikiro.Infrastructure.Id.Controllers
{
    /// <summary>
    /// id发号器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class IdController : ControllerBase
    {
        private readonly FormatConvert _formatConvert;

        public IdController(FormatConvert formatConvert)
        {
            _formatConvert = formatConvert;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        public string Generate(string format = null)
        {
            throw new Exception("asdas");
            if (format.IsNullOrWhiteSpace())
                return GuidHelper.GenerateComb().ToString();

            if (format.Equals("N"))
                return GuidHelper.GenerateComb().ToString(format);

            return _formatConvert.Convert(format);
        }
    }
}