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

namespace Bespoke.Sph.Web.Areas.Sph.Views.SphAccount
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
    
    #line 8 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
    using System.Web.Optimization;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/SphAccount/ChangePassword.cshtml")]
    public partial class ChangePassword : System.Web.Mvc.WebViewPage<dynamic>
    {
        public ChangePassword()
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

            
            #line 13 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral(" - SPH</title>\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 254), Tuple.Create("\"", 274)
, Tuple.Create(Tuple.Create("", 261), Tuple.Create<System.Object, System.Int32>(Href("~/favicon.ico")
, 261), false)
);

WriteLiteral(" rel=\"shortcut icon\"");

WriteLiteral(" type=\"image/x-icon\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width\"");

WriteLiteral(" />\r\n");

WriteLiteral("    ");

            
            #line 16 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
Write(Styles.Render("~/Content/css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n</head>\r\n<body>\r\n    <div");

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

WriteLiteral(" class=\"span4 offset2 login-form\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"logo\"");

WriteLiteral(">\r\n                            <img");

WriteAttribute("src", Tuple.Create(" src=\"", 804), Tuple.Create("\"", 831)
, Tuple.Create(Tuple.Create("", 810), Tuple.Create<System.Object, System.Int32>(Href("~/Images/logo_sph.png")
, 810), false)
);

WriteLiteral(" alt=\"logo\"");

WriteLiteral(" />\r\n                        </div>\r\n                        <h2>Tukar kata lalua" +
"n</h2>\r\n\r\n\r\n\r\n                        <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n\r\n                            <p>\r\n                                New passwor" +
"ds are required to be a minimum of ");

            
            #line 37 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
                                                                         Write(Membership.MinRequiredPasswordLength);

            
            #line default
            #line hidden
WriteLiteral(" characters in length.\r\n                            </p>\r\n                       " +
"     <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"oldPassword\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Kata laluan asal</label>\r\n                                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                    <input");

WriteLiteral(" required=\"\"");

WriteLiteral(" data-bind=\"value: oldPassword\"");

WriteLiteral(" id=\"oldPassword-\"");

WriteLiteral(" type=\"password\"");

WriteLiteral(" name=\"oldPassword\"");

WriteLiteral(" />\r\n                                </div>\r\n                            </div>\r\n" +
"\r\n                            <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"password\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Kata laluan baru</label>\r\n                                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                    <input");

WriteAttribute("min", Tuple.Create(" min=\"", 1898), Tuple.Create("\"", 1952)
            
            #line 49 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
, Tuple.Create(Tuple.Create("", 1904), Tuple.Create<System.Object, System.Int32>(Membership.MinRequiredNonAlphanumericCharacters
            
            #line default
            #line hidden
, 1904), false)
);

WriteLiteral(" required");

WriteLiteral(" data-bind=\"value: password\"");

WriteLiteral(" id=\"password\"");

WriteLiteral(" type=\"password\"");

WriteLiteral(" name=\"password\"");

WriteLiteral(" />\r\n                                </div>\r\n                            </div>\r\n" +
"\r\n                            <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"confirm-password\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Ulang kata laluan</label>\r\n                                <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                    <input");

WriteAttribute("min", Tuple.Create(" min=\"", 2385), Tuple.Create("\"", 2439)
            
            #line 56 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
, Tuple.Create(Tuple.Create("", 2391), Tuple.Create<System.Object, System.Int32>(Membership.MinRequiredNonAlphanumericCharacters
            
            #line default
            #line hidden
, 2391), false)
);

WriteLiteral(" required");

WriteLiteral(" data-bind=\"value: confirmPassword\"");

WriteLiteral(" id=\"confirm-password\"");

WriteLiteral(" type=\"password\"");

WriteLiteral(" name=\"confirmPassword\"");

WriteLiteral(" />\r\n                                </div>\r\n                            </div>\r\n" +
"\r\n                            <input");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" value=\"Tukar\"");

WriteLiteral(" data-bind=\"command: submit\"");

WriteLiteral(" />\r\n                        </form>\r\n\r\n\r\n\r\n                    </div>\r\n         " +
"       </div>\r\n");

WriteLiteral("                <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                    <hr");

WriteLiteral(" class=\"span12\"");

WriteLiteral(" />\r\n                </div>\r\n");

            
            #line 70 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
                
            
            #line default
            #line hidden
            
            #line 70 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
           Write(Html.Partial("_Footer"));

            
            #line default
            #line hidden
            
            #line 70 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
                                        


            }

            
            #line default
            #line hidden
WriteLiteral("        </section>\r\n    </div>\r\n\r\n");

WriteLiteral("    ");

            
            #line 77 "..\..\Areas\Sph\Views\SphAccount\ChangePassword.cshtml"
Write(Scripts.Render("~/scripts/vendor"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(function () {\r\n            $(\'.carousel\').carousel({\r\n              " +
"  interval: 2000\r\n            });\r\n        });\r\n    </script>\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 3265), Tuple.Create("\"", 3300)
, Tuple.Create(Tuple.Create("", 3271), Tuple.Create<System.Object, System.Int32>(Href("~/App/durandal/amd/require.js")
, 3271), false)
);

WriteLiteral("></script>\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 3324), Tuple.Create("\"", 3353)
, Tuple.Create(Tuple.Create("", 3330), Tuple.Create<System.Object, System.Int32>(Href("~/App/objectbuilders.js")
, 3330), false)
);

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
        require.config({
            baseUrl: ""/App"",
            waitSeconds: 15
        });

        $(function () {


            var vm = {
                oldPassword: ko.observable(),
                password: ko.observable(),
                confirmPassword: ko.observable(),
                submit: function () {
                    var tcs = new $.Deferred();
                    var data = JSON.stringify({ model: ko.toJS(vm) });
                    require([objectbuilders.datacontext, objectbuilders.config, objectbuilders.logger],
                        function (context, config, logger) {

                            context.post(data, ""/SphAccount/ChangePassword"")
                                .then(function (result) {
                                    tcs.resolve(result);
                                    if (result.status !== ""OK"") {
                                        logger.error(result.status + ""<br/>"" + result.message);
                                    } else {
                                        window.location = ""/#/"" + config.startModule;
                                    }
                                });
                        });
                    return tcs.promise();
                }
            };
            ko.applyBindings(vm);

        });
    </script>
</body>
</html>
");

        }
    }
}
#pragma warning restore 1591
