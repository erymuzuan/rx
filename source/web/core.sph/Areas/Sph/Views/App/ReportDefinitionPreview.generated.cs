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

namespace Bespoke.Sph.Web.Areas.Sph.Views.App
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
    
    #line 1 "..\..\Areas\Sph\Views\App\ReportDefinitionPreview.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/ReportDefinitionPreview.cshtml")]
    public partial class ReportDefinitionPreview : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.RdlExecutionViewModel>
    {
        public ReportDefinitionPreview()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\Sph\Views\App\ReportDefinitionPreview.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 8 "..\..\Areas\Sph\Views\App\ReportDefinitionPreview.cshtml"
 foreach (var layout in Model.Rdl.ReportLayoutCollection)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

            
            #line 11 "..\..\Areas\Sph\Views\App\ReportDefinitionPreview.cshtml"
        
            
            #line default
            #line hidden
            
            #line 11 "..\..\Areas\Sph\Views\App\ReportDefinitionPreview.cshtml"
         foreach (var item in layout.ReportItemCollection)
        {
            
            
            #line default
            #line hidden
            
            #line 13 "..\..\Areas\Sph\Views\App\ReportDefinitionPreview.cshtml"
       Write(Html.DisplayFor(m => item));

            
            #line default
            #line hidden
            
            #line 13 "..\..\Areas\Sph\Views\App\ReportDefinitionPreview.cshtml"
                                       
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 16 "..\..\Areas\Sph\Views\App\ReportDefinitionPreview.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
