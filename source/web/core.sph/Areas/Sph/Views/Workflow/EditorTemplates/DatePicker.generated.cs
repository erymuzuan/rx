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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Workflow/EditorTemplates/DatePicker.cshtml")]
    public partial class _Areas_Sph_Views_Workflow_EditorTemplates_DatePicker_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.DatePicker>
    {
        public _Areas_Sph_Views_Workflow_EditorTemplates_DatePicker_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 76), Tuple.Create("\"", 98)
            
            #line 4 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 82), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 82), false)
);

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">");

            
            #line 4 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\DatePicker.cshtml"
                                                            Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteLiteral(" class=\"controls col-lg-6\"");

WriteLiteral(">\r\n        <input");

WriteAttribute("class", Tuple.Create(" class=\"", 204), Tuple.Create("\"", 247)
            
            #line 6 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 212), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " "+ Model.Size
            
            #line default
            #line hidden
, 212), false)
);

WriteLiteral(" \r\n            data-bind=\"");

            
            #line 7 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\DatePicker.cshtml"
                   Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 324), Tuple.Create("\"", 345)
            
            #line 7 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 329), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 329), false)
);

WriteLiteral(" type=\"text\"");

WriteAttribute("name", Tuple.Create(" \r\n            name=\"", 358), Tuple.Create("\"", 390)
            
            #line 8 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 379), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 379), false)
);

WriteLiteral(" />\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
