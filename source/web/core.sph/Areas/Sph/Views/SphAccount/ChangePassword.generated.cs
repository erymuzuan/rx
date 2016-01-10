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
    
    #line 7 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/SphAccount/ChangePassword.cshtml")]
    public partial class _Areas_Sph_Views_SphAccount_ChangePassword_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_Sph_Views_SphAccount_ChangePassword_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
  
    ViewBag.Title = "Change Password";
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n");

WriteLiteral("<!DOCTYPE html>\r\n<html");

WriteLiteral(" lang=\"en\"");

WriteLiteral(">\r\n<head>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(" />\r\n    <title>");

            
            #line 12 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral(" - SPH</title>\r\n    <link");

WriteLiteral(" href=\"/favicon.ico\"");

WriteLiteral(" rel=\"shortcut icon\"");

WriteLiteral(" type=\"image/x-icon\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width\"");

WriteLiteral(" />\r\n    <link");

WriteLiteral(" href=\"/Content/external/bootstrap.min.css\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n    <link");

WriteLiteral(" href=\"/Content/__css.min.css\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n</head>\r\n<body>\r\n    <div");

WriteLiteral(" id=\"body\"");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n        <section>\r\n");

            
            #line 21 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
            
            
            #line default
            #line hidden
            
            #line 21 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
             using (Html.BeginForm((string)ViewBag.FormAction, "SphAccount"))
            {


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 25 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
               Write(Html.Partial("_Slider"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    <div");

WriteLiteral(" class=\"col-sm-4 col-sm-offset-2 login-form\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"logo\"");

WriteLiteral(">\r\n                            <img");

WriteLiteral(" src=\"/images/logo_sph.png\"");

WriteLiteral(" alt=\"logo\"");

WriteLiteral(" />\r\n                        </div>\r\n                        <h2>Change Password<" +
"/h2>\r\n\r\n\r\n\r\n                        <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n\r\n                            <p>\r\n                                New passwor" +
"ds are required to be a minimum of ");

            
            #line 37 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
                                                                         Write(Membership.MinRequiredPasswordLength);

            
            #line default
            #line hidden
WriteLiteral(" characters in length.\r\n                            </p>\r\n\r\n                     " +
"       <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"oldPassword\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Original Password</label>\r\n                                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                    <input required");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: oldPassword\"");

WriteLiteral(" id=\"oldPassword-\"");

WriteLiteral(" type=\"password\"");

WriteLiteral(" name=\"oldPassword\"");

WriteLiteral(" />\r\n                                </div>\r\n                            </div>\r\n" +
"\r\n                            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"password\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">New Password</label>\r\n                                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                    <input");

WriteLiteral(" class=\"form-control\"");

WriteAttribute("min", Tuple.Create(" min=\"", 2003), Tuple.Create("\"", 2057)
            
            #line 50 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
, Tuple.Create(Tuple.Create("", 2009), Tuple.Create<System.Object, System.Int32>(Membership.MinRequiredNonAlphanumericCharacters
            
            #line default
            #line hidden
, 2009), false)
);

WriteLiteral(" required");

WriteLiteral(" data-bind=\"value: password\"");

WriteLiteral(" id=\"password\"");

WriteLiteral(" type=\"password\"");

WriteLiteral(" name=\"password\"");

WriteLiteral(" />\r\n                                </div>\r\n                            </div>\r\n" +
"\r\n                            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"confirm-password\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Confirm New Password</label>\r\n                                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                    <input");

WriteLiteral(" class=\"form-control\"");

WriteAttribute("min", Tuple.Create(" min=\"", 2511), Tuple.Create("\"", 2565)
            
            #line 57 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
, Tuple.Create(Tuple.Create("", 2517), Tuple.Create<System.Object, System.Int32>(Membership.MinRequiredNonAlphanumericCharacters
            
            #line default
            #line hidden
, 2517), false)
);

WriteLiteral(" required");

WriteLiteral(" data-bind=\"value: confirmPassword\"");

WriteLiteral(" id=\"confirm-password\"");

WriteLiteral(" type=\"password\"");

WriteLiteral(" name=\"confirmPassword\"");

WriteLiteral(" />\r\n                                </div>\r\n                            </div>\r\n" +
"                            <div");

WriteLiteral(" class=\"alert alert-danger\"");

WriteLiteral(" data-bind=\"visible:message\"");

WriteLiteral(">\r\n                                <span");

WriteLiteral(" data-bind=\"html:message\"");

WriteLiteral("></span>\r\n                            </div>\r\n\r\n                            <inpu" +
"t");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" value=\"Submit\"");

WriteLiteral(" data-bind=\"command: submit\"");

WriteLiteral(" />\r\n                        </form>\r\n\r\n\r\n\r\n                    </div>\r\n         " +
"       </div>\r\n");

WriteLiteral("                <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                    <hr");

WriteLiteral(" class=\"col-sm-12\"");

WriteLiteral(" />\r\n                </div>\r\n");

            
            #line 74 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
                
            
            #line default
            #line hidden
            
            #line 74 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
           Write(Html.Partial("_Footer"));

            
            #line default
            #line hidden
            
            #line 74 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
                                        


            }

            
            #line default
            #line hidden
WriteLiteral("        </section>\r\n    </div>\r\n\r\n    <script");

WriteLiteral(" src=\"/Scripts/__vendor.min.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" src=\"/Scripts/__core.min.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(function () {\r\n            $(\'.carousel\').carousel({\r\n              " +
"  interval: 2000\r\n            });\r\n        });\r\n    </script>\r\n    <script");

WriteLiteral(" src=\"/Scripts/require.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" src=\"/SphApp/objectbuilders.js\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        require.config({\r\n            baseUrl: \"/SphApp\",\r\n            waitSec" +
"onds: 15,\r\n            paths: {\r\n                \"text\": \"/Scripts/text\",\r\n     " +
"           \'durandal\': \'/Scripts/durandal\',\r\n                \'plugins\': \'/Script" +
"s/durandal/plugins\',\r\n                \'transitions\': \'/Scripts/durandal/transiti" +
"ons\',\r\n                \"jquery.contextmenu\": \"/scripts/jquery.contextMenu\",\r\n   " +
"             \"jquery.ui.position\": \"/scripts/jquery.ui.position\",\r\n             " +
"   \'bootstrap\': \'../Scripts/bootstrap\'\r\n            },\r\n            shim: {\r\n   " +
"             \'bootstrap\': {\r\n                    deps: [\'jquery\'],\r\n            " +
"        exports: \'jQuery\'\r\n                }\r\n            }\r\n        });\r\n\r\n\r\n  " +
"      define(\'jquery\', function () { return jQuery; });\r\n        define(\'knockou" +
"t\', ko);\r\n\r\n        $(function () {\r\n            var vm = {\r\n                old" +
"Password: ko.observable(),\r\n                password: ko.observable(),\r\n        " +
"        confirmPassword: ko.observable(),\r\n                message: ko.observabl" +
"e(),\r\n                submit: function () {\r\n                    var tcs = new $" +
".Deferred(),\r\n                        data = JSON.stringify({ model: ko.toJS(vm)" +
" });\r\n                    require([objectbuilders.datacontext, objectbuilders.co" +
"nfig],\r\n                        function (context, config) {\r\n\r\n                " +
"            context.post(data, \"/SphAccount/ChangePassword\")\r\n                  " +
"              .then(function (result) {\r\n                                    tcs" +
".resolve(result);\r\n                                    if (result.status !== \"OK" +
"\") {\r\n                                        vm.message(result.status + \"<br/>\"" +
" + result.message);\r\n                                    } else {\r\n             " +
"                           window.location = \"/sph#\" + config.startModule;\r\n    " +
"                                }\r\n                                });\r\n        " +
"                });\r\n                    return tcs.promise();\r\n                " +
"}\r\n            };\r\n            ko.applyBindings(vm);\r\n\r\n        });\r\n    </scrip" +
"t>\r\n</body>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
