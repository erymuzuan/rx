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

namespace Bespoke.Sph.Web.Areas.App.Views.Shared.DisplayTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/HtmlElement.cshtml")]
    public partial class _HtmlElement : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.HtmlElement>
    {
        public _HtmlElement()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if: ko.unwrap($type) === \"Bespoke.Sph.Domain.HtmlElement, domain.sph\" -->\r" +
"\n<div");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(">\r\n    <textarea");

WriteLiteral(" name=\"html-text\"");

WriteLiteral(" id=\"html-text\"");

WriteLiteral(" data-bind=\"value : Text\"");

WriteLiteral(" rows=\"10\"");

WriteLiteral(" cols=\"30\"");

WriteLiteral(" style=\"width:690px;min-height: 200px\"");

WriteLiteral("></textarea>\r\n    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : editHtml\"");

WriteLiteral(">\r\n        <i");

WriteLiteral(" class=\"fa fa-edit\"");

WriteLiteral("></i>\r\n        Edit HTML\r\n    </a>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
