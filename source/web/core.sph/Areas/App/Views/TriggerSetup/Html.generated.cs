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
    
    #line 1 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    #line 2 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
    using Bespoke.Sph.Web.Models;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/TriggerSetup/Html.cshtml")]
    public partial class _Areas_App_Views_TriggerSetup_Html_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_App_Views_TriggerSetup_Html_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1>Trigger</h1>\r\n\r\n");

            
            #line 9 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
Write(Html.Partial("_errorList"));

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" id=\"action-list-panel\"");

WriteLiteral(" class=\"col-lg-7 col-md-6 col-sm-12 col-xs-12\"");

WriteLiteral(" data-bind=\"with : trigger\"");

WriteLiteral(">\r\n\r\n        <div");

WriteLiteral(" id=\"action-panel\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"btn-group\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" class=\"btn btn-link dropdown-toggle\"");

WriteLiteral(" data-toggle=\"drop-down\"");

WriteLiteral(">\r\n                    <i");

WriteLiteral(" class=\"fa fa-plus-circle\"");

WriteLiteral("></i>\r\n                    Add an action &nbsp;<span");

WriteLiteral(" class=\"caret\"");

WriteLiteral("></span>\r\n                </a>\r\n                <ul");

WriteLiteral(" class=\"dropdown-menu\"");

WriteLiteral(" data-bind=\"foreach :$root.actionOptions\"");

WriteLiteral(">\r\n                    <li>\r\n                        <a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" style=\"text-align: left\"");

WriteLiteral(" data-bind=\"click: $root.trigger().addAction(action.$type)\"");

WriteLiteral(">\r\n\r\n                            <!-- ko if :designer.FontAwesomeIcon-->\r\n       " +
"                     <i");

WriteLiteral(" data-bind=\"attr : {\'class\':\'fa fa-\'+ designer.FontAwesomeIcon + \' fa-fw\'}\"");

WriteLiteral(@"></i>
                            <!--/ko-->
                            Add <!-- ko text: designer.Name -->
                            <!-- /ko-->
                        </a>
                    </li>

                </ul>
            </div>

        </div>
        <table");

WriteLiteral(" class=\"table table-striped\"");

WriteLiteral(">\r\n            <thead>\r\n                <tr>\r\n                    <th>Title</th>\r" +
"\n                    <th>Note</th>\r\n                    <th>Is active</th>\r\n    " +
"                <th></th>\r\n                </tr>\r\n            </thead>\r\n        " +
"    <tbody");

WriteLiteral(" data-bind=\"foreach: ActionCollection\"");

WriteLiteral(">\r\n                <tr>\r\n                    <td>\r\n                        <a");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"click : $parent.editAction.call($parent,$data)\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(">\r\n\r\n                            <img");

WriteLiteral(" data-bind=\"attr: { src : \'/api/triggers/actions/\' + ko.unwrap($type) + \'/image\'}" +
"\"");

WriteLiteral(" height=\"16\"");

WriteLiteral(" width=\"16\"");

WriteLiteral(" class=\"pull-left\"");

WriteLiteral(" style=\"margin-right: 10px\"");

WriteLiteral(" alt=\".\"");

WriteLiteral(" />\r\n                            &nbsp;\r\n                            <!-- ko text" +
" : Title -->\r\n                            <!-- /ko -->\r\n                        " +
"</a>\r\n                    </td>\r\n                    <td>\r\n                     " +
"   <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"input-action-note form-control\"");

WriteLiteral(" data-bind=\"value: Note\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td>\r\n                       " +
" <input");

WriteLiteral(" class=\"input-action-isactive\"");

WriteLiteral(" data-bind=\"checked: IsActive\"");

WriteLiteral(" id=\"IsActive\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsActive\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td>\r\n                       " +
" <a");

WriteLiteral(" class=\"btn btn-mini\"");

WriteLiteral(" rel=\"nofollow\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click: $parent.removeAction.call($parent,$data)\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-times\"");

WriteLiteral("></i></a>\r\n                    </td>\r\n                </tr>\r\n            </tbody>" +
"\r\n        </table>\r\n\r\n    </div>\r\n    <div");

WriteLiteral(" id=\"view-properties-tab\"");

WriteLiteral(" class=\"col-lg-5 col-md-6 col-sm-12 col-xs-12\"");

WriteLiteral(">\r\n       \r\n        <ul");

WriteLiteral(" class=\"nav nav-tabs\"");

WriteLiteral(" data-bind2=\"filter : {path:\'>li\'}\"");

WriteLiteral(">\r\n            <li");

WriteLiteral(" class=\"active\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"#general-trigger-panel\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">General</a>\r\n            </li>\r\n            <li>\r\n                <a");

WriteLiteral(" href=\"#rules-panel\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Rules</a>\r\n            </li>\r\n            <li>\r\n                <a");

WriteLiteral(" href=\"#ref-assemblies-panel\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Referenced Assemblies</a>\r\n            </li>\r\n\r\n        </ul>\r\n        <div");

WriteLiteral(" class=\"tab-content\"");

WriteLiteral(" data-bind=\"with : trigger\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"tab-pane active\"");

WriteLiteral(" id=\"general-trigger-panel\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 85 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
           Write(Html.Partial("_generalTrigger"));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <div");

WriteLiteral(" id=\"rules-panel\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 89 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
           Write(Html.Partial("_rules"));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n            <div");

WriteLiteral(" id=\"ref-assemblies-panel\"");

WriteLiteral("  class=\"tab-pane\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 92 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
           Write(Html.Partial("_referencedAssemblies"));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
