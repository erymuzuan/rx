﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ReportDefinitionEdit/_ConfigurationColumnOptions.cshtml")]
    public partial class ConfigurationColumnOptions : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ConfigurationColumnOptions()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" data-bind=\"with: $root\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"alert alert-info alert-dismissable\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"alert\"");

WriteLiteral(" aria-hidden=\"true\"");

WriteLiteral(">&times;</button>\r\n        <i");

WriteLiteral(" class=\"fa fa-question-circle fa-2x\"");

WriteLiteral(" style=\"margin-right: 10px\"");

WriteLiteral("></i>\r\n        Check the field to add to the report columns, to remove use the Co" +
"lumns Properties Tab\r\n    </div>\r\n    <ul");

WriteLiteral(" id=\"entity-columns\"");

WriteLiteral(" data-bind=\"foreach: entityColumns,filter: { path: \'li>label\' }\"");

WriteLiteral(">\r\n        <li>\r\n            <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked: IsSelected\"");

WriteLiteral(@" />
                <!-- ko text:Name-->
                <!--/ko-->
                <!-- ko if :IsCustomField -->*
                <!-- /ko -->
            </label>
        </li>
    </ul>
    <p>
        *Custom Fields may not available in all record
    </p>
</div>");

        }
    }
}
#pragma warning restore 1591
