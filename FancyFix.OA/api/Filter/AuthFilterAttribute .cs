using FancyFix.Core.Admin;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FancyFix.OA.api.Filter
{
    /// <summary>
    /// WebApi的Filter命名空间 System.Web.Http.Filters
    /// </summary>
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 检查用户是否有该Action执行的操作权限
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //如果请求Header不包含ticket，则判断是否是匿名调用
            var attr = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
            bool isAnonymous = attr.Any(a => a is AllowAnonymousAttribute);

            //是匿名用户，则继续执行；非匿名用户，抛出“未授权访问”信息
            if (!isAnonymous)
            {
                //获取当前用户对象
                var myInfo = new AdminState((HttpContextBase)actionContext.Request.Properties["MS_HttpContext"]).GetUserInfo();
                if (myInfo == null || !(bool)myInfo.InJob)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
                }
            }
            base.OnActionExecuting(actionContext);
        }
    }
}