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
    
    #line 1 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsWithAuditTrail.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/EntityFormRenderer/Html2ColsWithAuditTrail.cshtml")]
    public partial class Html2ColsWithAuditTrail : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.FormRendererViewModel>
    {
        public Html2ColsWithAuditTrail()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsWithAuditTrail.cshtml"
  

    Layout = null;
    var caption = string.IsNullOrWhiteSpace(Model.Form.Caption) ? Model.Form.Name : Model.Form.Caption;


            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1>");

            
            #line 11 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsWithAuditTrail.cshtml"
Write(caption);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n");

            
            #line 12 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsWithAuditTrail.cshtml"
Write(Html.Partial("_ErrorList"));

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"col-sm-8\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 15 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsWithAuditTrail.cshtml"
   Write(Html.Partial("_FormContent"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 18 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsWithAuditTrail.cshtml"
   Write(Html.Partial("_AuditTrailContent"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
