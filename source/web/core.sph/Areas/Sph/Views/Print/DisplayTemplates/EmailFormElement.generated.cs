﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Areas.Sph.Views.Print.DisplayTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Print/DisplayTemplates/EmailFormElement.cshtml")]
    public partial class _EmailFormElement : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.EmailFormElement>
    {
        public _EmailFormElement()
        {
        }
        public override void Execute()
        {
WriteLiteral("<label");

WriteLiteral(" class=\"col-md-4\"");

WriteLiteral(">");

            
            #line 3 "..\..\Areas\Sph\Views\Print\DisplayTemplates\EmailFormElement.cshtml"
                   Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n<span");

WriteLiteral(" class=\"col-md-8\"");

WriteLiteral(" data-bind=\"text: ");

            
            #line 4 "..\..\Areas\Sph\Views\Print\DisplayTemplates\EmailFormElement.cshtml"
                                   Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral("></span>\r\n");

        }
    }
}
#pragma warning restore 1591
