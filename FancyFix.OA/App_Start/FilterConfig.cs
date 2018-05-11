using FancyFix.OA.Filter;
using System.Web;
using System.Web.Mvc;

namespace FancyFix.OA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //全局权限过滤，严格模式，慎用
            //filters.Add(new PermissionFilter());
        }
    }
}
