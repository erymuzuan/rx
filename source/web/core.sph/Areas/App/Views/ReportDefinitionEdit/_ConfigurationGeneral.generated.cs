﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ReportDefinitionEdit/_ConfigurationGeneral.cshtml")]
    public partial class _Areas_App_Views_ReportDefinitionEdit__ConfigurationGeneral_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_App_Views_ReportDefinitionEdit__ConfigurationGeneral_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"rdl-title\"");

WriteLiteral(" class=\"control-label col-sm-3\"");

WriteLiteral(">Title</label>\r\n        <div");

WriteLiteral(" class=\"col-sm-8\"");

WriteLiteral(">\r\n            <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Title\"");

WriteLiteral(" id=\"rdl-title\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Title\"");

WriteLiteral(" />\r\n        </div>\r\n    </div>\r\n    \r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"cat\"");

WriteLiteral(" class=\"control-label col-sm-3\"");

WriteLiteral(">Category</label>\r\n        <div");

WriteLiteral(" class=\"col-sm-8\"");

WriteLiteral(">\r\n            <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Category\"");

WriteLiteral(" id=\"cat\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Category\"");

WriteLiteral(" />\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"rdl-description\"");

WriteLiteral(" class=\"control-label col-sm-3\"");

WriteLiteral(">Description</label>\r\n        <div");

WriteLiteral(" class=\"col-sm-8\"");

WriteLiteral(">\r\n            <textarea");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Description\"");

WriteLiteral(" id=\"rdl-description\"");

WriteLiteral(" name=\"Description\"");

WriteLiteral("></textarea>\r\n        </div>\r\n    </div>\r\n    \r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-9 col-sm-offset-3\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" data-bind=\"checked: IsActive\"");

WriteLiteral(" id=\"rdl-isactive\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsActive\"");

WriteLiteral(" />\r\n                Is Active\r\n            </label>\r\n        </div>\r\n    </div>\r" +
"\n    \r\n    \r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-sm-9 col-sm-offset-3\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" data-bind=\"checked: IsPrivate\"");

WriteLiteral(" id=\"rdl-isprivate\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsPrivate\"");

WriteLiteral(" />\r\n                Private\r\n            </label>\r\n        </div>\r\n    </div>\r\n " +
"   \r\n\r\n</form>\r\n");

        }
    }
}
#pragma warning restore 1591
