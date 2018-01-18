using System.Web.Mvc;

namespace FancyFix.OA.Areas.System
{
    public class SystemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "System";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "System_default",
                "System/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FancyFix.OA.Areas.System.Controllers" }
            );
        }
    }
}