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
    
    #line 1 "..\..\Views\CustomForm\Compare.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/CustomForm/Compare.cshtml")]
    public partial class _Views_CustomForm_Compare_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.PageCompareViewModel>
    {
        public _Views_CustomForm_Compare_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\CustomForm\Compare.cshtml"
  
    ViewBag.Title = "Diff";
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <title>Diff</title>\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 207), Tuple.Create("\"", 248)
            
            #line 11 "..\..\Views\CustomForm\Compare.cshtml"
, Tuple.Create(Tuple.Create("", 214), Tuple.Create<System.Object, System.Int32>(Url.Content("~/Content/Diff.css")
            
            #line default
            #line hidden
, 214), false)
);

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" type=\"text/css\"");

WriteLiteral(" />\r\n</head>\r\n<body>\r\n    <div");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" id=\"diffBox\"");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" id=\"leftPane\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"diffHeader\"");

WriteLiteral(">Theirs</div>\r\n");

            
            #line 18 "..\..\Views\CustomForm\Compare.cshtml"
                
            
            #line default
            #line hidden
            
            #line 18 "..\..\Views\CustomForm\Compare.cshtml"
                  
                    Html.RenderPartial("DiffPane", Model.Diff.NewText);
                
            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div");

WriteLiteral(" id=\"rightPane\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"diffHeader\"");

WriteLiteral(">Mine</div>\r\n");

            
            #line 24 "..\..\Views\CustomForm\Compare.cshtml"
                
            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\CustomForm\Compare.cshtml"
                  
                    Html.RenderPartial("DiffPane", Model.Diff.OldText);
                
            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"clear\"");

WriteLiteral(">\r\n            </div>\r\n        </div>\r\n\r\n    </div>\r\n\r\n</body>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
