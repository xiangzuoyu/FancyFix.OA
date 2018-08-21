using System.Web.Mvc;

namespace FancyFix.OA.Areas.FinanceStatistics
{
    public class FinanceStatisticsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FinanceStatistics";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FinanceStatistics_default",
                "FinanceStatistics/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}