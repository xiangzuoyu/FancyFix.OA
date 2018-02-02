using System.Web;
using System.Web.Mvc;

namespace FancyFix.ThirdPartyPlatform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
