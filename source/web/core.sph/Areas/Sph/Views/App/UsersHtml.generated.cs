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
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/UsersHtml.cshtml")]
    public partial class UsersHtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public UsersHtml()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Areas\Sph\Views\App\UsersHtml.cshtml"
  
    ViewBag.Title = "Users";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n    <h1");

WriteLiteral(" class=\"col-lg-12\"");

WriteLiteral(">Pengurusan Pengguna</h1>\r\n</div>\r\n<div");

WriteLiteral(" class=\"tabbable row\"");

WriteLiteral(">\r\n    <ul");

WriteLiteral(" class=\"nav nav-tabs\"");

WriteLiteral(">\r\n        <li");

WriteLiteral(" class=\"active\"");

WriteLiteral(">\r\n            <a");

WriteLiteral(" href=\"#user-tab\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Pengguna</a>\r\n        </li>\r\n        <li>\r\n            <a");

WriteLiteral(" href=\"#designation-tab\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Tetapan Jawatan</a>\r\n        </li>\r\n        <li>\r\n            <a");

WriteLiteral(" href=\"#department-tab\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Tetapan Jabatan</a>\r\n        </li>\r\n    </ul>\r\n\r\n    <div");

WriteLiteral(" class=\"tab-content\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" id=\"user-tab\"");

WriteLiteral(" class=\"tab-pane active\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"search-panel row\"");

WriteLiteral(" data-bind=\"with: searchTerm\"");

WriteLiteral(">\r\n                <form");

WriteLiteral(" class=\"form-inline col-lg-12\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"form-group col-lg-1\"");

WriteLiteral(">\r\n                            <label");

WriteLiteral(" class=\"\"");

WriteLiteral(">Jabatan</label>\r\n                        </div>\r\n                        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                            <select");

WriteLiteral(" class=\"form-control col-lg-2\"");

WriteLiteral(" data-bind=\"options: $root.departmentOptions, value: department, optionsText: \'Na" +
"me\', optionsValue: \'Name\'\"");

WriteLiteral("></select>\r\n                        </div>\r\n                        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                            <label");

WriteLiteral(" class=\"control-label col-lg-1 sr-only\"");

WriteLiteral(">Kata Kunci</label>\r\n                            <input");

WriteLiteral(" placeholder=\"Kata kunci\"");

WriteLiteral(" class=\"form-control col-lg-2\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: keyword\"");

WriteLiteral(" required />\r\n                        </div>\r\n                        <div");

WriteLiteral(" class=\"form-group col-lg-2 pull-right\"");

WriteLiteral(">\r\n                            <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-primary\"");

WriteLiteral(" id=\"search-btn\"");

WriteLiteral(">\r\n                                <i");

WriteLiteral(" class=\"fa fa-search pull-left\"");

WriteLiteral("></i>\r\n                                Search\r\n                            </butt" +
"on>\r\n                        </div>\r\n\r\n                    </div>\r\n\r\n           " +
"     </form>\r\n            </div>\r\n            <div>\r\n                <table");

WriteLiteral(" class=\"table table-striped\"");

WriteLiteral(" data-bind=\"serverPaging: { entity: \'UserProfile\', query: \'UserProfileId gt 0\', l" +
"ist: profiles, map: map }\"");

WriteLiteral(@">
                    <thead>
                        <tr>
                            <th>ID Pengguna</th>
                            <th>Nama</th>
                            <th>Jabatan</th>
                            <th>Fungsi</th>
                            <th>Emel</th>
                            <th>Password</th>
                        </tr>
                    </thead>
                    <tbody");

WriteLiteral(" data-bind=\"foreach: profiles\"");

WriteLiteral(">\r\n                        <tr>\r\n                            <td>\r\n              " +
"                  <a");

WriteLiteral(" data-toggle=\"modal\"");

WriteLiteral(" href=\"#user-details-modal\"");

WriteLiteral(" data-bind=\"click: $root.editCommand, text: UserName\"");

WriteLiteral("></a>\r\n\r\n                            </td>\r\n                            <td");

WriteLiteral(" data-bind=\"text: FullName\"");

WriteLiteral("></td>\r\n                            <td");

WriteLiteral(" data-bind=\"text: Department\"");

WriteLiteral("></td>\r\n                            <td");

WriteLiteral(" data-bind=\"text: Designation\"");

WriteLiteral("></td>\r\n                            <td");

WriteLiteral(" data-bind=\"text: Email\"");

WriteLiteral("></td>\r\n                            <td>\r\n                                <a");

WriteLiteral(" data-toggle=\"modal\"");

WriteLiteral(" href=\"#user-change-password\"");

WriteLiteral(" data-bind=\"click: $root.resetPasswordCommand\"");

WriteLiteral(">Reset</a>\r\n                            </td>\r\n                        </tr>\r\n   " +
"                 </tbody>\r\n                </table>\r\n            </div>\r\n       " +
" </div>\r\n        <div");

WriteLiteral(" id=\"designation-tab\"");

WriteLiteral(" class=\"tab-pane \"");

WriteLiteral(">\r\n            <!--ko compose:\'viewmodels/_users.designation\' -->\r\n            <!" +
"--/ko-->\r\n        </div>\r\n        <div");

WriteLiteral(" id=\"department-tab\"");

WriteLiteral(" class=\"tab-pane \"");

WriteLiteral(">\r\n            <!--ko compose:\'viewmodels/_users.department\' -->\r\n            <!-" +
"-/ko-->\r\n        </div>\r\n    </div>\r\n</div>\r\n<!--ko compose:\'_user.add.html\' -->" +
"\r\n<!--/ko-->\r\n<!--ko compose:\'_user.change.password.html\' -->\r\n<!--/ko-->");

        }
    }
}
#pragma warning restore 1591
