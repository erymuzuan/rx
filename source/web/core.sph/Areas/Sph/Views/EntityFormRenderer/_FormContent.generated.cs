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

namespace Bespoke.Sph.Web.Areas.Sph.Views.EntityFormRenderer
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
    
    #line 1 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/EntityFormRenderer/_FormContent.cshtml")]
    public partial class FormContent : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.FormRendererViewModel>
    {
        public FormContent()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
  

    Layout = null;
    var formId = @Model.Form.Route + "-form";

            
            #line default
            #line hidden
WriteLiteral("\r\n<form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(" data-bind=\"with : entity\"");

WriteAttribute("id", Tuple.Create(" id=\"", 215), Tuple.Create("\"", 227)
            
            #line 8 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
, Tuple.Create(Tuple.Create("", 220), Tuple.Create<System.Object, System.Int32>(formId
            
            #line default
            #line hidden
, 220), false)
);

WriteLiteral(">\r\n");

            
            #line 9 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
    
            
            #line default
            #line hidden
            
            #line 9 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
     foreach (var fe in Model.Form.FormDesign.FormElementCollection)
    {
        var fe1 = fe;
        fe1.Path = fe1.Path.ConvertJavascriptObjectToFunction();
        var button = fe1 as Button;



            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" data-bind=\"visible:");

            
            #line 16 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
                           Write(fe.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 522), Tuple.Create("\"", 541)
            
            #line 17 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
, Tuple.Create(Tuple.Create("", 528), Tuple.Create<System.Object, System.Int32>(fe.ElementId
            
            #line default
            #line hidden
, 528), false)
);

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">");

            
            #line 17 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
                                                                 Write(fe.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n");

            
            #line 19 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
                
            
            #line default
            #line hidden
            
            #line 19 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
                 if (null != button && button.IsToolbarItem)
                {
                // toolbar button
                }
                else
                {
                
            
            #line default
            #line hidden
            
            #line 25 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
            Write(fe.UseDisplayTemplate ? Html.Raw(fe1.GenerateDisplayTemplate("DurandalJs",Model.EntityDefinition)) : Html.Raw(fe1.GenerateEditorTemplate("DurandalJs", Model.EntityDefinition)));

            
            #line default
            #line hidden
            
            #line 25 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
                                                                                                                                                                                                  
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n");

            
            #line 28 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
            
            
            #line default
            #line hidden
            
            #line 28 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
             if (!string.IsNullOrWhiteSpace(fe.HelpText))
            {

            
            #line default
            #line hidden
WriteLiteral("                <span");

WriteLiteral(" class=\"help-block\"");

WriteLiteral(">");

            
            #line 30 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
                                    Write(fe.HelpText);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

            
            #line 31 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n");

            
            #line 33 "..\..\Areas\Sph\Views\EntityFormRenderer\_FormContent.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</form>");

        }
    }
}
#pragma warning restore 1591
