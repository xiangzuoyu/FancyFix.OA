using System.Web.Mvc;

namespace FancyFix.OA.Areas.DesignerTask
{
    public class DesignerTaskAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DesignerTask";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DesignerTask_default",
                "DesignerTask/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}