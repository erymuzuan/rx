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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/DisplayTemplates/WebsiteFormElement.cshtml")]
    public partial class _Areas_Sph_Views_App_DisplayTemplates_WebsiteFormElement_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.WebsiteFormElement>
    {
        public _Areas_Sph_Views_App_DisplayTemplates_WebsiteFormElement_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 85), Tuple.Create("\"", 107)
            
            #line 3 "..\..\Areas\Sph\Views\App\DisplayTemplates\WebsiteFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 91), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 91), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 3 "..\..\Areas\Sph\Views\App\DisplayTemplates\WebsiteFormElement.cshtml"
                                                   Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n        <input");

WriteAttribute("class", Tuple.Create(" class=\"", 195), Tuple.Create("\"", 238)
            
            #line 5 "..\..\Areas\Sph\Views\App\DisplayTemplates\WebsiteFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 203), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " "+ Model.Size
            
            #line default
            #line hidden
, 203), false)
);

WriteAttribute("title", Tuple.Create(" \r\n               title=\"", 239), Tuple.Create("\"", 278)
            
            #line 6 "..\..\Areas\Sph\Views\App\DisplayTemplates\WebsiteFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 264), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 264), false)
);

WriteLiteral(" \r\n            data-bind=\"");

            
            #line 7 "..\..\Areas\Sph\Views\App\DisplayTemplates\WebsiteFormElement.cshtml"
                   Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 355), Tuple.Create("\"", 376)
            
            #line 7 "..\..\Areas\Sph\Views\App\DisplayTemplates\WebsiteFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 360), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 360), false)
);

WriteLiteral(" type=\"url\"");

WriteAttribute("name", Tuple.Create(" name=\"", 388), Tuple.Create("\"", 406)
            
            #line 7 "..\..\Areas\Sph\Views\App\DisplayTemplates\WebsiteFormElement.cshtml"
                                  , Tuple.Create(Tuple.Create("", 395), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 395), false)
);

WriteLiteral(" />\r\n    </div>\r\n</div>\r\n\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
