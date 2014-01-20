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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityFormDesigner/_DesignerContextAction.cshtml")]
    public partial class DesignerContextAction : System.Web.Mvc.WebViewPage<dynamic>
    {
        public DesignerContextAction()
        {
        }
        public override void Execute()
        {
WriteLiteral("<section");

WriteLiteral(" class=\"context-action-panel\"");

WriteLiteral(" data-bind=\"visible: isSelected\"");

WriteLiteral(">\r\n    <button");

WriteLiteral(" class=\"btn btn-link  btn-context-action\"");

WriteLiteral(">\r\n        <i");

WriteLiteral(" class=\"fa fa-chevron-circle-right\"");

WriteLiteral("></i>\r\n    </button>\r\n\r\n    <div");

WriteLiteral(" class=\"context-action panel panel-default\"");

WriteLiteral(" data-bind=\"with: $root.selectedFormElement\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"panel-heading\"");

WriteLiteral(">\r\n            <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(">&times;</button>\r\n            <span>Properties</span>\r\n        </div>\r\n        <" +
"div");

WriteLiteral(" class=\"panel-body\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <label");

WriteLiteral(" for=\"context-action-label\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Label</label>\r\n                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Label, valueUpdate: \'keyup\'\"");

WriteLiteral(" id=\"context-action-label\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                </div>\r\n            </div>\r\n\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <label");

WriteLiteral(" for=\"context-action-label\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Tooltip</label>\r\n                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Tooltip, valueUpdate: \'keyup\'\"");

WriteLiteral(" id=\"context-action-tooltip\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                </div>\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <label");

WriteLiteral(" for=\"context-action-label\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Help</label>\r\n                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                    <textarea");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" rows=\"3\"");

WriteLiteral(" cols=\"32\"");

WriteLiteral(" data-bind=\"value: HelpText, valueUpdate: \'keyup\'\"");

WriteLiteral(" id=\"context-action-help\"");

WriteLiteral("></textarea>\r\n                </div>\r\n            </div>\r\n            <a");

WriteLiteral(" class=\"btn btn-warning pull-right\"");

WriteLiteral(" data-bind=\"click: $root.removeFormElement\"");

WriteLiteral(">\r\n                <i");

WriteLiteral(" class=\"glyphicon glyphicon-remove\"");

WriteLiteral("></i>\r\n                Buang\r\n            </a>\r\n        </div>\r\n        ");

WriteLiteral("\r\n\r\n    </div>\r\n</section>\r\n");

        }
    }
}
#pragma warning restore 1591
