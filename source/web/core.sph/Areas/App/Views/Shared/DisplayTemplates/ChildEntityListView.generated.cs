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
    
    #line 2 "..\..\Areas\App\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/ChildEntityListView.cshtml")]
    public partial class ChildEntityListView_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ChildEntityListView>
    {
        public ChildEntityListView_()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

WriteLiteral("\r\n<!--ko if:ko.unwrap($type) === \"Bespoke.Sph.Domain.ChildEntityListView, domain." +
"sph\" -->\r\n<div");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected },click: $root.selectFormE" +
"lement\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 8 "..\..\Areas\App\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
Write(Html.Partial("_DesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <h3");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral("></h3>\r\n    <table");

WriteLiteral(" class=\"table table-condensed table-striped\"");

WriteLiteral(">\r\n        <thead>\r\n            <tr");

WriteLiteral(" data-bind=\"foreach: ViewColumnCollection\"");

WriteLiteral(">\r\n                <th>\r\n                    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" >\r\n                        <span");

WriteLiteral(" data-bind=\"text:Header\"");

WriteLiteral("></span>\r\n                        <i");

WriteLiteral(" class=\"fa fa-check\"");

WriteLiteral("></i>\r\n\r\n                    </a>\r\n                </th>\r\n            </tr>\r\n    " +
"    </thead>\r\n        <tbody>\r\n");

            
            #line 24 "..\..\Areas\App\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
            
            
            #line default
            #line hidden
            
            #line 24 "..\..\Areas\App\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
             for (int i = 0; i < 5; i++)
            {

            
            #line default
            #line hidden
WriteLiteral("                <tr");

WriteLiteral(" data-bind=\"foreach: ViewColumnCollection\"");

WriteLiteral(">\r\n                    <td>\r\n                        Sample\r\n                    " +
"</td>\r\n                </tr>\r\n");

            
            #line 31 "..\..\Areas\App\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </tbody>\r\n    </table>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
