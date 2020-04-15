using GS.Infrastructure.Id.Models;
using GS.Tookits.Base;
using GS.Tookits.Extension;
using GS.Tookits.Helper;
using Microsoft.AspNetCore.Mvc;

namespace GS.Infrastructure.Id.Controllers
{
    /// <summary>
    /// 分布式id
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
            if (format.IsNullOrWhiteSpace())
                return GuidHelper.GenerateComb().ToString();

            if (format.Equals("N"))
                return GuidHelper.GenerateComb().ToString(format);

            return _formatConvert.Convert(format);
        }
    }
}