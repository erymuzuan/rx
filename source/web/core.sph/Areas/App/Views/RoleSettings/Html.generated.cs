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
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/RoleSettings/Html.cshtml")]
    public partial class _Areas_App_Views_RoleSettings_Html_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.RoleSettingViewModel>
    {
        public _Areas_App_Views_RoleSettings_Html_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
  
    Layout = null;


            
            #line default
            #line hidden
WriteLiteral("\r\n<h1>Designation</h1>\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n    <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(" data-bind=\"with: designation\"");

WriteLiteral(">\r\n\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"Name\"");

WriteLiteral(" class=\"col-lg-2\"");

WriteLiteral(">Designation Name</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-9\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Name\"");

WriteLiteral(" id=\"Name\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Name\"");

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"description\"");

WriteLiteral(" class=\"col-lg-2\"");

WriteLiteral(">Description</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-9\"");

WriteLiteral(">\r\n                <textarea");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" rows=\"8\"");

WriteLiteral(" data-bind=\"value: Description\"");

WriteLiteral(" id=\"description\"");

WriteLiteral(" name=\"description\"");

WriteLiteral("></textarea>\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"StartModule\"");

WriteLiteral(" class=\"col-lg-2\"");

WriteLiteral(">Start Module</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-9\"");

WriteLiteral(">\r\n                <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"StartModule\"");

WriteLiteral(" data-bind=\"value: StartModule\"");

WriteLiteral(">\r\n");

            
            #line 30 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 30 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
                     foreach (var r in Model.Routes.Where(j => j.Nav))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <option");

WriteAttribute("value", Tuple.Create(" value=\"", 1171), Tuple.Create("\"", 1187)
            
            #line 32 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
, Tuple.Create(Tuple.Create("", 1179), Tuple.Create<System.Object, System.Int32>(r.Route
            
            #line default
            #line hidden
, 1179), false)
);

WriteLiteral(">");

            
            #line 32 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
                                            Write(r.Title);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 33 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"

                    }

            
            #line default
            #line hidden
WriteLiteral("                </select>\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"col-md-offset-2 col-md-6\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsSearchVisible\"");

WriteLiteral(" id=\"show-search-toolbar\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsSearchVisible\"");

WriteLiteral(" />\r\n                        Show search toolbar\r\n                    </label>\r\n " +
"               </div>\r\n            </div>\r\n        </div>\r\n");

            
            #line 48 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
        
            
            #line default
            #line hidden
            
            #line 48 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
         foreach (var e in Model.SearchableEntityOptions)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"col-md-offset-3 col-md-5\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                        <label>\r\n                            <input");

WriteLiteral(" data-bind=\"checked: SearchableEntityCollection,enable : IsSearchVisible\"");

WriteAttribute("id", Tuple.Create(" id=\"", 2070), Tuple.Create("\"", 2099)
            
            #line 54 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
                                 , Tuple.Create(Tuple.Create("", 2075), Tuple.Create<System.Object, System.Int32>(e
            
            #line default
            #line hidden
, 2075), false)
, Tuple.Create(Tuple.Create("", 2079), Tuple.Create("-show-search-toolbar", 2079), true)
);

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create(" name=\"", 2116), Tuple.Create("\"", 2142)
, Tuple.Create(Tuple.Create("", 2123), Tuple.Create("IsSearchVisible", 2123), true)
            
            #line 54 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
                                                                                                , Tuple.Create(Tuple.Create("", 2138), Tuple.Create<System.Object, System.Int32>(e
            
            #line default
            #line hidden
, 2138), false)
);

WriteAttribute("value", Tuple.Create(" value=\"", 2143), Tuple.Create("\"", 2153)
            
            #line 54 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
                                                                                                            , Tuple.Create(Tuple.Create("", 2151), Tuple.Create<System.Object, System.Int32>(e
            
            #line default
            #line hidden
, 2151), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("                            ");

            
            #line 55 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
                       Write(e);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </label>\r\n                    </div>\r\n                <" +
"/div>\r\n            </div>\r\n");

            
            #line 60 "..\..\Areas\App\Views\RoleSettings\Html.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"col-md-offset-2 col-md-6\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsMessageVisible\"");

WriteLiteral(" id=\"show-message-notification\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsMessageVisible\"");

WriteLiteral(" />\r\n                        Show message notification\r\n                    </lab" +
"el>\r\n                </div>\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"col-md-offset-2 col-md-6\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsHelpVisible\"");

WriteLiteral(" id=\"show-help\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsHelpVisible\"");

WriteLiteral(" />\r\n                        Show help link\r\n                    </label>\r\n      " +
"          </div>\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"show-help-uri\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Help Url</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: HelpUri, enable: IsHelpVisible\"");

WriteLiteral("\r\n                       placeholder=\"Help Uri, empty URL will default to Rx Help" +
"\"");

WriteLiteral("\r\n                       class=\"form-control\"");

WriteLiteral(" id=\"show-help-uri\"");

WriteLiteral(">\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <span>Please (<i");

WriteLiteral(" class=\"fa fa-check\"");

WriteLiteral("></i>)check the option to set the designation roles</span>\r\n        </div>\r\n\r\n   " +
"     <div>\r\n            <table");

WriteLiteral(" class=\"table table-striped table-condensed\"");

WriteLiteral(" data-bind=\"filter: { path: \'tbody>tr\' }\"");

WriteLiteral(">\r\n                <thead>\r\n                    <tr>\r\n                        <th" +
">#</th>\r\n                        <th>Role</th>\r\n                        <th></th" +
">\r\n                    </tr>\r\n                </thead>\r\n                <tbody");

WriteLiteral(" data-bind=\"foreach :$root.roleOptions\"");

WriteLiteral(">\r\n\r\n                    <tr>\r\n                        <td");

WriteLiteral(" data-bind=\"text:$index() + 1\"");

WriteLiteral("></td>\r\n                        <td>\r\n                            <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(" data-bind=\"value:$data, attr :{\'title\' : $data}\"");

WriteLiteral(">\r\n                                <input");

WriteLiteral(" data-bind=\"checked: $parent.RoleCollection, value : $data\"");

WriteLiteral("\r\n                                       type=\"checkbox\"");

WriteLiteral(" name=\"role\"");

WriteLiteral(" />\r\n                                <!-- ko text: $data -->\r\n                   " +
"             <!-- /ko-->\r\n                            </label>\r\n\r\n\r\n\r\n\r\n        " +
"                </td>\r\n                        <td>\r\n                           " +
" <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"click : $root.deleteRole.bind($parent, $data), visible: !($data.toLow" +
"erCase() === \'developers\' || $data.toLowerCase() === \'administrators\')\"");

WriteLiteral(">\r\n                                Delete\r\n                            </a>\r\n    " +
"                    </td>\r\n                    </tr>\r\n\r\n                </tbody>" +
"\r\n            </table>\r\n            <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"command : $root.addRole\"");

WriteLiteral(">\r\n                <i");

WriteLiteral(" class=\"fa fa-plus-circle\"");

WriteLiteral("></i>\r\n                Add a role\r\n            </a>\r\n        </div>\r\n    </form>\r" +
"\n\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
