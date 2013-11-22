using System;
using System.Configuration;
using System.Web.Optimization;

namespace Bespoke.Sph.Web.App_Start
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
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/jquery.dataTables.min.js")
                .Include("~/scripts/jquery.tablesorter.min.js")
                .Include("~/scripts/knockout-{version}.debug.js")
                .Include("~/scripts/knockout.mapping-latest.js")
                .Include("~/scripts/modernizr-{version}.js")
                .Include("~/scripts/sammy-{version}.js")
                .Include("~/scripts/toastr.js")
                .Include("~/scripts/Q.js")
                .Include("~/scripts/bootstrap.js")
                .Include("~/scripts/moment.js")
                .Include("~/scripts/underscore.js")
                .Include("~/kendo/js/kendo.all.js")
                .Include("~/scripts/nprogress.js")
                .Include("~/scripts/typeahead.min.js")
              );
            bundles.Add(
              new ScriptBundle("~/scripts/core")
                .Include("~/App/objectbuilders.js")
                .Include("~/scripts/string.js")
                .Include("~/scripts/_pager.js")
                .Include("~/scripts/_theme.js")
                .Include("~/scripts/_ko.bootstrap.js")
                .Include("~/scripts/_ko.kendo.js")
                .Include("~/scripts/_ko.workflow.js")
                .Include("~/scripts/_uiready.js")
                .Include("~/scripts/_function.prototypes.js")
                .Include("~/scripts/_task.js")
                .Include("~/scripts/_utils.js")
                .Include("~/scripts/_constants.js")
              );
            bundles.Add(
              new ScriptBundle("~/scripts/domain.schema")
                .Include("~/App/schemas/*.js")
              );
            bundles.Add(
              new ScriptBundle("~/scripts/domain.prototypes")
                .Include("~/App/prototypes/*.js")
              );
            bundles.Add(
              new ScriptBundle("~/scripts/domain.partials")
                .Include("~/App/partial/*.js")
              );
            bundles.Add(
              new ScriptBundle("~/scripts/themes")
                .Include("~/Content/theme." +  theme +"/*.js")
              );
            
            bundles.Add(
              new StyleBundle("~/Content/css")
                .Include("~/kendo/styles/kendo.common.css")
                .Include("~/kendo/styles/kendo.metro.css")
                .Include("~/kendo/styles/kendo.dataviz.css")
                .Include("~/kendo/styles/kendo.dataviz.metrol.css")
                .Include("~/Content/ie10mobile.css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/durandal.css")
                .Include("~/Content/nprogress.css")
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/toastr.css")
                .Include("~/Content/blueimp-gallery.css")
                .Include("~/Content/typeahead.js-bootstrap.css")
                .Include("~/Content/theme." + theme + "/style.css")
                .Include("~/Content/theme." + theme + "/space.css")
                .Include("~/Content/theme." + theme + "/site.css")
                .Include("~/Content/theme." + theme + "/header.css")
                .Include("~/Content/theme." + theme + "/nav.css")
                .Include("~/Content/theme." + theme + "/dashboard.css")
                .Include("~/Content/theme." + theme + "/building.css")
                .Include("~/Content/theme." + theme + "/complaint.css")
                .Include("~/Content/theme." + theme + "/complaint.css")
                .Include("~/Content/theme." + theme + "/rentalapplication.css")
                .Include("~/Content/theme." + theme + "/user.css")
                .Include("~/Content/theme." + theme + "/report.css")
                .Include("~/Content/theme." + theme + "/public.index.css")
                .Include("~/Content/theme." + theme + "/workflow.triggers.css")
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