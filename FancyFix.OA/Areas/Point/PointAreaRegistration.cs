using System.Web.Mvc;

namespace FancyFix.OA.Areas.Point
{
    public class PointAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Point";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Point_default",
                "Point/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FancyFix.OA.Areas.Point.Controllers" }
            );
        }
    }
}