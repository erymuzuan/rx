﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/EditorTemplates/TextBox.cshtml")]
    public partial class _Areas_Sph_Views_App_EditorTemplates_TextBox_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.TextBox>
    {
        public _Areas_Sph_Views_App_EditorTemplates_TextBox_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("           ");

            
            #line 2 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
             
               var required = Model.IsRequired ? "required" : null;
           
            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" data-bind=\"visible:");

            
            #line 5 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
                   Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" data-i8n=\"");

            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
                Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("for", Tuple.Create(" for=\"", 228), Tuple.Create("\"", 250)
            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
, Tuple.Create(Tuple.Create("", 234), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 234), false)
);

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">");

            
            #line 6 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
                                                                                    Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n        <input ");

            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
          Write(required);

            
            #line default
            #line hidden
WriteLiteral(" class=\"");

            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
                            Write(Model.CssClass + " form-control " + Model.Size);

            
            #line default
            #line hidden
WriteLiteral("\" title=\"");

            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
                                                                                     Write(Model.Tooltip);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n               data-bind=\"");

            
            #line 9 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
                      Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"\r\n               id=\"");

            
            #line 10 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
              Write(Model.ElementId);

            
            #line default
            #line hidden
WriteLiteral("\" type=\"text\" name=\"");

            
            #line 10 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
                                                  Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n    </div>\r\n");

            
            #line 12 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
    
            
            #line default
            #line hidden
            
            #line 12 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
     if (!string.IsNullOrWhiteSpace(Model.HelpText))
    {

            
            #line default
            #line hidden
WriteLiteral("        <span");

WriteLiteral(" class=\"help-block\"");

WriteLiteral(">");

            
            #line 14 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
                            Write(Model.HelpText);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

            
            #line 15 "..\..\Areas\Sph\Views\App\EditorTemplates\TextBox.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>");

        }
    }
}
#pragma warning restore 1591
