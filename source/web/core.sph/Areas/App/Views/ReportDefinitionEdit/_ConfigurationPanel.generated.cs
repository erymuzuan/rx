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
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ReportDefinitionEdit/_ConfigurationPanel.cshtml")]
    public partial class _Areas_App_Views_ReportDefinitionEdit__ConfigurationPanel_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_App_Views_ReportDefinitionEdit__ConfigurationPanel_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"modal fade view-model-modal\"");

WriteLiteral(" id=\"rdl-configuration-panel\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"modal-dialog\"");

WriteLiteral(" style=\"z-index: 1050\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"modal-content\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"modal-header\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">&times;</button>\r\n                <h3>Configuration</h3>\r\n            </div>\r\n  " +
"          <div");

WriteLiteral(" class=\"modal-body\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"tabbable\"");

WriteLiteral(">\r\n                    <ul");

WriteLiteral(" class=\"nav nav-tabs\"");

WriteLiteral(">\r\n                        <li");

WriteLiteral(" class=\"active\"");

WriteLiteral(">\r\n                            <a");

WriteLiteral(" href=\"#general-tab-item\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">General</a>\r\n                        </li>\r\n                        <li>\r\n      " +
"                      <a");

WriteLiteral(" href=\"#parameters-tab-item\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Parameters</a>\r\n                        </li>\r\n                        <li>\r\n   " +
"                         <a");

WriteLiteral(" href=\"#datasource-tab-item\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Datasource</a>\r\n                        </li>\r\n                        <li>\r\n   " +
"                         <a");

WriteLiteral(" href=\"#column-options-tab-item\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Available Columns</a>\r\n                        </li>\r\n                        <l" +
"i>\r\n                            <a");

WriteLiteral(" href=\"#column-props-tab-item\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Columns Properties</a>\r\n                        </li>\r\n                    </ul>" +
"\r\n                    <div");

WriteLiteral(" class=\"tab-content\"");

WriteLiteral(" data-bind=\"with : reportDefinition\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" id=\"general-tab-item\"");

WriteLiteral(" class=\"tab-pane active\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 29 "..\..\Areas\App\Views\ReportDefinitionEdit\_ConfigurationPanel.cshtml"
                       Write(Html.Partial("_ConfigurationGeneral"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                        <div");

WriteLiteral(" id=\"parameters-tab-item\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 32 "..\..\Areas\App\Views\ReportDefinitionEdit\_ConfigurationPanel.cshtml"
                       Write(Html.Partial("_ConfigurationParameters"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                        <div");

WriteLiteral(" id=\"datasource-tab-item\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 35 "..\..\Areas\App\Views\ReportDefinitionEdit\_ConfigurationPanel.cshtml"
                       Write(Html.Partial("_ConfigurationDatasource"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                        <div");

WriteLiteral(" id=\"column-options-tab-item\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 38 "..\..\Areas\App\Views\ReportDefinitionEdit\_ConfigurationPanel.cshtml"
                       Write(Html.Partial("_ConfigurationColumnOptions"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                        </div>\r\n                        <div");

WriteLiteral(" id=\"column-props-tab-item\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 42 "..\..\Areas\App\Views\ReportDefinitionEdit\_ConfigurationPanel.cshtml"
                       Write(Html.Partial("_ConfigurationColumnProperties"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n\r\n                    </div>\r\n                <" +
"/div>\r\n            </div>\r\n\r\n            <div");

WriteLiteral(" class=\"modal-footer\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"btn btn-close-configuration-dialog btn-default\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">Close</a>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
