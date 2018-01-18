using FancyFix.OA.Bll;
using FancyFix.Core.Admin;
using FancyFix.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FancyFix.OA.Filter
{
    /// <summary>
    /// 登录验证过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CheckLoginAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 验证核心
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            try
            {
                var myInfo = new AdminState(httpContext).GetUserInfo();

                if (myInfo == null || !(bool)myInfo.InJob) return false;

                return true;
            }
            catch (Exception ex)
            {
                Tools.Tool.LogHelper.WriteLog(typeof(PermissionFilterAttribute), ex, 0, "");
                return false;
            }
        }

        /// <summary>
        /// 验证失败处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { Controller = "auth", action = "login", area = "" }));
        }
    }
}