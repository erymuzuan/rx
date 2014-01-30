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

namespace Bespoke.Sph.Web.Areas.App.Views.EntityFormRenderer
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
    
    #line 1 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    using Bespoke.Sph.Web;
    
    #line 3 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
    using Humanizer;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityFormRenderer/Script.cshtml")]
    public partial class Script : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.FormRendererViewModel>
    {
        public Script()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 6 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
  
    ViewBag.Title = "title";
    Layout = null;
    var ns = ConfigurationManager.ApplicationName.ToCamelCase() + "_" + this.Model.EntityDefinition.EntityDefinitionId;
    var typeCtor = string.Format("bespoke.{0}.domain.{1}({{WebId:system.guid()}})", ns, Model.EntityDefinition.Name);
    var typeName = string.Format("bespoke.{0}.domain.{1}", ns, Model.EntityDefinition.Name);
    var saveUrl = string.Format("/{0}/Save", @Model.EntityDefinition.Name);
    var validateUrl = string.Format("/Sph/BusinessRule/Validate/{0}?rules={1}", @Model.EntityDefinition.Name, string.Join(";", Model.Form.Rules.Select(r => r.Dehumanize())));
    var codeNamespace = ConfigurationManager.ApplicationName + "_" + Model.EntityDefinition.EntityDefinitionId;
    var commands = Model.Form.FormDesign.FormElementCollection.OfType<Button>()
        .Where(b => b.IsToolbarItem)
        .Select(b => string.Format("{{ caption :\"{0}\", command : {1}, icon:\"{2}\" }}", b.Label, b.CommandName, b.IconClass));
    var commandsJs = string.Format("[{0}]", string.Join(",", commands));


    var formId = @Model.Form.Route + "-form";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h2>title</h2>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteAttribute("src", Tuple.Create(" src=\"", 1307), Tuple.Create("\"", 1346)
, Tuple.Create(Tuple.Create("", 1313), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout-3.0.0.debug.js")
, 1313), false)
);

WriteLiteral("></script>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" data-script=\"true\"");

WriteLiteral(@">
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.validation, objectbuilders.eximp, objectbuilders.dialog],
        function (context, logger, router, system, validation, eximp, dialog) {

            var entity = ko.observable(new ");

            
            #line 30 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                      Write(Html.Raw(typeCtor));

            
            #line default
            #line hidden
WriteLiteral("),\r\n                form = ko.observable(new bespoke.sph.domain.EntityForm()),\r\n " +
"               activate = function (id) {\r\n\r\n                    var query = Str" +
"ing.format(\"");

            
            #line 34 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                           Write(Model.EntityDefinition.Name + "Id");

            
            #line default
            #line hidden
WriteLiteral(" eq {0}\", id),\r\n                        tcs = new $.Deferred(),\r\n                " +
"        itemTask = context.loadOneAsync(\"Customer\", query),\r\n                   " +
"     formTask = context.loadOneAsync(\"EntityForm\", \"Route eq \'");

            
            #line 37 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                                                            Write(Model.Form.Route);

            
            #line default
            #line hidden
WriteLiteral("\'\");\r\n\r\n                            $.when(itemTask, formTask).done(function(b,f)" +
" {  \r\n                                if (b) {\r\n                                " +
"    var item = context.toObservable(b, /Bespoke\\.");

            
            #line 41 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                                                            Write(codeNamespace);

            
            #line default
            #line hidden
WriteLiteral("\\.Domain\\.(.*?),/);\r\n                                    entity(item);\r\n         " +
"                       } \r\n                                else {\r\n             " +
"                       entity(new ");

            
            #line 45 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                          Write(Html.Raw(typeCtor));

            
            #line default
            #line hidden
WriteLiteral(@");
                                }
                                form(f);
                                
                                tcs.resolve(true);
                            });

                        return tcs.promise();
                },
                attached = function (view) {
                    // validation
                    validation.init($('#");

            
            #line 56 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                   Write(formId);

            
            #line default
            #line hidden
WriteLiteral("\'), form());\r\n                    \r\n                },\r\n\r\n");

            
            #line 60 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                
            
            #line default
            #line hidden
            
            #line 60 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var rule in Model.Form.Rules)
                {
                    var function = rule.Dehumanize();

            
            #line default
            #line hidden
WriteLiteral("                  ");

            
            #line 63 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                   Write(function);

            
            #line default
            #line hidden
WriteLiteral(" = function(){\r\n\r\n                    var tcs = new $.Deferred(),\r\n              " +
"          data = ko.mapping.toJSON(entity);\r\n\r\n                    context.post(" +
"data, \"/Sph/BusinessRule/Validate/");

            
            #line 68 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                                              Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("?rules=");

            
            #line 68 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                                                                                 Write(function);

            
            #line default
            #line hidden
WriteLiteral("\" )\r\n                        .then(function (result) {\r\n                         " +
"   tcs.resolve(result);\r\n                        });\r\n                    return" +
" tcs.promise();\r\n                },");

WriteLiteral("\r\n");

            
            #line 74 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                ");

            
            #line 75 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var btn in Model.Form.FormDesign.FormElementCollection.OfType<Button>())
                {
                    var function = btn.CommandName;

            
            #line default
            #line hidden
WriteLiteral("                  ");

            
            #line 78 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                   Write(function);

            
            #line default
            #line hidden
WriteLiteral(" = function(){\r\n");

WriteLiteral("                    ");

            
            #line 79 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
               Write(Html.Raw(btn.Command));

            
            #line default
            #line hidden
WriteLiteral("\r\n                },");

WriteLiteral("\r\n");

            
            #line 81 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral(@"                save = function() {
                    if (!validation.valid()) {
                        return Task.fromResult(false);
                    }

                    var tcs = new $.Deferred(),
                        data = ko.mapping.toJSON(entity);

");

            
            #line 90 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 90 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.Rules.Any())
                    {


            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n\r\n                    context.post(data, \"");

            
            #line 95 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                   Write(validateUrl);

            
            #line default
            #line hidden
WriteLiteral("\")\r\n                        .then(function(result) {\r\n                           " +
" if(result.success){\r\n                                context.post(data, \"");

            
            #line 98 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                               Write(saveUrl);

            
            #line default
            #line hidden
WriteLiteral(@""")
                                   .then(function(result) {
                                       tcs.resolve(result);
                                   });
                            }else{
                                tcs.resolve(result);
                            }
                        });
                    ");

WriteLiteral("\r\n");

            
            #line 107 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n\r\n                    context.post(data, \"");

            
            #line 112 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                   Write(saveUrl);

            
            #line default
            #line hidden
WriteLiteral("\")\r\n                        .then(function(result) {\r\n                           " +
" tcs.resolve(result);\r\n                        });\r\n                    ");

WriteLiteral("\r\n");

            
            #line 117 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("\r\n                    return tcs.promise();\r\n                };\r\n\r\n            re" +
"turn {\r\n");

            
            #line 123 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                
            
            #line default
            #line hidden
            
            #line 123 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var rule in Model.Form.Rules)
                    {
                        var function = rule.Dehumanize();

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 126 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                     Write(function);

            
            #line default
            #line hidden
WriteLiteral(" : ");

            
            #line 126 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                 Write(function);

            
            #line default
            #line hidden
WriteLiteral(",");

WriteLiteral("\r\n");

            
            #line 127 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                activate: activate,\r\n                attached: attached,\r\n       " +
"         entity: entity,\r\n                save : save,\r\n                toolbar " +
": { \r\n");

            
            #line 133 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 133 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsEmailAvailable)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("emailCommand : function(){\r\n                        console.log(\"Sending email\");" +
"\r\n                        return Task.fromResult(true);\r\n                    },");

WriteLiteral("\r\n");

            
            #line 139 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 140 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsPrintAvailable)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("emailCommand :{},");

WriteLiteral("\r\n");

            
            #line 143 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    saveCommand : save,\r\n                    commands : ko.observ" +
"ableArray(");

            
            #line 145 "..\..\Areas\App\Views\EntityFormRenderer\Script.cshtml"
                                             Write(Html.Raw(commandsJs));

            
            #line default
            #line hidden
WriteLiteral(")\r\n                }\r\n            };\r\n        });\r\n</script>");

        }
    }
}
#pragma warning restore 1591
