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
    
    #line 1 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/PartialViewDesigner/Html.cshtml")]
    public partial class _Areas_App_Views_PartialViewDesigner_Html_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public _Areas_App_Views_PartialViewDesigner_Html_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(" data-bind=\"with : entity\"");

WriteLiteral(">\r\n    <h1>\r\n        Partial View :\r\n        <span");

WriteLiteral(" data-bind=\"text:Name\"");

WriteLiteral("></span>\r\n    </h1>\r\n</div>\r\n");

            
            #line 14 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
Write(Html.Partial("_DesignerBuildError"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n    <!-- left toolbox-->\r\n    <div");

WriteLiteral(" class=\"col-sm-3 col-lg-3\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 19 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
   Write(Html.Partial("_Toolbox"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n\r\n    <div");

WriteLiteral(" id=\"template-form-designer\"");

WriteLiteral(" data-bind=\"with : form().FormDesign\"");

WriteLiteral(" class=\"col-lg-9 col-md-9\"");

WriteLiteral(">\r\n\r\n        <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n            <!--ko foreach:FormElementCollection -->\r\n");

            
            #line 26 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
            
            
            #line default
            #line hidden
            
            #line 26 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
             foreach (var fe in Model.FormElements)
            {
                var fe1 = fe;
                
            
            #line default
            #line hidden
            
            #line 29 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
           Write(Html.DisplayFor(m => fe1));

            
            #line default
            #line hidden
            
            #line 29 "..\..\Areas\App\Views\PartialViewDesigner\Html.cshtml"
                                          
            }

            
            #line default
            #line hidden
WriteLiteral("            <!--/ko-->\r\n        </form>\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591