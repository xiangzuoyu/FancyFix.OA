using System.Web.Mvc;

namespace FancyFix.OA.Areas.ArtTask
{
    public class ArtTaskAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ArtTask";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ArtTask_default",
                "ArtTask/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}