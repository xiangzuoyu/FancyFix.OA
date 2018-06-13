using System.Web.Http;
using System.Web.Mvc;

namespace FancyFix.OA.Areas.Demand
{
    public class DemandAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Demand";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Demand_default",
                "Demand/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


            //      context.MapRoute(
            //    name: "Demand_default",
            //    url: "Demand/{controller}/{action}/{id}",
            //    defaults: new
            //    {
            //        action = "Index",
            //        id = UrlParameter.Optional,
            //    }
            //);

            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                this.AreaName + "_Api",
                "api/" + this.AreaName + "/{controller}/{action}/{id}",
                new
                {
                    id = RouteParameter.Optional
                }
            );
        }
    }
}