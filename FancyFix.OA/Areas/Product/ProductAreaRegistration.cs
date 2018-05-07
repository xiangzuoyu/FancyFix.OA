using System.Web.Mvc;

namespace FancyFix.OA.Areas.Product
{
    public class ProductAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Product";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Product_default",
                "Product/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "FancyFix.OA.Areas.Product.Controllers" }
            );
        }
    }
}