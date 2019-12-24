using System.Web;
using System.Web.Optimization;

namespace MaxxExpress
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/customWork").Include(
                        "~/Areas/WorkPage/js/workMain.js"));

            bundles.Add(new ScriptBundle("~/bundles/customPortal").Include(
                        "~/Areas/PortalPage/js/portalMain.js"));

            bundles.Add(new ScriptBundle("~/bundles/customSelect").Include(
                       "~/Areas/PortalPage/js/classie.js", "~/Areas/PortalPage/js/selectFx.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Areas/WorkPage/css/bootstrap.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/site.css"));


           
        }
    }
}