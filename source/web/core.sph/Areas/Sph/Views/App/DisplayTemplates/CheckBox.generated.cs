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

namespace Bespoke.Sph.Web.Areas.Sph.Views.App.DisplayTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/DisplayTemplates/CheckBox.cshtml")]
    public partial class CheckBox : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.CheckBox>
    {
        public CheckBox()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Sph\Views\App\DisplayTemplates\CheckBox.cshtml"
  
    

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 6 "..\..\Areas\Sph\Views\App\DisplayTemplates\CheckBox.cshtml"
                            Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n        <span");

WriteLiteral(" data-bind=\"text: ");

            
            #line 8 "..\..\Areas\Sph\Views\App\DisplayTemplates\CheckBox.cshtml"
                          Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" id=\"ID\"");

WriteLiteral("  />\r\n    </div>\r\n</div>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
