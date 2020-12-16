using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Sikiro.Tookits.Extension;

namespace Sikiro.Common.Utils
{
    public static class HttpContextExtension
    {
        public static AdministratorData GetCurrentUser(this HttpContext context)
        {
            return context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserData)?.Value.FromJson<AdministratorData>();
        }
    }
}
