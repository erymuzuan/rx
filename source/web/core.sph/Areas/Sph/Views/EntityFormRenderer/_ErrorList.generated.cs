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

namespace Bespoke.Sph.Web.Areas.Sph.Views.EntityFormRenderer
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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/EntityFormRenderer/_ErrorList.cshtml")]
    public partial class ErrorList : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ErrorList()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" id=\"error-list\"");

WriteLiteral(" class=\"row\"");

WriteLiteral(" data-bind=\"visible:errors().length\"");

WriteLiteral(">\r\n    <!-- ko foreach : errors -->\r\n    <div");

WriteLiteral(" class=\"col-sm-8 col-sm-offset-2 alert alert-dismissable alert-danger\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"alert\"");

WriteLiteral(" aria-hidden=\"true\"");

WriteLiteral(">&times;</button>\r\n        <i");

WriteLiteral(" class=\"fa fa-exclamation\"");

WriteLiteral("></i>\r\n        <span");

WriteLiteral(" data-bind=\"text:Message\"");

WriteLiteral("></span>\r\n\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"col-sm-2\"");

WriteLiteral("></div>\r\n    <!-- /ko-->\r\n</div>");

        }
    }
}
#pragma warning restore 1591
