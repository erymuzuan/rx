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
    
    #line 1 "..\..\Areas\Sph\Views\FormDialogRenderer\_FormContent.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Areas\Sph\Views\FormDialogRenderer\_FormContent.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/FormDialogRenderer/_FormContent.cshtml")]
    public partial class _Areas_Sph_Views_FormDialogRenderer__FormContent_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.DialogRendererViewModel>
    {
        public _Areas_Sph_Views_FormDialogRenderer__FormContent_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\Sph\Views\FormDialogRenderer\_FormContent.cshtml"
  

    Layout = null;
    var formId = @Model.Dialog.Route + "-form";

            
            #line default
            #line hidden
WriteLiteral("\r\n<form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(" data-bind=\"with : entity\"");

WriteAttribute("id", Tuple.Create(" id=\"", 247), Tuple.Create("\"", 259)
            
            #line 9 "..\..\Areas\Sph\Views\FormDialogRenderer\_FormContent.cshtml"
, Tuple.Create(Tuple.Create("", 252), Tuple.Create<System.Object, System.Int32>(formId
            
            #line default
            #line hidden
, 252), false)
);

WriteLiteral(">\r\n");

            
            #line 10 "..\..\Areas\Sph\Views\FormDialogRenderer\_FormContent.cshtml"
    
            
            #line default
            #line hidden
            
            #line 10 "..\..\Areas\Sph\Views\FormDialogRenderer\_FormContent.cshtml"
     foreach (var fe in Model.Dialog.FormDesign.FormElementCollection)
    {
        var fe1 = fe;
        fe1.Path = fe1.Path.ConvertJavascriptObjectToFunction();
        var button = fe1 as Button;
        if (null != button && button.IsToolbarItem)
        {
            // toolbar button
        }
        else
        {
            
            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\Sph\Views\FormDialogRenderer\_FormContent.cshtml"
        Write(fe.UseDisplayTemplate ? Html.DisplayFor(f => fe1) : Html.EditorFor(f => fe1));

            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\Sph\Views\FormDialogRenderer\_FormContent.cshtml"
                                                                                           
        }
    }

            
            #line default
            #line hidden
WriteLiteral("</form>");

        }
    }
}
#pragma warning restore 1591
