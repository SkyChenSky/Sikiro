using System.Linq;
using System.Security.Claims;
using GS.Tookits.Extension;
using Microsoft.AspNetCore.Http;

namespace GS.Common.Utils
{
    public static class HttpContextExtension
    {
        public static AdministratorData GetCurrentUser(this HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value.FromJson<AdministratorData>();
        }
    }
}
