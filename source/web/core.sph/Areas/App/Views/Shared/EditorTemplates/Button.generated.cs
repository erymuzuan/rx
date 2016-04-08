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
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/EditorTemplates/Button.cshtml")]
    public partial class _Areas_App_Views_Shared_EditorTemplates_Button_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.Button>
    {
        public _Areas_App_Views_Shared_EditorTemplates_Button_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

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

WriteLiteral(" data-bind=\"options :$root.operationsOption,\r\n                            options" +
"Caption :\'[Use command]\',\r\n                            value: Operation\"");

WriteLiteral(" id=\"form-design-operation\"");

WriteLiteral(" name=\"FormDesign.Button.Operation\"");

WriteLiteral("></select>\r\n</div>\r\n\r\n\r\n<!-- /ko -->\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"form-design-operation-method\"");

WriteLiteral(">Method</label>\r\n    <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: OperationMethod,enable:Operation\"");

WriteLiteral(" id=\"form-design-operation-method\"");

WriteLiteral(" name=\"FormDesign.OperationMethod\"");

WriteLiteral(">\r\n        <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">[Please select]</option>\r\n        <option");

WriteLiteral(" value=\"post\"");

WriteLiteral(">POST</option>\r\n        <option");

WriteLiteral(" value=\"put\"");

WriteLiteral(">PUT</option>\r\n        <option");

WriteLiteral(" value=\"patch\"");

WriteLiteral(">PATCH</option>\r\n    </select>\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"form-design-delete-operation\"");

WriteLiteral(">DELETE Operation</label>\r\n    <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"options :$root.deleteOperationsOption,\r\n                            o" +
"ptionsCaption :\'[Select a DELETE operation]\',\r\n                            value" +
": DeleteOperation\"");

WriteLiteral(" id=\"form-design-delete-operation\"");

WriteLiteral(" name=\"FormDesign.Operation\"");

WriteLiteral("></select>\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"success-message\"");

WriteLiteral(">Success Message</label>\r\n    <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: OperationSuccessMesage, enable:Operation\"");

WriteLiteral("\r\n           placeholder=\"The message to alert user when the operation return suc" +
"cess\"");

WriteLiteral("\r\n           class=\"form-control\"");

WriteLiteral(" id=\"success-message\"");

WriteLiteral(">\r\n\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"success-url\"");

WriteLiteral(">Then navigate to</label>\r\n    <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: OperationSuccessNavigateUrl, enable:Operation\"");

WriteLiteral("\r\n           placeholder=\"Once the alert is okayed then navigate to this url\"");

WriteLiteral("\r\n           class=\"form-control\"");

WriteLiteral(" id=\"success-url\"");

WriteLiteral(">\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"operation-success-callback\"");

WriteLiteral(">Success Callback</label>\r\n    <textarea");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: OperationSuccessCallback, enable:Operation\"");

WriteLiteral("\r\n              placeholder=\"Execute custom code once the operation successfuly i" +
"nvoked\"");

WriteLiteral("\r\n              id=\"operation-success-callback\"");

WriteLiteral(" name=\"OperationSuccessCallback\"");

WriteLiteral("></textarea>\r\n    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : editOperationSuccessCallback, disable:(ko.unwrap(OperationSuc" +
"cessMesage) || ko.unwrap(OperationSuccessNavigateUrl))\"");

WriteLiteral(">Edit</a>\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"operation-failure-callback\"");

WriteLiteral(">Failuire Callback</label>\r\n    <textarea");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: OperationFailureCallback, enable:Operation\"");

WriteLiteral("\r\n              placeholder=\"Execute custom code once the operation failed to be " +
"successfuly invoked\"");

WriteLiteral("\r\n              id=\"operation-failure-callback\"");

WriteLiteral(" name=\"OperationFailureCallback\"");

WriteLiteral("></textarea>\r\n    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : editOperationFailureCallback\"");

WriteLiteral(">Edit</a>\r\n</div>\r\n\r\n\r\n\r\n\r\n<div");

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

WriteLiteral(">Edit</a>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
