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

namespace Bespoke.Sph.Web.Areas.Sph.Views.App.DisplayTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/DisplayTemplates/EmailFormElement.cshtml")]
    public partial class EmailFormElement_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.EmailFormElement>
    {
        public EmailFormElement_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 83), Tuple.Create("\"", 105)
            
            #line 3 "..\..\Areas\Sph\Views\App\DisplayTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 89), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 89), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 3 "..\..\Areas\Sph\Views\App\DisplayTemplates\EmailFormElement.cshtml"
                                                   Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n        <input");

WriteAttribute("class", Tuple.Create(" class=\"", 193), Tuple.Create("\"", 236)
            
            #line 5 "..\..\Areas\Sph\Views\App\DisplayTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 201), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " "+ Model.Size
            
            #line default
            #line hidden
, 201), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 237), Tuple.Create("\"", 259)
            
            #line 5 "..\..\Areas\Sph\Views\App\DisplayTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 245), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 245), false)
);

WriteLiteral(" \r\n            data-bind=\"");

            
            #line 6 "..\..\Areas\Sph\Views\App\DisplayTemplates\EmailFormElement.cshtml"
                   Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 336), Tuple.Create("\"", 357)
            
            #line 6 "..\..\Areas\Sph\Views\App\DisplayTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 341), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 341), false)
);

WriteLiteral(" type=\"email\"");

WriteAttribute("name", Tuple.Create(" name=\"", 371), Tuple.Create("\"", 389)
            
            #line 6 "..\..\Areas\Sph\Views\App\DisplayTemplates\EmailFormElement.cshtml"
                                    , Tuple.Create(Tuple.Create("", 378), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 378), false)
);

WriteLiteral(" />\r\n    </div>\r\n</div>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
