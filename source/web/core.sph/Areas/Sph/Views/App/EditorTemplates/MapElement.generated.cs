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

namespace Bespoke.Sph.Web.Areas.Sph.Views.App.EditorTemplates
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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/EditorTemplates/MapElement.cshtml")]
    public partial class MapElement_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.MapElement>
    {
        public MapElement_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 3 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
                                         Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 114), Tuple.Create("\"", 136)
            
            #line 4 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 120), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 120), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 4 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
                                                   Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n        <button");

WriteAttribute("title", Tuple.Create(" title=\"", 225), Tuple.Create("\"", 247)
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 233), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 233), false)
);

WriteAttribute("id", Tuple.Create(" id=\"", 248), Tuple.Create("\"", 269)
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 253), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 253), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 270), Tuple.Create("\"", 293)
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 278), Tuple.Create<System.Object, System.Int32>(Model.CssClass
            
            #line default
            #line hidden
, 278), false)
);

WriteLiteral(" data-bind=\"click: $root.showMapCommand\"");

WriteLiteral(">\r\n            <!-- ko if : staticMap -->\r\n            <img");

WriteLiteral(" data-bind=\"attr : {src:staticMap}\"");

WriteLiteral(" src=\"/Images/no-image.png\"");

WriteLiteral(" alt=\"map\"");

WriteLiteral(" />\r\n            <!-- /ko  -->\r\n            <!-- ko ifnot : staticMap -->\r\n      " +
"      <i");

WriteAttribute("class", Tuple.Create(" class=\"", 554), Tuple.Create("\"", 573)
            
            #line 11 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 562), Tuple.Create<System.Object, System.Int32>(Model.Icon
            
            #line default
            #line hidden
, 562), false)
);

WriteLiteral("></i>\r\n");

WriteLiteral("            ");

            
            #line 12 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
       Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\r\n            <!-- /ko  -->\r\n            </button>\r\n");

            
            #line 15 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
        
            
            #line default
            #line hidden
            
            #line 15 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
         if (!string.IsNullOrWhiteSpace(Model.HelpText))
        {

            
            #line default
            #line hidden
WriteLiteral("            <span");

WriteLiteral(" class=\"help-inline\"");

WriteLiteral(">");

            
            #line 17 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
                                 Write(Model.HelpText);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

            
            #line 18 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591