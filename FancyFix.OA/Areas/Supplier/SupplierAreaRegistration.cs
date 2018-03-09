using System.Web.Mvc;

namespace FancyFix.OA.Areas.Supplier
{
    public class SupplierAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Supplier";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Supplier_default",
                "Supplier/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}