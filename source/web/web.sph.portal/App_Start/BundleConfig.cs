﻿using System.Web;
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
                        "~/Scripts/bootstrap.min.js",
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

            /*
             * 
             * 
             *    <link href="~/Content/bootstrap.css" rel="stylesheet">
    <link href="~/Content/bootstrap-responsive.css" rel="stylesheet">
    <link href="~/Content/font-awesome.css" rel="stylesheet">
    <link href="~/Content/m-forms.min.css" rel="stylesheet">
    <link href="~/Content/m-buttons.css" rel="stylesheet">
    <link href="~/Content/custom.css" rel="stylesheet">
             * 
             * */

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/custom.css",
                        "~/Content/bootstrap-responsive.css",
                        "~/Content/bootstrap.css",
                        "~/Content/custom-slider.css",
                        "~/Content/font-awesome-ie7.css",
                        "~/Content/font-awesome.css",
                        "~/Content/m-buttons.css",
                        "~/Content/m-forms.css",
                        "~/Content/m-icons.css",
                        "~/Content/sprite.css",
                        "~/Content/metro-tile.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}