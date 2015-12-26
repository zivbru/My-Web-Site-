using System.Web;
using System.Web.Optimization;

namespace Final
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.sequence-min.js",
                        "~/Scripts/jquery-1.9.1.min.js",
                        "~/Scripts/jquery.fancybox.js",
                        "~/Scripts/jquery.fancybox-media.js",
                        "~/Scripts/jquery.mousewheel-3.0.6.pack.js",
                        "~/Scripts/jquery.fancybox-buttons.js",
                        "~/Scripts/jquery.fancybox-thumbs.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/main.css",
                      "~/Content/icomoon-social.css",
                      "~/Content/leaflet.css",
                      "~/content/jquery.fancybox.css",
                      "~/content/jquery.fancybox-buttons.css",
                      "~/content/jquery.fancybox-thumbs.css",
                      "~/content/about-us.css",
                      "~/content/framework.css"
                      ));
            
        }
    }
}
