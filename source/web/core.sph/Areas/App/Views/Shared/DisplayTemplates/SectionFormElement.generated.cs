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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/SectionFormElement.cshtml")]
    public partial class SectionFormElement_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.SectionFormElement>
    {
        public SectionFormElement_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if:$data[\'$type\']() === \"Bespoke.Sph.Domain.SectionFormElement, domain.sph" +
"\" -->\r\n<div");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(">\r\n    <h2");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral("></h2>\r\n    <div");

WriteLiteral(" data-bind=\"attr : {\'class\':CssClass},html: HelpText\"");

WriteLiteral(" ></div>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
