using System.Web;
using System.Web.Optimization;

namespace FancyFix.OA
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //css
            bundles.Add(new StyleBundle("~/Content/layout/css").Include(
                  "~/Content/css/bootstrap/bootstrap.min.css",
                  "~/Content/css/adminlte/AdminLTE.min.css",
                  "~/Content/css/adminlte/skins/_all-skins.min.css"));

            //js
            bundles.Add(new ScriptBundle("~/Content/layout/js").Include(
                     "~/Content/js/plugins/jQuery/jquery-2.2.3.min.js",
                     "~/Content/js/bootstrap/bootstrap.min.js",
                     "~/Content/js/adminlte/app.min.js"));
        }
    }
}
