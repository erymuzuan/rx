﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Areas.App.Views.ReportDefinitionEdit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ReportDefinitionEdit/_PieChartDesignerContextAction.cshtml")]
    public partial class PieChartDesignerContextAction : System.Web.Mvc.WebViewPage<dynamic>
    {
        public PieChartDesignerContextAction()
        {
        }
        public override void Execute()
        {
WriteLiteral("<section");

WriteLiteral(" class=\"btn-group pull-right\"");

WriteLiteral(" data-bind=\"visible: isSelected\"");

WriteLiteral(">\r\n    <button");

WriteLiteral(" class=\"btn btn-link btn-context-action\"");

WriteLiteral(">\r\n        <i");

WriteLiteral(" class=\"fa fa-chevron-circle-right\"");

WriteLiteral("></i>\r\n    </button>\r\n    <div");

WriteLiteral(" class=\"context-action alert alert-info\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"modal-header\"");

WriteLiteral(">\r\n            <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">&times;</button>\r\n            <span>Pie Chart Properties</span>\r\n        </div>\r" +
"\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Title</label>\r\n            <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Title\"");

WriteLiteral(" name=\"Title-linechart\"");

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Category Field</label>\r\n            <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" data-bind=\"value: CategoryField\"");

WriteLiteral("  name=\"ValueLabelFormat\"");

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Value Field</label>\r\n            <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" data-bind=\"value: ValueField\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"HorizontalAxisField\"");

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n        <a");

WriteLiteral(" class=\"btn btn-link pull-right\"");

WriteLiteral(" data-bind=\"click: $root.removeReportItem\"");

WriteLiteral(">\r\n            <i");

WriteLiteral(" class=\"fa fa-times\"");

WriteLiteral("></i>\r\n            Remove\r\n        </a>\r\n        ");

WriteLiteral("\r\n    </div>\r\n</section>\r\n");

        }
    }
}
#pragma warning restore 1591