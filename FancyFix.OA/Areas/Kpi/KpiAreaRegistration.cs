using System.Web.Mvc;

namespace FancyFix.OA.Areas.Kpi
{
    public class KpiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Kpi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Kpi_default",
                "Kpi/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FancyFix.OA.Areas.Kpi.Controllers" }
            );
        }
    }
}