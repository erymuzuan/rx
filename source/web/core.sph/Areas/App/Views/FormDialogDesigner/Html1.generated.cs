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
    
    #line 1 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/FormDialogDesigner/Html.cshtml")]
    public partial class _Areas_App_Views_FormDialogDesigner_Html_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public _Areas_App_Views_FormDialogDesigner_Html_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(" data-bind=\"with : entity\"");

WriteLiteral(">\r\n    <h1>\r\n        Dialog :\r\n        <span");

WriteLiteral(" data-bind=\"text:Name\"");

WriteLiteral("></span>\r\n    </h1>\r\n</div>\r\n");

            
            #line 14 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
Write(Html.Partial("_DesignerBuildError"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n    <!-- left toolbox-->\r\n    <div");

WriteLiteral(" class=\"col-sm-3 col-lg-3\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 19 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
   Write(Html.Partial("_Toolbox"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    <!-- ko with : form -->\r\n\r\n    <div");

WriteLiteral(" class=\"modalBlockout col-sm-9 col-lg-9\"");

WriteLiteral(" style=\"opacity: 0.2; min-height: 600px\"");

WriteLiteral("></div>\r\n    <div");

WriteLiteral(" class=\"modalHost\"");

WriteLiteral(" style=\"opacity: 1;\"");

WriteLiteral(">\r\n        <section");

WriteLiteral(" class=\"view-model-modal\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"modal-dialog\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"modal-content\"");

WriteLiteral(">\r\n\r\n                    <div");

WriteLiteral(" class=\"modal-header\"");

WriteLiteral(" style=\"cursor: move;\"");

WriteLiteral(">\r\n                        <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">&times;</button>\r\n                        <h3");

WriteLiteral(" data-bind=\"text: Title\"");

WriteLiteral("></h3>\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"modal-body\"");

WriteLiteral(" id=\"template-form-designer\"");

WriteLiteral(" style=\"min-height: 200px\"");

WriteLiteral("  data-bind=\"with : FormDesign\"");

WriteLiteral(">\r\n\r\n                        <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n                            <!--ko foreach:FormElementCollection -->\r\n");

            
            #line 37 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 37 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
                             foreach (var fe in Model.FormElements)
                            {
                                var fe1 = fe;
                                
            
            #line default
            #line hidden
            
            #line 40 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
                           Write(Html.DisplayFor(m => fe1));

            
            #line default
            #line hidden
            
            #line 40 "..\..\Areas\App\Views\FormDialogDesigner\Html.cshtml"
                                                          
                            }

            
            #line default
            #line hidden
WriteLiteral("                            <!--/ko-->\r\n                        </form>\r\n        " +
"            </div>\r\n                    <div");

WriteLiteral(" class=\"modal-footer\"");

WriteLiteral(">\r\n                        <!--ko foreach:DialogButtonCollection -->\r\n           " +
"             <button");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" data-bind=\"text:Text\"");

WriteLiteral(">OK</button>\r\n\r\n                        <!--/ko-->\r\n                    </div>\r\n " +
"               </div>\r\n            </div>\r\n        </section>\r\n    </div>\r\n\r\n\r\n " +
"   <!-- /ko -->\r\n</div>");

        }
    }
}
#pragma warning restore 1591
