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

namespace Bespoke.Sph.Web.Areas.App.Views.EntityFormRenderer.EditorTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityFormRenderer/EditorTemplates/ImageElement.cshtml")]
    public partial class _ImageElement : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ImageElement>
    {
        public _ImageElement()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ImageElement.cshtml"
 if (string.IsNullOrWhiteSpace(Model.Enable))
{
    Model.Enable = "true";
}
            
            #line default
            #line hidden
            
            #line 5 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ImageElement.cshtml"
   
    var path = string.Format("'/sph/image/store/' + {0}()", this.Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\r\n<img");

WriteLiteral(" data-bind=\"attr : {\'src\':");

            
            #line 8 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ImageElement.cshtml"
                         Write(path);

            
            #line default
            #line hidden
WriteLiteral("}, visible:");

            
            #line 8 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ImageElement.cshtml"
                                         Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" />");

        }
    }
}
#pragma warning restore 1591
