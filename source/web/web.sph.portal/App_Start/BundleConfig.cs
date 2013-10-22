using System.Web.Optimization;

namespace web.sph.portal
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/custom.js",
                        "~/Scripts/jquery-1.8.2.intellisense.js",
                        "~/Scripts/jquery-1.8.2.js",
                        "~/Scripts/jquery-1.8.2.min.js",
                        "~/Scripts/jquery-ui-1.8.24.js",
                        "~/Scripts/jquery-ui-1.8.24.min.js",
                        "~/Scripts/jquery.ba-cond.min.js",
                        "~/Scripts/jquery.gmap.js",
                        "~/Scripts/jquery.isotope.min.js",
                        "~/Scripts/jquery.js",
                        "~/Scripts/jquery.slitslider.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate-vsdoc.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/knockout-2.2.0.debug.js",
                        "~/Scripts/knockout-2.2.0.js",
                        "~/Scripts/metro-custom.js",
                        "~/Scripts/modernizr-2.6.2.js",
                        "~/Scripts/modernizr.custom.79639.js",
                        "~/Scripts/modernizr-*"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap/bootstrap.css",
                        "~/Content/font-awesome-ie7.css",
                        "~/Content/font-awesome.css",
                        "~/Content/m-forms.css",
                        "~/Content/m-buttons.css",
                        "~/Content/sprite.css",
                        "~/Content/custom.css"
                        ));

        }
    }
}