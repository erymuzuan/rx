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
    
    #line 1 "..\..\Areas\App\Views\Shared\DisplayTemplates\TextBox.cshtml"
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/TextBox.cshtml")]
    public partial class _Areas_App_Views_Shared_DisplayTemplates_TextBox_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.TextBox>
    {
        public _Areas_App_Views_Shared_DisplayTemplates_TextBox_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if: ko.unwrap($data[\'$type\']) === \"Bespoke.Sph.Domain.TextBox, domain.sph\"" +
" -->\r\n<div");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 6 "..\..\Areas\App\Views\Shared\DisplayTemplates\TextBox.cshtml"
Write(Html.Partial("_DesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <label");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral("></label>\r\n    <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n        <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"attr: { \'title\': Tooltip, \'class\': CssClass() + \' form-control \' + Si" +
"ze(),\'placeholder\' : \' [ \' + Path() + \' ] \' }\"");

WriteLiteral(" />\r\n\r\n        <span");

WriteLiteral(" data-bind=\"text: HelpText\"");

WriteLiteral(" class=\"help-block\"");

WriteLiteral("></span>\r\n    </div>\r\n</div>\r\n<!--/ko-->");

        }
    }
}
#pragma warning restore 1591
