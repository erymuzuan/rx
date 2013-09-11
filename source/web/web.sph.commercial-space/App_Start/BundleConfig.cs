using System;
using System.Configuration;
using System.Web.Optimization;

namespace Bespoke.Sph.Commerspace.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);
            var theme = ConfigurationManager.AppSettings["theme"];

            bundles.Add(
              new ScriptBundle("~/scripts/vendor")
                .Include("~/scripts/jquery-{version}.js")
                .Include("~/scripts/knockout-{version}.debug.js")
                .Include("~/scripts/knockout.mapping-latest.js")
                .Include("~/scripts/modernizr-{version}.js")
                .Include("~/scripts/sammy-{version}.js")
                .Include("~/scripts/toastr.js")
                .Include("~/scripts/Q.js")
                .Include("~/scripts/bootstrap.js")
                .Include("~/scripts/moment.js")
                .Include("~/scripts/string.js")
                .Include("~/scripts/underscore.js")
                .Include("~/kendo/js/kendo.all.js")
                .Include("~/scripts/nprogress.js")
                .Include("~/scripts/_pager.js")
                .Include("~/scripts/_kendo-knockoutbindings.js")
                .Include("~/scripts/_uiready.js")
                .Include("~/scripts/_function.prototypes.js")
                .Include("~/scripts/_task.js")
                .Include("~/scripts/_constants.js")
                .Include("~/App/schemas/*.js")
                .Include("~/App/partial/*.js")
              );

            bundles.Add(
                new ScriptBundle("~/scripts/public")
                    .Include("~/scripts/jquery-{version}.js")
                    .Include("~/scripts/jcarousellite_1.0.1.js")
                );
            bundles.Add(
              new StyleBundle("~/Content/css")
                .Include("~/kendo/styles/kendo.common.css")
                .Include("~/kendo/styles/kendo.metro.css")
                .Include("~/kendo/styles/kendo.dataviz.css")
                .Include("~/kendo/styles/kendo.dataviz.metrol.css")
                .Include("~/Content/ie10mobile.css")
                .Include("~/Content/bootstrap." + theme + ".css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/durandal.css")
                .Include("~/Content/nprogress.css")
                .Include("~/Content/style.css")
                .Include("~/Content/widget.css")
                .Include("~/Content/icomoon.css")
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/toastr.css")
                .Include("~/Content/sprite.css")
                .Include("~/Content/css/commercialspace.css")
                .Include("~/Content/theme." + theme + "/site.css")
                .Include("~/Content/theme." + theme + "/header.css")
                .Include("~/Content/theme." + theme + "/nav.css")
                .Include("~/Content/theme." + theme + "/dashboard.css")
                .Include("~/Content/theme." + theme + "/building.css")
                .Include("~/Content/theme." + theme + "/complaint.css")
                .Include("~/Content/theme." + theme + "/complaint.css")
                .Include("~/Content/theme." + theme + "/rentalapplication.css")
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