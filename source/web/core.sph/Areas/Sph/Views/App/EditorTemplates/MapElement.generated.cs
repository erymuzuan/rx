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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/EditorTemplates/MapElement.cshtml")]
    public partial class _Areas_Sph_Views_App_EditorTemplates_MapElement_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.MapElement>
    {
        public _Areas_Sph_Views_App_EditorTemplates_MapElement_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 3 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
                                         Write(Html.Raw(Model.Visible));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 124), Tuple.Create("\"", 146)
            
            #line 4 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 130), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 130), false)
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

WriteAttribute("title", Tuple.Create(" title=\"", 235), Tuple.Create("\"", 257)
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 243), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 243), false)
);

WriteAttribute("id", Tuple.Create(" id=\"", 258), Tuple.Create("\"", 279)
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 263), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 263), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 280), Tuple.Create("\"", 303)
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 288), Tuple.Create<System.Object, System.Int32>(Model.CssClass
            
            #line default
            #line hidden
, 288), false)
);

WriteLiteral(" data-bind=\"click: $root.showMapCommand\"");

WriteLiteral(">\r\n            <!-- ko if : staticMap -->\r\n            <img");

WriteLiteral(" data-bind=\"attr : {src:staticMap}\"");

WriteLiteral(" src=\"/Images/no-image.png\"");

WriteLiteral(" alt=\"map\"");

WriteLiteral(" />\r\n            <!-- /ko  -->\r\n            <!-- ko ifnot : staticMap -->\r\n      " +
"      <i");

WriteAttribute("class", Tuple.Create(" class=\"", 564), Tuple.Create("\"", 583)
            
            #line 11 "..\..\Areas\Sph\Views\App\EditorTemplates\MapElement.cshtml"
, Tuple.Create(Tuple.Create("", 572), Tuple.Create<System.Object, System.Int32>(Model.Icon
            
            #line default
            #line hidden
, 572), false)
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
