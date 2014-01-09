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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/AddressElement.cshtml")]
    public partial class AddressElement : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.AddressElement>
    {
        public AddressElement()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if:$data[\'$type\']() === \"Bespoke.Sph.Domain.AddressElement, domain.sph\" --" +
">\r\n\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 6 "..\..\Areas\App\Views\Shared\DisplayTemplates\AddressElement.cshtml"
Write(Html.Partial("_DesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div>\r\n        <h3");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral("></h3>\r\n    </div>\r\n    <div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible: IsUnitNoVisible\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">No Unit</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible: IsBlockVisible\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">Blok</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n                <!-- ko if : BlockOptionsPath -->\r\n                <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral("></select>\r\n                <!-- /ko -->\r\n                <!-- ko ifnot : BlockOp" +
"tionsPath -->\r\n                <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                <!-- /ko -->\r\n            </div>\r\n        </div>\r\n        <d" +
"iv");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible: IsFloorVisible\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">Tingkat</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral("/>\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" >\r\n            <label");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">Jalan</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral("/>\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" >\r\n            <label");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">Bandar/Daerah</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral("/>\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" >\r\n            <label");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">Poskod</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral("/>\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" >\r\n            <label");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">Negeri</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n                <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(">\r\n                    <option");

WriteLiteral(" value=\"Kelantan\"");

WriteLiteral(">Kelantan</option>\r\n                </select>\r\n            </div>\r\n        </div>" +
"\r\n    </div>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
