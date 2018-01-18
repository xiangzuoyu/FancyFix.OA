using System.Web.Mvc;

namespace FancyFix.OA.Areas.Rank
{
    public class RankAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Rank";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Rank_default",
                "Rank/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "FancyFix.OA.Areas.Rank.Controllers" }
            );
        }
    }
}