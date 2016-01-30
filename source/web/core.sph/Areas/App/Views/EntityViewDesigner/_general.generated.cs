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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityViewDesigner/_general.cshtml")]
    public partial class _Areas_App_Views_EntityViewDesigner__general_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_App_Views_EntityViewDesigner__general_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"view-name\"");

WriteLiteral(" class=\"col-lg-3 col-md-3 control-label\"");

WriteLiteral(">Name</label>\r\n        <div");

WriteLiteral(" class=\"col-lg-9 col-md-9 col-sm-12\"");

WriteLiteral(">\r\n            <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: Name, valueUpdate: \'keyup\'\"");

WriteLiteral("\r\n                   required");

WriteLiteral(" pattern=\"^[A-Za-z_][A-Za-z0-9_ ]*$\"");

WriteLiteral("\r\n                   placeholder=\"Name for the view\"");

WriteLiteral("\r\n                   class=\"form-control\"");

WriteLiteral(" id=\"view-name\"");

WriteLiteral(">\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"view-route\"");

WriteLiteral(" class=\"col-lg-3 col-md-3 col-sm-2 control-label\"");

WriteLiteral(">Route</label>\r\n        <div");

WriteLiteral(" class=\"col-lg-9 col-md-9 col-sm-12\"");

WriteLiteral(">\r\n            <input");

WriteLiteral(" type=\"text\"");

WriteLiteral("\r\n                   required");

WriteLiteral(" pattern=\"^[a-z][a-z0-9-.]*$\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Route, tooltip:\'Route is a way the system identify your view v" +
"ia its URL, must be lower case with - or .\'\"");

WriteLiteral("\r\n                   placeholder=\"route url\"");

WriteLiteral(" id=\"view-route\"");

WriteLiteral(">\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"view-route\"");

WriteLiteral(" class=\"col-lg-3 col-md-3 col-sm-2 control-label\"");

WriteLiteral(">Endpoint</label>\r\n        <div");

WriteLiteral(" class=\"col-lg-9 col-md-9 col-sm-12\"");

WriteLiteral(">\r\n            <select required");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" \r\n                    data-bind=\"value: Endpoint, options:$root.endpointOptions," +
" optionsCaption: \'[Select your query]\', tooltip:\'Query to point the search to.\'\"" +
"");

WriteLiteral(" id=\"view-query\"");

WriteLiteral(">\r\n                \r\n            </select>\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"view-note\"");

WriteLiteral(" class=\"col-lg-3 col-md-3 col-sm-2 control-label\"");

WriteLiteral(">Note</label>\r\n        <div");

WriteLiteral(" class=\"col-lg-9 col-md-9 col-sm-12\"");

WriteLiteral(">\r\n            <textarea");

WriteLiteral(" data-bind=\"value: Note, valueUpdate: \'keyup\'\"");

WriteLiteral("\r\n                      placeholder=\"Note about the view\"");

WriteLiteral("\r\n                      class=\"form-control\"");

WriteLiteral(" id=\"view-note\"");

WriteLiteral("></textarea>\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"icon-storeid\"");

WriteLiteral(" class=\"col-lg-3 col-md-3 col-sm-2 control-label\"");

WriteLiteral(">Image</label>\r\n        <div");

WriteLiteral(" class=\"col-lg-9 col-md-9 col-sm-12\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" data-bind=\"attr : {\'class\':TileColour}\"");

WriteLiteral(" style=\"padding: 10px;\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"pull-left\"");

WriteLiteral(">\r\n                    <img");

WriteLiteral(" data-bind=\"attr:{src: \'/sph/image/store/\' + IconStoreId() }\"");

WriteLiteral(" alt=\"Icon\"");

WriteLiteral(">\r\n                </div>\r\n                <div>\r\n                    <span");

WriteLiteral(" style=\"font-size: 32px; font-weight: bold; margin: 5px\"");

WriteLiteral(">0</span>\r\n                </div>\r\n                <div>\r\n                    <h5" +
"");

WriteLiteral(" data-bind=\"text: Name\"");

WriteLiteral("></h5>\r\n                    <label");

WriteLiteral(" data-bind=\"text: Note\"");

WriteLiteral("></label>\r\n                </div>\r\n            </div>\r\n            <input");

WriteLiteral(" type=\"file\"");

WriteLiteral(" data-bind=\"kendoUpload: {value: IconStoreId, extensions : [\'.png\', \'.gif\']}\"");

WriteLiteral(" name=\"files\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"icon-storeid\"");

WriteLiteral(">\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"icon-class\"");

WriteLiteral(" class=\"col-lg-3 col-md-3 control-label\"");

WriteLiteral(">Icon class</label>\r\n        <div");

WriteLiteral(" class=\"col-md-9 col-lg-9\"");

WriteLiteral(">\r\n            <i");

WriteLiteral(" data-bind=\"iconPicker: IconClass, attr:{\'class\':IconClass() + \' fa-2x\' }\"");

WriteLiteral(" id=\"icon-class\"");

WriteLiteral("></i>\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"ev-tl-color\"");

WriteLiteral(" class=\"col-lg-3 col-md-3 col-sm-2 control-label\"");

WriteLiteral(">Color</label>\r\n        <div");

WriteLiteral(" class=\"col-lg-9 col-md-9 col-sm-12\"");

WriteLiteral(">\r\n            <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"ev-tl-color\"");

WriteLiteral(" data-bind=\"value:TileColour\"");

WriteLiteral(">\r\n                <option");

WriteLiteral(" value=\"bblue\"");

WriteLiteral(">Blue</option>\r\n                <option");

WriteLiteral(" value=\"blightblue\"");

WriteLiteral(">Light blue</option>\r\n                <option");

WriteLiteral(" value=\"bred\"");

WriteLiteral(">Red</option>\r\n                <option");

WriteLiteral(" value=\"bgreen\"");

WriteLiteral(">Green</option>\r\n                <option");

WriteLiteral(" value=\"borange\"");

WriteLiteral(">Orange</option>\r\n                <option");

WriteLiteral(" value=\"bviolet\"");

WriteLiteral(">Violet</option>\r\n            </select>\r\n        </div>\r\n    </div>\r\n\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"ev-template\"");

WriteLiteral(" class=\"col-lg-3 col-md-3  control-label\"");

WriteLiteral(">Template</label>\r\n        <div");

WriteLiteral(" class=\"col-lg-9 col-md-9\"");

WriteLiteral(">\r\n            <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"ev-template\"");

WriteLiteral(" data-bind=\"value: Template, options:$root.templateOptions, optionsCaption :\'[Sel" +
"ect a template]\'\"");

WriteLiteral("></select>\r\n        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"col-md-offset-2 col-md-10\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                <label>\r\n                    <input");

WriteLiteral(" data-bind=\"checked: DisplayOnDashboard\"");

WriteLiteral(" id=\"ev-display-dashboard\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"DisplayOnDashboard\"");

WriteLiteral(" />\r\n                    Display thumbnail on dashboard\r\n                </label>" +
"\r\n            </div>\r\n        </div>\r\n    </div>\r\n\r\n</form>\r\n");

        }
    }
}
#pragma warning restore 1591
