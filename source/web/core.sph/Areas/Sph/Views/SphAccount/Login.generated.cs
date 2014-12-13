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
    
    #line 6 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/SphAccount/Login.cshtml")]
    public partial class Login : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.Areas.Sph.Controllers.LoginModel>
    {
        public Login()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
  
    ViewBag.Title = "Log in";
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("<!DOCTYPE html>\r\n<html");

WriteLiteral(" lang=\"en\"");

WriteLiteral(">\r\n<head>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(" />\r\n    <title>");

            
            #line 11 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral(" - SPH</title>\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 266), Tuple.Create("\"", 286)
, Tuple.Create(Tuple.Create("", 273), Tuple.Create<System.Object, System.Int32>(Href("~/favicon.ico")
, 273), false)
);

WriteLiteral(" rel=\"shortcut icon\"");

WriteLiteral(" type=\"image/x-icon\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width\"");

WriteLiteral(" />\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 400), Tuple.Create("\"", 426)
, Tuple.Create(Tuple.Create("", 407), Tuple.Create<System.Object, System.Int32>(Href("~/Content/__css.css")
, 407), false)
);

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n\r\n</head>\r\n<body>\r\n    <div");

WriteLiteral(" id=\"body\"");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n\r\n        <section>\r\n\r\n");

            
            #line 22 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
            
            
            #line default
            #line hidden
            
            #line 22 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
             using (Html.BeginForm((string)ViewBag.FormAction, "SphAccount"))
            {


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 26 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
               Write(Html.Partial("_Slider"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    <div");

WriteLiteral(" class=\"col-lg-4 col-lg-offset-2 login-form\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"logo\"");

WriteLiteral(">\r\n                            <img");

WriteLiteral(" src=\"/Images/logo_sph.png\"");

WriteLiteral(" alt=\"logo\"");

WriteLiteral("/>\r\n                        </div>\r\n                        <h2>Login Page</h2>\r\n" +
"");

            
            #line 32 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 32 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
                         if (!@ViewData.ModelState.IsValid)
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <div");

WriteLiteral(" class=\"alert alert-danger\"");

WriteLiteral(">\r\n                                <a");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"alert\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(">&times;</a>\r\n                                Incorrect userName or password!\r\n  " +
"                          </div>\r\n");

            
            #line 38 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"

                        }

            
            #line default
            #line hidden
WriteLiteral("                        <form");

WriteLiteral(" method=\"POST\"");

WriteLiteral(" action=\"\"");

WriteLiteral(" accept-charset=\"UTF-8\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 41 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
                       Write(Html.TextBoxFor(m => m.UserName, new {placeholder = "UserName", @class = "form-control", required = true, autofocus = "autofocus"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 42 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
                       Write(Html.ValidationMessageFor(m => m.UserName));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            <label");

WriteLiteral(" class=\"controls\"");

WriteLiteral("></label>\r\n");

WriteLiteral("                            ");

            
            #line 44 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
                       Write(Html.PasswordFor(m => m.Password, new {placeholder = "Password", @class = "form-control", required = true}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 45 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
                       Write(Html.ValidationMessageFor(m => m.Password));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                                <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"remember\"");

WriteLiteral(" value=\"1\"");

WriteLiteral(">\r\n                                Remember me\r\n                            </lab" +
"el>\r\n\r\n                            <input");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" value=\"\"");

WriteLiteral(" name=\"ReturnUrl\"");

WriteLiteral("/>\r\n                            <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" name=\"submit\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(">Login</button>\r\n                        </form>\r\n\r\n                        <div>" +
"\r\n                            <p>\r\n                                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2469), Tuple.Create("\"", 2505)
            
            #line 57 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
, Tuple.Create(Tuple.Create("", 2476), Tuple.Create<System.Object, System.Int32>(Url.Action("ChangePassword")
            
            #line default
            #line hidden
, 2476), false)
);

WriteLiteral(">Forgot your password</a>\r\n                            </p>\r\n                    " +
"    </div>\r\n                    </div>\r\n\r\n                </div>\r\n");

            
            #line 63 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
              


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                    <hr");

WriteLiteral(" class=\"col-lg-12\"");

WriteLiteral("/>\r\n                    <h3>Learn to create your own Login page!!!</h3>\r\n        " +
"            <p>Reactive Developer allows you to create your own branded Login</p" +
">\r\n                    <a");

WriteLiteral(" href=\"/docs/#custom-login-page\"");

WriteLiteral(">Got to help</a>\r\n                </div>\r\n");

            
            #line 71 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
                
            
            #line default
            #line hidden
            
            #line 71 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
           Write(Html.Partial("_Footer"));

            
            #line default
            #line hidden
            
            #line 71 "..\..\Areas\Sph\Views\SphAccount\Login.cshtml"
                                        

            }

            
            #line default
            #line hidden
WriteLiteral("        </section>\r\n    </div>\r\n\r\n\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 3113), Tuple.Create("\"", 3144)
, Tuple.Create(Tuple.Create("", 3119), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/__vendor.min.js")
, 3119), false)
);

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
        $(function () {
            $('.carousel').carousel({
                interval: 2000
            });
            var retUrl = getParameterByName(""ReturnUrl"") + window.location.hash;
            $('input[name=ReturnUrl]').val(retUrl);

            function getParameterByName(name) {
                name = name.replace(/[\[]/, ""\\\["").replace(/[\]]/, ""\\\]"");
                var regex = new RegExp(""[\\?&]"" + name + ""=([^&#]*)""),
                    results = regex.exec(location.search);
                return results == null ? """" : decodeURIComponent(results[1].replace(/\+/g, "" ""));
            }
        });
    </script>
</body>
</html>
");

        }
    }
}
#pragma warning restore 1591
