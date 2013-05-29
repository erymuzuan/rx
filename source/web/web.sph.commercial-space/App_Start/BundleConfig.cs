using System;
using System.Web.Optimization;

namespace Bespoke.Sph.Commerspace.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            bundles.Add(
              new ScriptBundle("~/scripts/vendor")
                .Include("~/scripts/jquery-{version}.js")
                .Include("~/scripts/knockout-{version}.debug.js")
                .Include("~/scripts/knockout.mapping-latest.js")
                .Include("~/scripts/modernizr-{version}.js")
                .Include("~/scripts/modernizr-2.6.1.min.js")
                .Include("~/scripts/modernizr-2.6.2.js")
                .Include("~/scripts/mustache.js")
                .Include("~/scripts/sammy-{version}.js")
                .Include("~/scripts/toastr.js")
                .Include("~/scripts/Q.js")
                .Include("~/scripts/breeze.debug.js")
                .Include("~/scripts/bootstrap.js")
                .Include("~/scripts/bootmetro.js")
                .Include("~/scripts/bootmetro-charms.js")
                .Include("~/scripts/bootstrap-datepicker.js")
                .Include("~/scripts/holder.js")
                .Include("~/scripts/jquery-1.7.2.js")
                .Include("~/scripts/jquery-1.7.2.min.js")
                .Include("~/scripts/jquery-1.8.2.js")
                .Include("~/scripts/jquery-1.8.2.min.js")
                .Include("~/scripts/jquery.blockUI.js")
                .Include("~/scripts/jquery.form.js")
                .Include("~/scripts/jquery.mousewheel.js")
                .Include("~/scripts/jquery.scrollTo.js")
                .Include("~/scripts/moment.js")
                .Include("~/scripts/string.js")
                .Include("~/scripts/underscore.js")
                .Include("~/kendo/js/kendo.all.js")
                .Include("~/scripts/_kendo-knockoutbindings.js")
              );

            bundles.Add(
              new StyleBundle("~/Content/css")
                .Include("~/kendo/styles/kendo.common.css")
                .Include("~/kendo/styles/kendo.metro.css")
                .Include("~/kendo/styles/kendo.dataviz.css")
                .Include("~/kendo/styles/kendo.dataviz.metrol.css")
                .Include("~/Content/ie10mobile.css")
                .Include("~/Content/bootstrap.min.css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/bootstrap-datepicker.css")
                .Include("~/Content/bootmetro-charms.css")
                .Include("~/Content/bootmetro-tiles.css")
                .Include("~/Content/bootmetro.css")
                .Include("~/Content/datepicker.css")
                .Include("~/Content/demo-old.css")
                .Include("~/Content/demo.css")
                .Include("~/Content/durandal.css")
                .Include("~/Content/icomoon.css")
                .Include("~/Content/metro-ui-dark.css")
                .Include("~/Content/metro-ui-light.css")
                .Include("~/Content/toastr.css")
                .Include("~/Content/app.css")
                .Include("~/Content/sprite.css")
                .Include("~/Content/site.css")
              );
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
            {
                throw new ArgumentNullException("ignoreList");
            }

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");

            //ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            //ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            //ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }
    }
}