﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Views.Shared.DisplayTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/CurrencyElement.cshtml")]
    public partial class CurrencyElement : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.CurrencyElement>
    {
        public CurrencyElement()
        {
        }
        public override void Execute()
        {

WriteLiteral("<div class=\"form-group\">\r\n\r\n    <label class=\"col-md-4\">");


            
            #line 4 "..\..\Views\Shared\DisplayTemplates\CurrencyElement.cshtml"
                       Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <span class=\"col-md-8\" data-bind=\"money: ");


            
            #line 5 "..\..\Views\Shared\DisplayTemplates\CurrencyElement.cshtml"
                                        Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\"></span>\r\n\r\n</div>");


        }
    }
}
#pragma warning restore 1591
