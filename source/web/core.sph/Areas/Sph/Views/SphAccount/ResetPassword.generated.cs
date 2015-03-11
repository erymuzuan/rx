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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/SphAccount/ResetPassword.cshtml")]
    public partial class _Areas_Sph_Views_SphAccount_ResetPassword_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.Areas.Sph.Controllers.ResetPaswordModel>
    {
        public _Areas_Sph_Views_SphAccount_ResetPassword_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\Sph\Views\SphAccount\ResetPassword.cshtml"
  
    ViewBag.Title = "Reset password";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h2>Reset Password</h2>\r\n");

            
            #line 9 "..\..\Areas\Sph\Views\SphAccount\ResetPassword.cshtml"
 if (Model.IsValid)
{


            
            #line default
            #line hidden
WriteLiteral("    <form");

WriteLiteral(" action=\"/\"");

WriteLiteral(" method=\"post\"");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"password\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Password</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"password\"");

WriteLiteral(" data-bind=\"value: password\"");

WriteLiteral("\r\n                       required");

WriteLiteral("\r\n                       placeholder=\"New Password\"");

WriteLiteral("\r\n                       class=\"form-control\"");

WriteLiteral(" id=\"password\"");

WriteLiteral(">\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"confirm\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Confirm</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" type=\"password\"");

WriteLiteral(" data-bind=\"value: confirm\"");

WriteLiteral("\r\n                       required");

WriteLiteral(" maxlength=\"25\"");

WriteLiteral("\r\n                       placeholder=\"Confirm Password\"");

WriteLiteral("\r\n                       class=\"form-control\"");

WriteLiteral(" id=\"confirm\"");

WriteLiteral(">\r\n            </div>\r\n\r\n\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"confirm\"");

WriteLiteral(" class=\"col-lg-2 control-label sr-only\"");

WriteLiteral(">Submit Password Reset</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" id=\"submit\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(">Submit</button>\r\n            </div>\r\n\r\n\r\n        </div>\r\n\r\n        <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
            $(function () {

                $('#submit').click(function (e) {
                    e.preventDefault();
                    var button = $(this),
                        password = $(""#password"").val(),
                        confirm = $(""#confirm"").val();
                    e.preventDefault(e);
                    var data = {
                        Email: """);

            
            #line 53 "..\..\Areas\Sph\Views\SphAccount\ResetPassword.cshtml"
                           Write(Model.Email);

            
            #line default
            #line hidden
WriteLiteral(@""",
                        Password: password,
                        ConfirmPassword: confirm
                    };

                    if (!password || (password !== confirm)) {
                        $(""#message"").show();
                        return;
                    }

                    button.prop(""disabled"", true);
                    $.ajax({
                        type: ""POST"",
                        data: JSON.stringify(data),
                        url: ""/SphAccount/ResetPassword"",
                        contentType: ""application/json; charset=utf-8"",
                        dataType: ""json"",
                        error: function (a, b, c) {
                            console.log(c);
                        },
                        success: function () {
                            button.prop(""disabled"", false);
                            window.location = """);

            
            #line 75 "..\..\Areas\Sph\Views\SphAccount\ResetPassword.cshtml"
                                          Write(Url.Action("Login"));

            
            #line default
            #line hidden
WriteLiteral("\";\r\n                        }\r\n                    });\r\n                });\r\n    " +
"        });\r\n        </script>\r\n    </form>\r\n");

            
            #line 82 "..\..\Areas\Sph\Views\SphAccount\ResetPassword.cshtml"
}
else
{
            
            #line default
            #line hidden
WriteLiteral("<div");

WriteLiteral(" class=\"alert alert-danger\"");

WriteLiteral(" role=\"alert\"");

WriteLiteral(">\r\n        <strong>Invalid Link!</strong> ");

            
            #line 85 "..\..\Areas\Sph\Views\SphAccount\ResetPassword.cshtml"
                                  Write(Model.Mesage);

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n");

            
            #line 87 "..\..\Areas\Sph\Views\SphAccount\ResetPassword.cshtml"

}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591