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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/EditorTemplates/ComboBox.cshtml")]
    public partial class ComboBox_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ComboBox>
    {
        public ComboBox_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 2 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
                                      Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 107), Tuple.Create("\"", 129)
            
            #line 3 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 113), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 113), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 130), Tuple.Create("\"", 158)
            
            #line 3 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 138), Tuple.Create<System.Object, System.Int32>(Model.LabelCssClass
            
            #line default
            #line hidden
, 138), false)
);

WriteLiteral(">");

            
            #line 3 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
                                                          Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 190), Tuple.Create("\"", 223)
            
            #line 4 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 198), Tuple.Create<System.Object, System.Int32>(Model.InputPanelCssClass
            
            #line default
            #line hidden
, 198), false)
);

WriteLiteral(">\r\n        <select");

WriteLiteral(" data-bind=\"");

            
            #line 5 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
                       Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 304), Tuple.Create("\"", 325)
            
            #line 5 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
   , Tuple.Create(Tuple.Create("", 309), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 309), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 326), Tuple.Create("\"", 346)
            
            #line 5 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
                            , Tuple.Create(Tuple.Create("", 333), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 333), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 347), Tuple.Create("\"", 403)
            
            #line 5 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
                                                  , Tuple.Create(Tuple.Create("", 355), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " "+ Model.Size
            
            #line default
            #line hidden
, 355), false)
, Tuple.Create(Tuple.Create(" ", 390), Tuple.Create("form-control", 391), true)
);

WriteLiteral(">\r\n");

            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
            
            
            #line default
            #line hidden
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
             foreach (var op in Model.ComboBoxItemCollection)
            {

            
            #line default
            #line hidden
WriteLiteral("                <option");

WriteAttribute("value", Tuple.Create(" value=\"", 508), Tuple.Create("\"", 525)
            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
, Tuple.Create(Tuple.Create("", 516), Tuple.Create<System.Object, System.Int32>(op.Value
            
            #line default
            #line hidden
, 516), false)
);

WriteLiteral(">");

            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
                                     Write(op.Caption);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 9 "..\..\Areas\Sph\Views\App\EditorTemplates\ComboBox.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </select>\r\n    </div>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591