using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AMS.Core;

namespace AMS.WebCore.Controllers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class UserAuthorizeAttribute:Attribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.ActionDescriptor.FilterDescriptors.Any(i => i.Filter is UserAnonymousAttribute))
            {
                return;
            }
            var workContext = filterContext.HttpContext.RequestServices.GetService(typeof(IWorkContext)) as IWorkContext;
            var user = workContext.CurrentAccount;
            if (user == null)//登录验证
            {
                var returnUrl = filterContext.HttpContext.Request.Path;
                if (filterContext.HttpContext.Request.IsAjaxRequest())//ajax
                {
                    filterContext.Result = new JsonResult(new { ResultState = false, ResultCode = 403, ResultMessage = "请先登录" });
                    return;
                }
                filterContext.Result = new RedirectResult("/user/login?returnUrl=" + returnUrl);
                return;
            }
        }
    }
}
