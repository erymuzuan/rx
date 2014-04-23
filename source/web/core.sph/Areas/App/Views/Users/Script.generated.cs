﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Areas.App.Views.Users
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
    
    #line 1 "..\..\Areas\App\Views\Users\Script.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Users/Script.cshtml")]
    public partial class Script : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.Areas.Sph.Controllers.AdminController>
    {
        public Script()
        {
        }
        public override void Execute()
        {



            
            #line 3 "..\..\Areas\App\Views\Users\Script.cshtml"
  
    ViewBag.Title = "UsersJs";
    Layout = "~/Views/Shared/_Layout.cshtml";

    var roles = Roles.GetAllRoles();



            
            #line default
            #line hidden
WriteLiteral(@"
<script type=""text/javascript"" data-script=""true"">
    define([objectbuilders.datacontext,'viewmodels/_users.designation','viewmodels/_users.department','services/logger'], function (context,designationvm,departmentvm,logger) {
        var isBusy = ko.observable(false),
            roles = ");


            
            #line 14 "..\..\Areas\App\Views\Users\Script.cshtml"
               Write(Html.Raw(JsonConvert.SerializeObject(roles)));

            
            #line default
            #line hidden
WriteLiteral(",\r\n            printprofile = ko.observable(new bespoke.sph.domain.Profile()),\r\n " +
"           profiles = ko.observableArray(),\r\n            activate = function() {" +
"\r\n                var query = String.format(\"UserProfileId gt 0\");\r\n            " +
"    var tcs = new $.Deferred();\r\n                loadDetails();\r\n               " +
" var profileTask = context.loadAsync(\"UserProfile\", query);\r\n                var" +
" designationTask = context.getListAsync(\"Designation\",\"DesignationId gt 0\", \"Nam" +
"e\");\r\n                var departmentTask = context.loadOneAsync(\"Setting\", \"Key " +
"eq \'Departments\'\");\r\n                $.when(designationTask,profileTask,departme" +
"ntTask)\r\n                 .then(function(s,p,d) {\r\n                     isBusy(f" +
"alse);\r\n                     if (s) {\r\n                         vm.designationOp" +
"tions(s);\r\n                     }\r\n                     if (d) {\r\n              " +
"           var departments = JSON.parse(ko.mapping.toJS(d.Value));;\r\n           " +
"              vm.departmentOptions(departments);\r\n                     }\r\n      " +
"               var list = _(p.itemCollection).map(map);\r\n                     vm" +
".profiles(list);\r\n\r\n                     tcs.resolve(true);\r\n                 })" +
";\r\n                return tcs.promise();\r\n            },\r\n            map = func" +
"tion(item) {\r\n                var profile = new bespoke.sph.domain.Profile();\r\n " +
"               profile.IsNew(false);\r\n                profile.FullName(item.Full" +
"Name());\r\n                profile.UserName(item.UserName());\r\n                pr" +
"ofile.Email(item.Email());\r\n                profile.Designation(item.Designation" +
"());\r\n                profile.Department(item.Department());\r\n                pr" +
"ofile.Telephone(item.Telephone());\r\n                return profile;\r\n           " +
" },\r\n            loadDetails = function() {\r\n                designationvm.activ" +
"ate(roles);\r\n                departmentvm.activate();\r\n            },\r\n         " +
"   add = function() {\r\n                vm.profile(new bespoke.sph.domain.Profile" +
"());\r\n                vm.profile().IsNew(true);\r\n                vm.profile().Us" +
"erName(\'\');\r\n                vm.profile().Email.subscribe(emailChanged);\r\n      " +
"          vm.profile().UserName.subscribe(userNameChaged);\r\n                \r\n  " +
"              $(\'#user-details-modal\').modal();\r\n            },\r\n            edi" +
"tedProfile = ko.observable(),\r\n            edit = function(user) {\r\n            " +
"    editedProfile(user);\r\n                var c1 = ko.mapping.fromJSON(ko.mappin" +
"g.toJSON(user));\r\n                var clone = c1;\r\n                vm.profile(cl" +
"one);\r\n            },\r\n            save = function() {\r\n                var tcs " +
"= new $.Deferred();\r\n                var data = ko.mapping.toJSON({profile : vm." +
"profile}) ;\r\n                isBusy(true);\r\n\r\n                context.post(data," +
" \"/Admin/AddUser\")\r\n                    .then(function(result) {\r\n              " +
"          isBusy(false);\r\n                        vm.profiles.push(result);\r\n   " +
"                     tcs.resolve(true);\r\n                    });\r\n              " +
"  return tcs.promise();\r\n            },\r\n            resetPassword = function(us" +
"er) {\r\n                vm.password1(\'\');\r\n                vm.password2(\'\');\r\n   " +
"             var c1 = ko.mapping.fromJSON(ko.mapping.toJSON(user));\r\n           " +
"     var clone = c1;\r\n                vm.profile(clone);\r\n            },\r\n      " +
"      savePassword = function() {\r\n                var tcs = new $.Deferred();\r\n" +
"                if (!vm.password1() && !vm.password2()) return tcs.promise();\r\n " +
"               if (vm.password1() != vm.password2()) {\r\n                    logg" +
"er.logError(\'Password mismatch\', this, this, true);\r\n                    return " +
"tcs.promise();\r\n                }\r\n                \r\n                var data = " +
"ko.mapping.toJSON({userName: vm.profile().UserName(), password: vm.password1}) ;" +
"\r\n                isBusy(true);\r\n\r\n                context.post(data, \"/Admin/Re" +
"setPassword\")\r\n                    .then(function(result) {\r\n                   " +
"     isBusy(false);\r\n                        if (result.OK) {\r\n                 " +
"           logger.info(result.messages);\r\n                        } else {\r\n    " +
"                        logger.logError(result.messages, this, this, true);\r\n   " +
"                     }\r\n                        tcs.resolve(true);\r\n            " +
"        });\r\n                return tcs.promise();\r\n                \r\n          " +
"      \r\n            }, \r\n            userNameChaged = function(userName) {\r\n    " +
"            vm.isBusyValidatingUserName(true);\r\n                var tcs = new $." +
"Deferred();\r\n                var data = JSON.stringify({userName : userName});\r\n" +
"                isBusy(true);\r\n                context.post(data, \"/Admin/Valida" +
"teUserName\")\r\n                    .then(function(result) {\r\n                    " +
"    isBusy(false);\r\n                        vm.isBusyValidatingUserName(false);\r" +
"\n                        if (result.status !== \"OK\") {\r\n                        " +
"    vm.userNameValidationStatus(result.message);\r\n                        }\r\n\r\n " +
"                       tcs.resolve(result);\r\n                    });\r\n          " +
"      return tcs.promise();\r\n            },\r\n            emailChanged = function" +
"(email) {\r\n                vm.isb(true);\r\n                var tcs = new $.Deferr" +
"ed();\r\n                var data = JSON.stringify({email : email});\r\n            " +
"    isBusy(true);\r\n                context.post(data, \"/Admin/ValidateEmail\")\r\n " +
"                   .then(function(result) {\r\n                        isBusy(fals" +
"e);\r\n                        vm.isBusyValidatingEmail(false);\r\n                 " +
"       if (result.status !== \"OK\") {\r\n                            vm.emailValida" +
"tionStatus(result.message);\r\n                        }\r\n\r\n                      " +
"  tcs.resolve(result);\r\n                    });\r\n                return tcs.prom" +
"ise();\r\n            };\r\n\r\n\r\n        var vm = {\r\n            activate: activate,\r" +
"\n            isBusyValidatingUserName : ko.observable(false),\r\n            isBus" +
"yValidatingEmail : ko.observable(false),\r\n            profiles: profiles,\r\n     " +
"       profile: ko.observable(new bespoke.sph.domain.Profile()),\r\n            pr" +
"intprofile : printprofile,\r\n            designationOptions : ko.observableArray(" +
"),\r\n            departmentOptions : ko.observableArray(),\r\n            userNameV" +
"alidationStatus : ko.observable(),\r\n            emailValidationStatus : ko.obser" +
"vable(),\r\n            editCommand: edit,\r\n            saveCommand: save,\r\n      " +
"      add: add,\r\n            password1: ko.observable(),\r\n            password2:" +
" ko.observable(),\r\n            resetPasswordCommand: resetPassword,\r\n           " +
" savePasswordCommand: savePassword,\r\n            map: map,\r\n            searchTe" +
"rm: {\r\n                department:ko.observable(),\r\n                keyword: ko." +
"observable()\r\n            },\r\n            toolbar : ko.observable({\r\n           " +
"     reloadCommand: function () {\r\n                    return activate();\r\n     " +
"           },\r\n                printCommand: ko.observable({\r\n                  " +
"  entity: ko.observable(\"UserProfile\"),\r\n                    id: ko.observable(0" +
"),\r\n                    item: printprofile,\r\n                })\r\n            })\r" +
"\n        };\r\n        \r\n        \r\n        return vm;\r\n        \r\n\r\n    });\r\n</scri" +
"pt>\r\n");


        }
    }
}
#pragma warning restore 1591
