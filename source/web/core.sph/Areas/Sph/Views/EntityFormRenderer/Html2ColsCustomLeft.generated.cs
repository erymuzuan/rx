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
    
    #line 1 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/EntityFormRenderer/Html2ColsCustomLeft.cshtml")]
    public partial class _Areas_Sph_Views_EntityFormRenderer_Html2ColsCustomLeft_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.FormRendererViewModel>
    {
        public _Areas_Sph_Views_EntityFormRenderer_Html2ColsCustomLeft_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
  

    Layout = null;
    var caption = string.IsNullOrWhiteSpace(Model.Form.Caption) ? Model.Form.Name : Model.Form.Caption;
    var header = Model.Form.FormLayoutCollection.SingleOrDefault(y => y.Position == "Header");

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" data-i18n=\"");

            
            #line 11 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
          Write(caption);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">");

            
            #line 11 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
                    Write(caption);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n");

            
            #line 12 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
Write(Html.Partial("_ErrorList"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 13 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
 if (null != header)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 16 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
   Write(Html.Raw(header.Content));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n");

            
            #line 18 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"col-sm-4\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 21 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
   Write(Html.Raw(Model.Form.FormLayoutCollection.Single(y => y.Position == "LeftSide").Content));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"col-sm-8\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 24 "..\..\Areas\Sph\Views\EntityFormRenderer\Html2ColsCustomLeft.cshtml"
   Write(Html.Partial("_FormContent"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
