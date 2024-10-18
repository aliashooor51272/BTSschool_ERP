using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ByteTechSchoolERP.DataAccess.Filters
{
    public class AuthenticationFilter : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string username = _httpContextAccessor.HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                context.Result = new RedirectToActionResult("Login", "AccessControl", new { area = "AccessControlArea" });
                return;
            }

            await next();
        }
    }
}
