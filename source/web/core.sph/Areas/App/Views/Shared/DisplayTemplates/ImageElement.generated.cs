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
    
    #line 1 "..\..\Areas\App\Views\Shared\DisplayTemplates\ImageElement.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/ImageElement.cshtml")]
    public partial class _Areas_App_Views_Shared_DisplayTemplates_ImageElement_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ImageElement>
    {
        public _Areas_App_Views_Shared_DisplayTemplates_ImageElement_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if: $data[\'$type\']() === \"Bespoke.Sph.Domain.ImageElement, domain.sph\" -->" +
"\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral("></label>\r\n    <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(" style=\"position: relative\"");

WriteLiteral(">\r\n        <img");

WriteLiteral(" alt=\"designer\"");

WriteLiteral(" src=\"/Images/no-image.png\"");

WriteLiteral(" />\r\n        <span");

WriteLiteral(" data-bind=\"text:\' [ \' + Path() + \' ] \'\"");

WriteLiteral(" style=\"position: absolute; top:53px;left:60px \"");

WriteLiteral("></span>\r\n    </div>\r\n    <span");

WriteLiteral(" data-bind=\"text: HelpText\"");

WriteLiteral(" class=\"help-block\"");

WriteLiteral("></span>\r\n</div>\r\n\r\n\r\n<!--/ko-->");

        }
    }
}
#pragma warning restore 1591
