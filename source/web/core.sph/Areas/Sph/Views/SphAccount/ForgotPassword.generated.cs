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
    
    #line 6 "..\..\Areas\Sph\Views\SphAccount\ForgotPassword.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/SphAccount/ForgotPassword.cshtml")]
    public partial class _Areas_Sph_Views_SphAccount_ForgotPassword_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_Sph_Views_SphAccount_ForgotPassword_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Sph\Views\SphAccount\ForgotPassword.cshtml"
  
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

            
            #line 11 "..\..\Areas\Sph\Views\SphAccount\ForgotPassword.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral(" - SPH</title>\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 225), Tuple.Create("\"", 245)
, Tuple.Create(Tuple.Create("", 232), Tuple.Create<System.Object, System.Int32>(Href("~/favicon.ico")
, 232), false)
);

WriteLiteral(" rel=\"shortcut icon\"");

WriteLiteral(" type=\"image/x-icon\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width\"");

WriteLiteral(" />\r\n    <link");

WriteLiteral(" href=\"/Content/external/bootstrap.min.css\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 433), Tuple.Create("\"", 459)
, Tuple.Create(Tuple.Create("", 440), Tuple.Create<System.Object, System.Int32>(Href("~/Content/__css.css")
, 440), false)
);

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n\r\n</head>\r\n<body>\r\n    <div");

WriteLiteral(" id=\"body\"");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n\r\n        <section>\r\n\r\n\r\n\r\n            <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 26 "..\..\Areas\Sph\Views\SphAccount\ForgotPassword.cshtml"
           Write(Html.Partial("_Slider"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                <div");

WriteLiteral(" class=\"col-lg-4 col-lg-offset-2 login-form\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"logo\"");

WriteLiteral(">\r\n                        <img");

WriteLiteral(" src=\"/Images/logo_sph.png\"");

WriteLiteral(" alt=\"logo\"");

WriteLiteral(" />\r\n                    </div>\r\n                    <h2>Forgot Password</h2>\r\n\r\n" +
"                    <form");

WriteLiteral(" method=\"POST\"");

WriteLiteral(" action=\"\"");

WriteLiteral(" accept-charset=\"UTF-8\"");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                            <label");

WriteLiteral(" for=\"email\"");

WriteLiteral(" class=\"col-sm-2 control-label\"");

WriteLiteral(">Email</label>\r\n                            <div");

WriteLiteral(" class=\"col-sm-9\"");

WriteLiteral(">\r\n                                <input");

WriteLiteral(" type=\"email\"");

WriteLiteral(" data-bind=\"value: email\"");

WriteLiteral("\r\n                                       required");

WriteLiteral("\r\n                                       placeholder=\"Your email address\"");

WriteLiteral("\r\n                                       class=\"form-control\"");

WriteLiteral(" id=\"email\"");

WriteLiteral(">\r\n                            </div>\r\n                        </div>\r\n          " +
"              <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                            <label");

WriteLiteral(" class=\"col-sm-2 control-label sr-only\"");

WriteLiteral(">Submit</label>\r\n                            <div");

WriteLiteral(" class=\"col-sm-9\"");

WriteLiteral(">\r\n                                <button");

WriteLiteral(" id=\"submit\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" name=\"submit\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(">Send me a reset password link</button>\r\n                                <i");

WriteLiteral(" id=\"progress\"");

WriteLiteral(" class=\"fa fa-spin fa-spinner\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral("></i>\r\n                            </div>\r\n                        </div>\r\n      " +
"              </form>\r\n\r\n\r\n                </div>\r\n\r\n            </div>\r\n\r\n\r\n   " +
"         <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                <hr");

WriteLiteral(" class=\"col-lg-12\"");

WriteLiteral(" />\r\n                <h3>Learn to create your own Login page</h3>\r\n              " +
"  <p>Reactive Developer allows you to create your own branded Login</p>\r\n       " +
"         <a");

WriteLiteral(" href=\"/docs/#custom-login-page\"");

WriteLiteral(">Go to help</a>\r\n            </div>\r\n");

WriteLiteral("            ");

            
            #line 64 "..\..\Areas\Sph\Views\SphAccount\ForgotPassword.cshtml"
       Write(Html.Partial("_Footer"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n        </section>\r\n    </div>\r\n\r\n\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 2521), Tuple.Create("\"", 2552)
, Tuple.Create(Tuple.Create("", 2527), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/__vendor.min.js")
, 2527), false)
);

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(function () {\r\n            $(\'.carousel\').carousel({\r\n              " +
"  interval: 2000\r\n            });\r\n            var retUrl = getParameterByName(\"" +
"ReturnUrl\") + window.location.hash;\r\n            $(\'input[name=ReturnUrl]\').val(" +
"retUrl);\r\n\r\n            function getParameterByName(name) {\r\n                nam" +
"e = name.replace(/[\\[]/, \"\\\\\\[\").replace(/[\\]]/, \"\\\\\\]\");\r\n                var r" +
"egex = new RegExp(\"[\\\\?&]\" + name + \"=([^&#]*)\"),\r\n                    results =" +
" regex.exec(location.search);\r\n                return results == null ? \"\" : dec" +
"odeURIComponent(results[1].replace(/\\+/g, \" \"));\r\n            }\r\n\r\n            $" +
"(\'#submit\').click(function (e) {\r\n                var button = $(this),\r\n       " +
"             email = $(\'#email\').val();\r\n                if (!email) {\r\n        " +
"            return;\r\n                }\r\n                button.prop(\"disabled\", " +
"true);\r\n                $(\"#progress\").show();\r\n                e.preventDefault" +
"();\r\n                $.ajax({\r\n                    type: \"POST\",\r\n              " +
"      data: JSON.stringify({ email: email }),\r\n                    url: \"/SphAcc" +
"ount/ForgotPassword\",\r\n                    contentType: \"application/json; chars" +
"et=utf-8\",\r\n                    dataType: \"json\",\r\n                    error: fu" +
"nction (a, b, c) {\r\n                        console.log(c);\r\n                   " +
" },\r\n                    success: function () {\r\n                        button." +
"prop(\"disabled\", false);\r\n                        $(\"#progress\").hide();\r\n      " +
"                  window.location = \"/\";\r\n                    }\r\n               " +
" });\r\n            });\r\n        });\r\n    </script>\r\n</body>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
