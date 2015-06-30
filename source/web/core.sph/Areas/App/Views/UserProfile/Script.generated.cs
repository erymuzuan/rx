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
    
    #line 1 "..\..\Areas\App\Views\UserProfile\Script.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/UserProfile/Script.cshtml")]
    public partial class _Areas_App_Views_UserProfile_Script_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.UserProfileViewModel>
    {
        public _Areas_App_Views_UserProfile_Script_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\App\Views\UserProfile\Script.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" data-script=\"true\"");

WriteLiteral(">\r\n\r\n    define([objectbuilders.datacontext],\r\n        function (context) {\r\n    " +
"        var isBusy = ko.observable(false),\r\n            userName = ko.observable" +
"(\"");

            
            #line 11 "..\..\Areas\App\Views\UserProfile\Script.cshtml"
                                 Write(Model.User.UserName);

            
            #line default
            #line hidden
WriteLiteral("\"),\r\n            userProfile = ko.observable(new bespoke.sph.domain.UserProfile()" +
"),\r\n            startModule = ko.observable(\"");

            
            #line 13 "..\..\Areas\App\Views\UserProfile\Script.cshtml"
                                    Write(Model.Profile.StartModule);

            
            #line default
            #line hidden
WriteLiteral("\"),\r\n            startModuleOptions = ko.observableArray(");

            
            #line 14 "..\..\Areas\App\Views\UserProfile\Script.cshtml"
                                               Write(Html.Raw(JsonConvert.SerializeObject(Model.StartModuleOptions)));

            
            #line default
            #line hidden
WriteLiteral("),\r\n            languageOptions = ko.observableArray(),\r\n            activate = f" +
"unction () {\r\n                var query = String.format(\"UserName eq \'{0}\'\", use" +
"rName()),\r\n                    tcs = new $.Deferred(),\r\n                    load" +
"Task = context.loadOneAsync(\"UserProfile\", query),\r\n                    language" +
"OptionsTask = $.getJSON(\"/i18n/options\");\r\n                $.when(loadTask, lang" +
"uageOptionsTask).done(function (b, langs) {\r\n                    if (b)\r\n       " +
"                 userProfile(b);\r\n                    else\r\n                    " +
"    userProfile(new bespoke.sph.domain.UserProfile());\r\n                    var " +
"lang = langs[0],\r\n                        options = [];\r\n                    for" +
" (var code in lang) {\r\n                        if (lang.hasOwnProperty(code)) {\r" +
"\n                            options.push({ code: code, display: lang[code] });\r" +
"\n                        }\r\n                    }\r\n                    languageO" +
"ptions(options);\r\n                    tcs.resolve(true);\r\n                });\r\n\r" +
"\n                return tcs.promise();\r\n            },\r\n            saveAsync = " +
"function () {\r\n                var json = ko.toJSON(userProfile);\r\n             " +
"   return context.post(json, \"/App/UserProfile/UpdateUser\");\r\n            };\r\n\r\n" +
"            var vm = {\r\n                activate: activate,\r\n                use" +
"rProfile: userProfile,\r\n                startModule: startModule,\r\n             " +
"   startModuleOptions: startModuleOptions,\r\n                languageOptions: lan" +
"guageOptions,\r\n                toolbar: {\r\n                    saveCommand: save" +
"Async\r\n                },\r\n                isBusy: isBusy,\r\n                titl" +
"e: \"User profile Details\"\r\n            };\r\n\r\n            return vm;\r\n        });" +
"\r\n</script>\r\n");

        }
    }
}
#pragma warning restore 1591
