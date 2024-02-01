using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Forum_System.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class IsAdminAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAdminStatus = context.HttpContext.Session.GetString("AdminStatus");

            if (isAdminStatus != "True")
            {
                context.Result = new RedirectToRouteResult(new { controller = "Home", action = "Index" });
            }
        }
    }
}
