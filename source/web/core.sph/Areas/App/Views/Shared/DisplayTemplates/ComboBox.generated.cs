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
    
    #line 1 "..\..\Areas\App\Views\Shared\DisplayTemplates\ComboBox.cshtml"
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/ComboBox.cshtml")]
    public partial class _Areas_App_Views_Shared_DisplayTemplates_ComboBox_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ComboBox>
    {
        public _Areas_App_Views_Shared_DisplayTemplates_ComboBox_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if:ko.unwrap($type) === \"Bespoke.Sph.Domain.ComboBox, domain.sph\" -->\r\n<di" +
"v");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 6 "..\..\Areas\App\Views\Shared\DisplayTemplates\ComboBox.cshtml"
Write(Html.Partial("_DesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <label");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">LABEL</label>\r\n    <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n        <select");

WriteLiteral(" data-bind=\"attr: { \'class\': CssClass() + \' form-control\' + \' \' + Size(), \'title\'" +
": Tooltip,\'placeholder\' : \' [ \' + Path() + \' ] \'  }\"");

WriteLiteral(">\r\n            <option");

WriteLiteral(" data-bind=\"text:\' [ \' + Path() + \' ] \'\"");

WriteLiteral("></option>\r\n        </select>\r\n        <span");

WriteLiteral(" data-bind=\"text: HelpText\"");

WriteLiteral(" class=\"help-block\"");

WriteLiteral("></span>\r\n    </div>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
