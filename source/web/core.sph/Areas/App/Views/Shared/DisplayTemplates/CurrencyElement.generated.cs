﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/CurrencyElement.cshtml")]
    public partial class _CurrencyElement : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.CurrencyElement>
    {
        public _CurrencyElement()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if: $data[\'$type\']() === \"Bespoke.Sph.Domain.CurrencyElement, domain.sph\" " +
"-->\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(">\r\n");

WriteLiteral("     ");

            
            #line 5 "..\..\Areas\App\Views\Shared\DisplayTemplates\CurrencyElement.cshtml"
Write(Html.Partial("_DesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n   <label");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral("></label>\r\n    <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n        <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"attr: { \'title\': Tooltip, \'class\': CssClass() + \' \' + Size() ,placeho" +
"lder:\' [ \' + Path() + \' ] \'}\"");

WriteLiteral(" />\r\n    </div>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
