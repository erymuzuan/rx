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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/DisplayTemplates/DatePicker.cshtml")]
    public partial class _Areas_Sph_Views_App_DisplayTemplates_DatePicker_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.DatePicker>
    {
        public _Areas_Sph_Views_App_DisplayTemplates_DatePicker_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 79), Tuple.Create("\"", 101)
            
            #line 4 "..\..\Areas\Sph\Views\App\DisplayTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 85), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 85), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 4 "..\..\Areas\Sph\Views\App\DisplayTemplates\DatePicker.cshtml"
                                                   Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n        <input");

WriteAttribute("class", Tuple.Create(" class=\"", 189), Tuple.Create("\"", 232)
            
            #line 6 "..\..\Areas\Sph\Views\App\DisplayTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 197), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " "+ Model.Size
            
            #line default
            #line hidden
, 197), false)
);

WriteLiteral(" \r\n            data-bind=\"");

            
            #line 7 "..\..\Areas\Sph\Views\App\DisplayTemplates\DatePicker.cshtml"
                   Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 309), Tuple.Create("\"", 330)
            
            #line 7 "..\..\Areas\Sph\Views\App\DisplayTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 314), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 314), false)
);

WriteLiteral(" type=\"text\"");

WriteAttribute("name", Tuple.Create(" \r\n            name=\"", 343), Tuple.Create("\"", 375)
            
            #line 8 "..\..\Areas\Sph\Views\App\DisplayTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 364), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 364), false)
);

WriteLiteral(" />\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
