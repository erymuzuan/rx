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

namespace Bespoke.Sph.Web.Areas.App.Views.EntityFormDesigner
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
    
    #line 1 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityFormDesigner/Html.cshtml")]
    public partial class Html : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public Html()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(" data-bind=\"with : entity\"");

WriteLiteral(">\r\n    <h1>\r\n        Custom entity form:\r\n        <span");

WriteLiteral(" data-bind=\"text:Name\"");

WriteLiteral("></span>\r\n    </h1>\r\n</div>\r\n<div");

WriteLiteral(" id=\"error-list-entity-form\"");

WriteLiteral(" class=\"row\"");

WriteLiteral(" data-bind=\"visible:errors().length\"");

WriteLiteral(">\r\n    <!-- ko foreach : errors -->\r\n    <div");

WriteLiteral(" class=\"col-sm-8 col-sm-offset-2 alert alert-dismissable alert-danger\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"alert\"");

WriteLiteral(" aria-hidden=\"true\"");

WriteLiteral(">&times;</button>\r\n        <i");

WriteLiteral(" class=\"fa fa-exclamation\"");

WriteLiteral("></i>\r\n        <span");

WriteLiteral(" data-bind=\"text:Message\"");

WriteLiteral("></span>\r\n        <!-- ko if : Code -->\r\n        <strong");

WriteLiteral(" class=\"icon-read-more\"");

WriteLiteral(" data-bind=\"bootstrapPopover : Code\"");

WriteLiteral("> ..more</strong>\r\n        <!-- /ko-->\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"col-sm-2\"");

WriteLiteral("></div>\r\n    <!-- /ko-->\r\n</div>\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n    <!-- left toolbox-->\r\n    <div");

WriteLiteral(" class=\"col-sm-3\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 30 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
   Write(Html.Partial("_Toolbox"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n    <!-- ko with : form -->\r\n    <div");

WriteLiteral(" id=\"screen-activity-designer\"");

WriteLiteral(" class=\"col-sm-9\"");

WriteLiteral(" data-bind=\"with: FormDesign\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" id=\"template-form-designer\"");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(" data-bind=\"with : $root.form\"");

WriteLiteral(" id=\"toolbar-panel-design\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(">\r\n                    <i");

WriteLiteral(" class=\"fa fa-save\"");

WriteLiteral("></i>\r\n                    Save\r\n                </button>\r\n                <butt" +
"on");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" data-bind=\"visible:IsPrintAvailable\"");

WriteLiteral(">\r\n                    <i");

WriteLiteral(" class=\"fa fa-print\"");

WriteLiteral("></i>\r\n                    Print\r\n                </button>\r\n                <but" +
"ton");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" data-bind=\"visible:IsEmailAvailable\"");

WriteLiteral(">\r\n                    <i");

WriteLiteral(" class=\"fa fa-envelope\"");

WriteLiteral("></i>\r\n                    Email\r\n                </button>\r\n                <but" +
"ton");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" data-bind=\"visible:IsWatchAvailable\"");

WriteLiteral(">\r\n                    <i");

WriteLiteral(" class=\"fa fa-eye\"");

WriteLiteral("></i>\r\n                    Watch\r\n                </button>\r\n            </div>\r\n" +
"            <h1");

WriteLiteral(" data-bind=\"text: Name\"");

WriteLiteral("></h1>\r\n            <span");

WriteLiteral(" data-bind=\"text: Description\"");

WriteLiteral("></span>\r\n\r\n            <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n                <!--ko foreach:FormElementCollection -->\r\n\r\n\r\n");

            
            #line 60 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
                
            
            #line default
            #line hidden
            
            #line 60 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
                 foreach (var fe in Model.FormElements)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <!--ko if: ko.unwrap($type) === \"");

            
            #line 62 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
                                                 Write(fe.GetType().GetShortAssemblyQualifiedName());

            
            #line default
            #line hidden
WriteLiteral("\" -->\r\n");

WriteLiteral("                    <div");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 64 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
                   Write(Html.Partial("_DesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        <label");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral("></label>\r\n                        <div");

WriteLiteral(" class=\"col-lg-4\"");

WriteLiteral(">\r\n\r\n");

WriteLiteral("                            ");

            
            #line 68 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
                       Write(Html.Raw(fe.GetDesignSurfaceElement()));

            
            #line default
            #line hidden
WriteLiteral("\r\n                           \r\n                            <span");

WriteLiteral(" data-bind=\"text: HelpText\"");

WriteLiteral(" class=\"help-block\"");

WriteLiteral("></span>\r\n                        </div>\r\n                    </div>\r\n");

WriteLiteral("                    <!--/ko-->\r\n");

            
            #line 74 "..\..\Areas\App\Views\EntityFormDesigner\Html.cshtml"
                    
                }

            
            #line default
            #line hidden
WriteLiteral("                <!--/ko-->\r\n\r\n            </form>\r\n        </div>\r\n\r\n    </div>\r\n" +
"\r\n    <!-- /ko -->\r\n</div>");

        }
    }
}
#pragma warning restore 1591
