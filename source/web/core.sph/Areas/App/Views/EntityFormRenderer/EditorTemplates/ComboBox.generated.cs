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

namespace Bespoke.Sph.Web.Areas.App.Views.EntityFormRenderer.EditorTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityFormRenderer/EditorTemplates/ComboBox.cshtml")]
    public partial class _ComboBox : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ComboBox>
    {
        public _ComboBox()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
  
    var required = Model.IsRequired ? "required" : null;

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 5 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
 if (Model.IsCompact)
{

            
            #line default
            #line hidden
WriteLiteral("    <select");

WriteLiteral(" data-bind=\"");

            
            #line 7 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                   Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 200), Tuple.Create("\"", 221)
            
            #line 7 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 205), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 205), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 222), Tuple.Create("\"", 242)
            
            #line 7 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                        , Tuple.Create(Tuple.Create("", 229), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 229), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 243), Tuple.Create("\"", 299)
            
            #line 7 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                                              , Tuple.Create(Tuple.Create("", 251), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " "+ Model.Size
            
            #line default
            #line hidden
, 251), false)
, Tuple.Create(Tuple.Create(" ", 286), Tuple.Create("form-control", 287), true)
);

WriteLiteral(">\r\n");

            
            #line 8 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
        
            
            #line default
            #line hidden
            
            #line 8 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
         foreach (var op in Model.ComboBoxItemCollection)
        {

            
            #line default
            #line hidden
WriteLiteral("            <option");

WriteAttribute("value", Tuple.Create(" value=\"", 392), Tuple.Create("\"", 409)
            
            #line 10 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 400), Tuple.Create<System.Object, System.Int32>(op.Value
            
            #line default
            #line hidden
, 400), false)
);

WriteLiteral(">");

            
            #line 10 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                                 Write(op.Caption);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 11 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </select>\r\n");

            
            #line 13 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
}
else
{


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 17 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                                          Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n        <label");

WriteAttribute("for", Tuple.Create(" for=\"", 552), Tuple.Create("\"", 574)
            
            #line 18 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 558), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 558), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 575), Tuple.Create("\"", 603)
            
            #line 18 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 583), Tuple.Create<System.Object, System.Int32>(Model.LabelCssClass
            
            #line default
            #line hidden
, 583), false)
);

WriteLiteral(">");

            
            #line 18 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                                                              Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 639), Tuple.Create("\"", 672)
            
            #line 19 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 647), Tuple.Create<System.Object, System.Int32>(Model.InputPanelCssClass
            
            #line default
            #line hidden
, 647), false)
);

WriteLiteral(">\r\n            <select");

WriteLiteral(" data-bind=\"");

            
            #line 20 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                           Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 757), Tuple.Create("\"", 778)
            
            #line 20 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
       , Tuple.Create(Tuple.Create("", 762), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 762), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 779), Tuple.Create("\"", 799)
            
            #line 20 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                                , Tuple.Create(Tuple.Create("", 786), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 786), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 800), Tuple.Create("\"", 856)
            
            #line 20 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                                                      , Tuple.Create(Tuple.Create("", 808), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " "+ Model.Size
            
            #line default
            #line hidden
, 808), false)
, Tuple.Create(Tuple.Create(" ", 843), Tuple.Create("form-control", 844), true)
);

WriteLiteral(">\r\n");

            
            #line 21 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                
            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                 foreach (var op in Model.ComboBoxItemCollection)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <option");

WriteAttribute("value", Tuple.Create(" value=\"", 973), Tuple.Create("\"", 990)
            
            #line 23 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 981), Tuple.Create<System.Object, System.Int32>(op.Value
            
            #line default
            #line hidden
, 981), false)
);

WriteLiteral(">");

            
            #line 23 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                                         Write(op.Caption);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 24 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </select>\r\n        </div>\r\n    </div>\r\n");

            
            #line 28 "..\..\Areas\App\Views\EntityFormRenderer\EditorTemplates\ComboBox.cshtml"
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
