using System.Web.Mvc;

namespace FancyFix.OA.Areas.Valuable
{
    public class ValuableAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Valuable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Valuable_default",
                "Valuable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FancyFix.OA.Areas.Valuable.Controllers" }
            );
        }
    }
}