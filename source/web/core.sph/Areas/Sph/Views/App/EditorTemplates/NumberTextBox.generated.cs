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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/EditorTemplates/NumberTextBox.cshtml")]
    public partial class _Areas_Sph_Views_App_EditorTemplates_NumberTextBox_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.NumberTextBox>
    {
        public _Areas_Sph_Views_App_EditorTemplates_NumberTextBox_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 2 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
                                      Write(Html.Raw(Model.Visible));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 122), Tuple.Create("\"", 144)
            
            #line 3 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
, Tuple.Create(Tuple.Create("", 128), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 128), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 145), Tuple.Create("\"", 173)
            
            #line 3 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
, Tuple.Create(Tuple.Create("", 153), Tuple.Create<System.Object, System.Int32>(Model.LabelCssClass
            
            #line default
            #line hidden
, 153), false)
);

WriteLiteral(">");

            
            #line 3 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
                                                          Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 205), Tuple.Create("\"", 238)
            
            #line 4 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
, Tuple.Create(Tuple.Create("", 213), Tuple.Create<System.Object, System.Int32>(Model.InputPanelCssClass
            
            #line default
            #line hidden
, 213), false)
);

WriteLiteral(">\r\n        <input");

WriteAttribute("step", Tuple.Create(" step=\"", 256), Tuple.Create("\"", 274)
            
            #line 5 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
, Tuple.Create(Tuple.Create("", 263), Tuple.Create<System.Object, System.Int32>(Model.Step
            
            #line default
            #line hidden
, 263), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 275), Tuple.Create("\"", 344)
            
            #line 5 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
, Tuple.Create(Tuple.Create("", 283), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " form-control "+ Model.Size
            
            #line default
            #line hidden
, 283), false)
, Tuple.Create(Tuple.Create(" ", 331), Tuple.Create("form-control", 332), true)
);

WriteAttribute("title", Tuple.Create(" \r\n               title=\"", 345), Tuple.Create("\"", 384)
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
, Tuple.Create(Tuple.Create("", 370), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 370), false)
);

WriteLiteral(" data-bind=\"");

            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
                                             Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create("\r\n               id=\"", 447), Tuple.Create("\"", 484)
            
            #line 7 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
, Tuple.Create(Tuple.Create("", 468), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 468), false)
);

WriteLiteral(" type=\"number\"");

WriteAttribute("name", Tuple.Create(" name=\"", 499), Tuple.Create("\"", 517)
            
            #line 7 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
, Tuple.Create(Tuple.Create("", 506), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 506), false)
);

WriteLiteral(" />\r\n    </div>\r\n");

            
            #line 9 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
    
            
            #line default
            #line hidden
            
            #line 9 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
     if (!string.IsNullOrWhiteSpace(Model.HelpText))
    {

            
            #line default
            #line hidden
WriteLiteral("        <span");

WriteLiteral(" class=\"help-block\"");

WriteLiteral(">");

            
            #line 11 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
                            Write(Model.HelpText);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

            
            #line 12 "..\..\Areas\Sph\Views\App\EditorTemplates\NumberTextBox.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>  ");

        }
    }
}
#pragma warning restore 1591
