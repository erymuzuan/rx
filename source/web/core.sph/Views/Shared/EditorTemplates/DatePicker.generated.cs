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

namespace Bespoke.Sph.Web.Views.Shared.EditorTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/DatePicker.cshtml")]
    public partial class DatePicker : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.DatePicker>
    {
        public DatePicker()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
 if (Model.IsCompact)
{


            
            #line default
            #line hidden
WriteLiteral("    <input");

WriteAttribute("class", Tuple.Create(" class=\"", 78), Tuple.Create("\"", 135)
            
            #line 6 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 86), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " form-control " + Model.Size
            
            #line default
            #line hidden
, 86), false)
);

WriteLiteral("\r\n           data-bind=\"");

            
            #line 7 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
                  Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 210), Tuple.Create("\"", 231)
            
            #line 7 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 215), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 215), false)
);

WriteLiteral(" type=\"text\"");

WriteAttribute("name", Tuple.Create("\r\n           name=\"", 244), Tuple.Create("\"", 274)
            
            #line 8 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 263), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 263), false)
);

WriteLiteral(" />\r\n");

            
            #line 9 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
}
else
{


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group datepicker\"");

WriteLiteral(">\r\n        <label");

WriteAttribute("for", Tuple.Create(" for=\"", 349), Tuple.Create("\"", 371)
            
            #line 14 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 355), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 355), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 372), Tuple.Create("\"", 400)
            
            #line 14 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 380), Tuple.Create<System.Object, System.Int32>(Model.LabelCssClass
            
            #line default
            #line hidden
, 380), false)
);

WriteLiteral(">");

            
            #line 14 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
                                                              Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 436), Tuple.Create("\"", 469)
            
            #line 15 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 444), Tuple.Create<System.Object, System.Int32>(Model.InputPanelCssClass
            
            #line default
            #line hidden
, 444), false)
);

WriteLiteral(">\r\n            <input");

WriteAttribute("class", Tuple.Create(" class=\"", 491), Tuple.Create("\"", 548)
            
            #line 16 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 499), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " form-control " + Model.Size
            
            #line default
            #line hidden
, 499), false)
);

WriteLiteral("\r\n                   data-bind=\"");

            
            #line 17 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
                          Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 631), Tuple.Create("\"", 652)
            
            #line 17 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
      , Tuple.Create(Tuple.Create("", 636), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 636), false)
);

WriteLiteral(" type=\"text\"");

WriteAttribute("name", Tuple.Create("\r\n                   name=\"", 665), Tuple.Create("\"", 703)
            
            #line 18 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
, Tuple.Create(Tuple.Create("", 692), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 692), false)
);

WriteLiteral(" />\r\n        </div>\r\n    </div>\r\n");

            
            #line 21 "..\..\Views\Shared\EditorTemplates\DatePicker.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
