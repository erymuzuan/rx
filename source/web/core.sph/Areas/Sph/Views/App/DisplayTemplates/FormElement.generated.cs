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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/DisplayTemplates/FormElement.cshtml")]
    public partial class _Areas_Sph_Views_App_DisplayTemplates_FormElement_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.FormElement>
    {
        public _Areas_Sph_Views_App_DisplayTemplates_FormElement_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"alert alert-error\"");

WriteLiteral(">\r\n    <strong>There\'s no EditorTemplate for ");

            
            #line 4 "..\..\Areas\Sph\Views\App\DisplayTemplates\FormElement.cshtml"
                                     Write(Model.GetType().Name);

            
            #line default
            #line hidden
WriteLiteral("</strong>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
