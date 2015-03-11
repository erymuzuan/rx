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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/EditorTemplates/Button.cshtml")]
    public partial class Button_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.Button>
    {
        public Button_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if: ko.unwrap($type) === \"Bespoke.Sph.Domain.Button, domain.sph\" -->\r\n<div" +
"");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"button-icon-class\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Icon class</label>\r\n    <br />\r\n    <i");

WriteLiteral(" data-bind=\"iconPicker: IconClass, attr:{\'class\':IconClass() + \' fa-2x\' }\"");

WriteLiteral(" id=\"button-icon-class\"");

WriteLiteral("></i>\r\n   \r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n\r\n    <label>\r\n        <input");

WriteLiteral(" data-bind=\"checked: IsToolbarItem\"");

WriteLiteral(" id=\"button-toolbar-item\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsToolbarItem\"");

WriteLiteral(" />\r\n        Show on toolbar\r\n    </label>\r\n\r\n</div>\r\n\r\n<!-- ko if : typeof $root" +
".entity === \"function\" -->\r\n\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"form-element-button-command-name\"");

WriteLiteral(">Operation</label>\r\n    <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"options :$root.entity().EntityOperationCollection, optionsText: \'Name" +
"\', optionsValue : \'Name\', optionsCaption :\'[Use command]\',value: Operation\"");

WriteLiteral(" id=\"form-design-operation\"");

WriteLiteral(" name=\"FormDesign.Operation\"");

WriteLiteral("></select>\r\n</div>\r\n\r\n<!-- /ko -->\r\n\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"disable:Operation\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"form-element-button-command-name\"");

WriteLiteral(">Command Name</label>\r\n    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: CommandName,disable:Operation\"");

WriteLiteral("\r\n           placeholder=\"save is default command generated, edit the command scr" +
"ipt using the page editor\"");

WriteLiteral("\r\n           id=\"form-element-button-command-name\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"CommandName\"");

WriteLiteral(" />\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label>\r\n        <input");

WriteLiteral(" data-bind=\"checked: UseClick,disable:Operation\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"UseClick\"");

WriteLiteral(" />\r\n        Use click\r\n    </label>\r\n</div>\r\n\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"form-element-button-command\"");

WriteLiteral(">Command</label>\r\n    <textarea");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Command,disable:Operation\"");

WriteLiteral("\r\n              placeholder=\"Command needs to return a promise\"");

WriteLiteral("\r\n              id=\"form-element-button-command\"");

WriteLiteral(" name=\"Command\"");

WriteLiteral("></textarea>\r\n    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : editCommand,disable:Operation\"");

WriteLiteral(">Edit</a>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591