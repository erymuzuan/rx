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

namespace Bespoke.Sph.Web.Areas.App.Views.EntityFormDesigner
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityFormDesigner/_ValidationSetting.cshtml")]
    public partial class ValidationSetting : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ValidationSetting()
        {
        }
        public override void Execute()
        {
WriteLiteral("<form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(" data-bind=\"with : FieldValidation\"");

WriteLiteral(" role=\"form\"");

WriteLiteral(">\r\n\r\n    <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n        <label>\r\n            <input");

WriteLiteral(" data-bind=\"checked: IsRequired\"");

WriteLiteral(" id=\"validation-isrequired\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsRequired\"");

WriteLiteral(" />\r\n            Is required\r\n        </label>\r\n    </div>\r\n\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"validation-pattern\"");

WriteLiteral(">Pattern</label>\r\n        <input");

WriteLiteral(" data-bind=\"value:Pattern\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"validation-pattern\"");

WriteLiteral(" placeholder=\"regex pattern\"");

WriteLiteral(">\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"validation-minlength\"");

WriteLiteral(">Min Length</label>\r\n        <input");

WriteLiteral(" data-bind=\"value:MinLength\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"validation-minlength\"");

WriteLiteral(">\r\n    </div>\r\n\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"validation-maxlength\"");

WriteLiteral(">Max Length</label>\r\n        <input");

WriteLiteral(" data-bind=\"value:MaxLength\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"validation-maxlength\"");

WriteLiteral(">\r\n    </div>\r\n\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"validation-mode\"");

WriteLiteral(">Mode</label>\r\n        <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"validation-mode\"");

WriteLiteral(" data-bind=\"value:Mode\"");

WriteLiteral(">\r\n            <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">Text</option>\r\n            <option");

WriteLiteral(" value=\"email\"");

WriteLiteral(">Email</option>\r\n            <option");

WriteLiteral(" value=\"url\"");

WriteLiteral(">Url</option>\r\n            <option");

WriteLiteral(" value=\"digit\"");

WriteLiteral(">Digit</option>\r\n            <option");

WriteLiteral(" value=\"number\"");

WriteLiteral(">Number</option>\r\n            <option");

WriteLiteral(" value=\"date\"");

WriteLiteral(">Date</option>\r\n        </select>\r\n    </div>\r\n\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"validation-message\"");

WriteLiteral(">Message</label>\r\n        <input");

WriteLiteral(" data-bind=\"value:Message\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"validation-message\"");

WriteLiteral(" placeholder=\"Error message\"");

WriteLiteral(">\r\n    </div>\r\n</form>\r\n");

        }
    }
}
#pragma warning restore 1591
