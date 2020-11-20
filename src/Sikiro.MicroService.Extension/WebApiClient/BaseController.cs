using Microsoft.AspNetCore.Mvc;

namespace Sikiro.MicroService.Extension.Rpc
{
    /// <summary>
    /// 基础类
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
