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

namespace Bespoke.Sph.Web.Areas.App.Views.Shared.EditorTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/EditorTemplates/ImageElement.cshtml")]
    public partial class _ImageElement : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ImageElement>
    {
        public _ImageElement()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

WriteLiteral("<!--ko if: ko.unwrap($type) === \"Bespoke.Sph.Domain.ImageElement, domain.sph\" -->" +
"\r\n\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" >\r\n    <label");

WriteLiteral(" for=\"form-element-image-height\"");

WriteLiteral(">Width</label>\r\n    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Width\"");

WriteLiteral("\r\n           placeholder=\"Image width\"");

WriteLiteral("\r\n           id=\"form-element-image-width\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"width-image\"");

WriteLiteral(" />\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" >\r\n    <label");

WriteLiteral(" for=\"form-element-image-height\"");

WriteLiteral(">Height</label>\r\n    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Height\"");

WriteLiteral("\r\n           placeholder=\"Image height\"");

WriteLiteral("\r\n           id=\"form-element-image-height\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"width-height\"");

WriteLiteral(" />\r\n</div>\r\n\r\n\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
