using System.Linq;
using System.Security.Claims;
using GS.Common.Utils;
using GS.Tookits.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GS.WebApi.Customer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        protected AdministratorData CurrentUserData => GetCurrentUser();

        private AdministratorData GetCurrentUser()
        {
            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value.FromJson<AdministratorData>();
        }
    }
}
