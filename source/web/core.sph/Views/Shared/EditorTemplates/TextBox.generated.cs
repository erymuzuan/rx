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
    
    #line 1 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/TextBox.cshtml")]
    public partial class _Views_Shared_EditorTemplates_TextBox_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.TextBox>
    {
        
        #line 15 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
 

    public static IHtmlString @Attribute(string value, string attr)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new HtmlString("");
        return new HtmlString($"{attr}=\"{value}\"");
    }

    public static IHtmlString @Attribute(int? value, string attr)
    {
        if (null == value)
            return new HtmlString("");
        return new HtmlString($"{attr}=\"{value}\"");
    }


        #line default
        #line hidden
        
        public _Views_Shared_EditorTemplates_TextBox_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
  
    var required = (Model.IsRequired || Model.FieldValidation.IsRequired) ? "required" : null;
    var maxLength = Model.FieldValidation.MaxLength;
    var pattern = (string.IsNullOrWhiteSpace(Model.FieldValidation.Pattern) ? null : Model.FieldValidation.Pattern);
    var originalPath = Model.Path.ToEmptyString().Replace("().", ".");

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 9 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
 if (string.IsNullOrWhiteSpace(Model.Enable))
{
    Model.Enable = "true";
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("\r\n");

            
            #line 33 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
 if (Model.IsCompact)
{


            
            #line default
            #line hidden
WriteLiteral("    <input ");

            
            #line 36 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
      Write(required);

            
            #line default
            #line hidden
WriteLiteral(" class=\"");

            
            #line 36 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                        Write(Model.CssClass + " form-control " + Model.Size);

            
            #line default
            #line hidden
WriteLiteral("\" title=\"");

            
            #line 36 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                                                                                 Write(Model.Tooltip);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n           data-bind=\"");

            
            #line 37 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                  Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"\r\n");

WriteLiteral("           ");

            
            #line 38 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
      Write(Html.Raw(Model.FieldValidation.GetHtmlAttributes()));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("           ");

            
            #line 39 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
      Write(Attribute(Model.ElementId, "id"));

            
            #line default
            #line hidden
WriteLiteral("\r\n           type=\"text\"\r\n           name=\"");

            
            #line 41 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
            Write(originalPath);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n");

WriteLiteral("           ");

            
            #line 42 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
      Write(Attribute(maxLength, "maxlength"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("           ");

            
            #line 43 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
      Write(Attribute(pattern, "pattern"));

            
            #line default
            #line hidden
WriteLiteral(" />\r\n");

            
            #line 44 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
}
else
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" data-bind=\"visible:");

            
            #line 47 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                       Write(Html.Raw(Model.Visible));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" data-i18n=\"");

            
            #line 48 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                     Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("for", Tuple.Create(" for=\"", 1539), Tuple.Create("\"", 1561)
            
            #line 48 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
, Tuple.Create(Tuple.Create("", 1545), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 1545), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 1562), Tuple.Create("\"", 1590)
            
            #line 48 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
, Tuple.Create(Tuple.Create("", 1570), Tuple.Create<System.Object, System.Int32>(Model.LabelCssClass
            
            #line default
            #line hidden
, 1570), false)
);

WriteLiteral(">");

            
            #line 48 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                                                                                       Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1626), Tuple.Create("\"", 1659)
            
            #line 49 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
, Tuple.Create(Tuple.Create("", 1634), Tuple.Create<System.Object, System.Int32>(Model.InputPanelCssClass
            
            #line default
            #line hidden
, 1634), false)
);

WriteLiteral(">\r\n            <input ");

            
            #line 50 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
              Write(required);

            
            #line default
            #line hidden
WriteLiteral(" class=\"");

            
            #line 50 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                                Write(Model.CssClass + " form-control " + Model.Size);

            
            #line default
            #line hidden
WriteLiteral("\" title=\"");

            
            #line 50 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                                                                                         Write(Model.Tooltip);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n                   data-bind=\"");

            
            #line 51 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                          Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"\r\n");

WriteLiteral("                   ");

            
            #line 52 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
              Write(Attribute(maxLength, "maxlength"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                   ");

            
            #line 53 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
              Write(Attribute(pattern, "pattern"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                   ");

            
            #line 54 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
              Write(Html.Raw(Model.FieldValidation.GetHtmlAttributes()));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                   ");

            
            #line 55 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
              Write(Attribute(Model.ElementId, "id"));

            
            #line default
            #line hidden
WriteLiteral(" type=\"text\" name=\"");

            
            #line 55 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                                                                  Write(originalPath);

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n");

            
            #line 56 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
            
            
            #line default
            #line hidden
            
            #line 56 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
             if (!string.IsNullOrWhiteSpace(Model.HelpText))
            {

            
            #line default
            #line hidden
WriteLiteral("                <span");

WriteLiteral(" class=\"help-block\"");

WriteLiteral(">");

            
            #line 58 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
                                    Write(Model.HelpText);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

            
            #line 59 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n");

            
            #line 62 "..\..\Views\Shared\EditorTemplates\TextBox.cshtml"

}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
