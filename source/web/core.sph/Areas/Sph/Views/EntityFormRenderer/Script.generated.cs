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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
    using Humanizer;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/EntityFormRenderer/Script.cshtml")]
    public partial class _Areas_Sph_Views_EntityFormRenderer_Script_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.FormRendererViewModel>
    {
        public _Areas_Sph_Views_EntityFormRenderer_Script_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n\r\n");

            
            #line 6 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
  
    ViewBag.Title = "title";
    Layout = null;
    var ns = ConfigurationManager.ApplicationName + "_" + this.Model.EntityDefinition.Id;
    var typeCtor = string.Format("bespoke.{0}.domain.{1}({{WebId:system.guid()}})", ns, Model.EntityDefinition.Name);
    var typeName = string.Format("bespoke.{0}.domain.{1}", ns, Model.EntityDefinition.Name);
    var saveUrl = string.Format("/{0}/Save", @Model.EntityDefinition.Name);
    var validateUrl = string.Format("/Sph/BusinessRule/Validate?{0};{1}", @Model.EntityDefinition.Name, string.Join(";", Model.Form.Rules.Select(r => r.Dehumanize())));
    var codeNamespace = ConfigurationManager.ApplicationName + "_" + Model.EntityDefinition.Id;
    var buttonOperations = Model.Form.FormDesign.FormElementCollection.OfType<Button>()
        .Where(b => b.IsToolbarItem)
        .Where(b => !string.IsNullOrWhiteSpace(b.Operation))
        .Select(b => string.Format("{{ caption :\"{0}\", command : {1}, icon:\"{2}\" }}", b.Label, b.Operation.ToCamelCase(), b.IconClass));

    var commands = Model.Form.FormDesign.FormElementCollection.OfType<Button>()
        .Where(b => b.IsToolbarItem)
        .Where(b => !string.IsNullOrWhiteSpace(b.CommandName))
        .Select(b => string.Format("{{ caption :\"{0}\", command : {1}, icon:\"{2}\" }}", b.Label, b.CommandName, b.IconClass));
    var commandsJs = string.Format("[{0}]", string.Join(",", commands.Concat(buttonOperations)));


    var formId = @Model.Form.Route + "-form";
    var saveOperation = Model.Form.Operation;
    var partialPath = string.IsNullOrWhiteSpace(Model.Form.Partial) ? string.Empty: ",'" + Model.Form.Partial + "'" ;
    var partialVariable = string.IsNullOrWhiteSpace(Model.Form.Partial) ? string.Empty : ",partial" ;

            
            #line default
            #line hidden
WriteLiteral(":\r\n\r\n<h2>title</h2>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteAttribute("src", Tuple.Create(" src=\"", 1925), Tuple.Create("\"", 1964)
, Tuple.Create(Tuple.Create("", 1931), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout-3.2.0.debug.js")
, 1931), false)
);

WriteLiteral("></script>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" data-script=\"true\"");

WriteLiteral(@">
    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router,
        objectbuilders.system, objectbuilders.validation, objectbuilders.eximp,
        objectbuilders.dialog, objectbuilders.watcher, objectbuilders.config,
        objectbuilders.app ");

            
            #line 39 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                      Write(Html.Raw(partialPath));

            
            #line default
            #line hidden
WriteLiteral("],\r\n        function (context, logger, router, system, validation, eximp, dialog," +
" watcher,config,app\r\n");

WriteLiteral("            ");

            
            #line 41 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
       Write(partialVariable);

            
            #line default
            #line hidden
WriteLiteral(") {\r\n\r\n            var entity = ko.observable(new ");

            
            #line 43 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                      Write(Html.Raw(typeCtor));

            
            #line default
            #line hidden
WriteLiteral(@"),
                errors = ko.observableArray(),
                form = ko.observable(new bespoke.sph.domain.EntityForm()),
                watching = ko.observable(false),
                id = ko.observable(),
                i18n = null,
                activate = function (entityId) {
                    id(entityId);

                    var query = String.format(""Id eq '{0}'"", entityId),
                        tcs = new $.Deferred(),
                        itemTask = context.loadOneAsync(""");

            
            #line 54 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                    Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\", query),\r\n                        formTask = context.loadOneAsync(\"EntityForm\"," +
" \"Route eq \'");

            
            #line 55 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                            Write(Model.Form.Route);

            
            #line default
            #line hidden
WriteLiteral("\'\"),\r\n                        watcherTask = watcher.getIsWatchingAsync(\"");

            
            #line 56 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                             Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\", entityId),\r\n                        i18nTask = $.getJSON(\"i18n/\" + config.lang" +
" + \"/");

            
            #line 57 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                  Write(Model.Form.Route);

            
            #line default
            #line hidden
WriteLiteral(@""");

                    $.when(itemTask, formTask, watcherTask, i18nTask).done(function(b,f,w,n) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new ");

            
            #line 65 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                  Write(Html.Raw(typeCtor));

            
            #line default
            #line hidden
WriteLiteral(");\r\n                        }\r\n                        form(f);\r\n                " +
"        watching(w);\r\n                        i18n = n[0];\r\n");

            
            #line 70 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 70 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                         if (!string.IsNullOrWhiteSpace(Model.Form.Partial))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            ");

WriteLiteral(@"
                            if(typeof partial.activate === ""function""){
                                var pt = partial.activate(entity());
                                if(typeof pt.done === ""function""){
                                    pt.done(tcs.resolve);
                                }else{
                                    tcs.resolve(true);
                                }
                            }
                            ");

WriteLiteral("\r\n");

            
            #line 82 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                        }
                        else
                        {

            
            #line default
            #line hidden
WriteLiteral("                            ");

WriteLiteral("tcs.resolve(true);\r\n");

            
            #line 86 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        \r\n                    });\r\n\r\n                    return t" +
"cs.promise();\r\n                },\r\n");

            
            #line 92 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 
            
            #line default
            #line hidden
            
            #line 92 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                  foreach (var operation in Model.EntityDefinition.EntityOperationCollection)
            {
                var opFunc = operation.Name.ToCamelCase();

            
            #line default
            #line hidden
WriteLiteral("                ");

            
            #line 95 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 Write(opFunc);

            
            #line default
            #line hidden
WriteLiteral(@" = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var data = ko.mapping.toJSON(entity);

                    return  context.post(data, ""/");

            
            #line 103 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                            Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("/");

            
            #line 103 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                         Write(operation.Name);

            
            #line default
            #line hidden
WriteLiteral(@""" )
                         .then(function (result) {
                             if (result.success) {
                                 logger.info(result.message);
                                 entity().Id(result.id);
                                 errors.removeAll();

");

WriteLiteral("                                 ");

            
            #line 110 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                             Write(Html.Raw(operation.GetConfirmationMessage()));

            
            #line default
            #line hidden
WriteLiteral(@"
                             } else {
                                 errors.removeAll();
                                 _(result.rules).each(function(v){
                                     errors(v.ValidationErrors);
                                 });
                                 logger.error(""There are errors in your entity, !!!"");
                             }
                         });
                 },");

WriteLiteral("\r\n");

            
            #line 120 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("                attached = function (view) {\r\n                    // validation\r\n" +
"                    validation.init($(\'#");

            
            #line 123 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                   Write(formId);

            
            #line default
            #line hidden
WriteLiteral("\'), form());\r\n\r\n\r\n");

            
            #line 126 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 126 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (!string.IsNullOrWhiteSpace(Model.Form.Partial))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n                    if(typeof partial.attached === \"function\"){\r\n              " +
"          partial.attached(view);\r\n                    }\r\n\r\n                    " +
"");

WriteLiteral("\r\n");

            
            #line 134 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral(@"
                },
                compositionComplete = function() {
                    $(""[data-i18n]"").each(function (i, v) {
                        var $label = $(v),
                            text = $label.data(""i18n"");
                        if (typeof i18n[text] === ""string"") {
                            $label.text(i18n[text]);
                        }
                    });
                },

");

            
            #line 147 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                
            
            #line default
            #line hidden
            
            #line 147 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var rule in Model.Form.Rules)
                {
                    var function = rule.Dehumanize().ToCamelCase();

            
            #line default
            #line hidden
WriteLiteral("                  ");

            
            #line 150 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                   Write(function);

            
            #line default
            #line hidden
WriteLiteral(" = function(){\r\n\r\n                    var data = ko.mapping.toJSON(entity);\r\n\r\n  " +
"                 return context.post(data, \"/Sph/BusinessRule/Validate?");

            
            #line 154 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                    Write(function);

            
            #line default
            #line hidden
WriteLiteral("\" );\r\n                },");

WriteLiteral("\r\n");

            
            #line 156 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                ");

            
            #line 157 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var btn in Model.Form.FormDesign.FormElementCollection.OfType<Button>())
                {
                    if (string.IsNullOrWhiteSpace(btn.CommandName))
                    {
                        continue;
                    }
                    var function = btn.CommandName;

            
            #line default
            #line hidden
WriteLiteral("                  ");

            
            #line 164 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                   Write(function);

            
            #line default
            #line hidden
WriteLiteral(" = function(){\r\n");

WriteLiteral("                    ");

            
            #line 165 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
               Write(Html.Raw(btn.Command));

            
            #line default
            #line hidden
WriteLiteral("\r\n                },");

WriteLiteral("\r\n");

            
            #line 167 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                save = function() {\r\n                    if (!validation.valid())" +
" {\r\n                        return Task.fromResult(false);\r\n                    " +
"}\r\n\r\n                    var data = ko.mapping.toJSON(entity);\r\n\r\n");

            
            #line 175 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 175 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.Rules.Any())
                    {


            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n\r\n                    return context.post(data, \"");

            
            #line 180 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                          Write(validateUrl);

            
            #line default
            #line hidden
WriteLiteral("\")\r\n                        .then(function(result) {\r\n                           " +
" if(result.success){\r\n                                context.post(data, \"");

            
            #line 183 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                               Write(saveUrl);

            
            #line default
            #line hidden
WriteLiteral("\")\r\n                                   .then(function(result) {\r\n                " +
"                       entity().Id(result.id);\r\n                                " +
"       app.showMessage(\"Your ");

            
            #line 186 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                        Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(" has been successfully saved\", \"");

            
            #line 186 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                                                                    Write(ConfigurationManager.ApplicationFullName);

            
            #line default
            #line hidden
WriteLiteral(@""", [""OK""]);
                                   });
                            }else{
                                var ve = _(result.validationErrors).map(function(v){
                                    return {
                                        Message : v.message
                                    };
                                });
                                errors(ve);
                                logger.error(""There are errors in your entity, !!!"");
                            }
                        });
                    ");

WriteLiteral("\r\n");

            
            #line 199 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n\r\n                    return context.post(data, \"");

            
            #line 204 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                          Write(saveUrl);

            
            #line default
            #line hidden
WriteLiteral("\")\r\n                        .then(function(result) {\r\n                           " +
" entity().Id(result.id);\r\n                            app.showMessage(\"Your ");

            
            #line 207 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                             Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(" has been successfully saved\", \"");

            
            #line 207 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                                                         Write(ConfigurationManager.ApplicationFullName);

            
            #line default
            #line hidden
WriteLiteral("\", [\"OK\"]);\r\n\r\n                        });\r\n                    ");

WriteLiteral("\r\n");

            
            #line 211 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("\r\n                },\r\n                remove = function() {\r\n                    " +
"return $.ajax({\r\n                        type: \"DELETE\",\r\n                      " +
"  url: \"/");

            
            #line 217 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                           Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(@"/Remove/"" + entity().Id(),
                        contentType: ""application/json; charset=utf-8"",
                        dataType: ""json"",
                        success: function() {
                            app.showMessage(""Your item has been successfully removed"", ""Removed"", [""OK""])
                              .done(function () {
                                  window.location = ""#");

            
            #line 223 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                  Write(Model.EntityDefinition.Name.ToLowerInvariant());

            
            #line default
            #line hidden
WriteLiteral("\";\r\n                              });\r\n                        }\r\n               " +
"     });\r\n\r\n\r\n                };\r\n\r\n            var vm = {\r\n");

            
            #line 232 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                  
            
            #line default
            #line hidden
            
            #line 232 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                   if (!string.IsNullOrWhiteSpace(Model.Form.Partial))
                  {

            
            #line default
            #line hidden
WriteLiteral("                            ");

WriteLiteral("\r\n                            partial : partial,\r\n                            ");

WriteLiteral("\r\n");

            
            #line 237 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 238 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     foreach (var rule in Model.Form.Rules)
                    {
                        var function = rule.Dehumanize().ToCamelCase();

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 241 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     Write(function);

            
            #line default
            #line hidden
WriteLiteral(" : ");

            
            #line 241 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                 Write(function);

            
            #line default
            #line hidden
WriteLiteral(",");

WriteLiteral("\r\n");

            
            #line 242 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral(@"                activate: activate,
                config: config,
                attached: attached,
                compositionComplete:compositionComplete,
                entity: entity,
                errors: errors,
                save : save,
");

            
            #line 250 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                
            
            #line default
            #line hidden
            
            #line 250 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var op in Model.EntityDefinition.EntityOperationCollection)
                {
                        var function = op.Name.ToCamelCase();

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 253 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     Write(function);

            
            #line default
            #line hidden
WriteLiteral(" : ");

            
            #line 253 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                 Write(function);

            
            #line default
            #line hidden
WriteLiteral(",");

WriteLiteral("\r\n");

            
            #line 254 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"

                }

            
            #line default
            #line hidden
WriteLiteral("                //\r\n\r\n");

            
            #line 258 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                
            
            #line default
            #line hidden
            
            #line 258 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var btn in Model.Form.FormDesign.FormElementCollection.OfType<Button>().Where(b => !string.IsNullOrWhiteSpace(b.CommandName)))
                {

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 260 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                      Write(btn.CommandName);

            
            #line default
            #line hidden
WriteLiteral(" : ");

            
            #line 260 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                           Write(btn.CommandName);

            
            #line default
            #line hidden
WriteLiteral(" ,\r\n                ");

WriteLiteral("\r\n");

            
            #line 262 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("\r\n                toolbar : {\r\n");

            
            #line 265 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 265 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsEmailAvailable)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("emailCommand : {\r\n                        entity : \"");

            
            #line 268 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                             Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\",\r\n                        id :id\r\n                    },");

WriteLiteral("\r\n");

            
            #line 271 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 272 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsPrintAvailable)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("printCommand :{\r\n                        entity : \'");

            
            #line 275 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                             Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                        id : id\r\n                    },");

WriteLiteral("\r\n");

            
            #line 278 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 279 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsRemoveAvailable)
            {

            
            #line default
            #line hidden
WriteLiteral("                ");

WriteLiteral("removeCommand :remove,\r\n                    canExecuteRemoveCommand : ko.computed" +
"(function(){\r\n                        return entity().Id();\r\n                   " +
" }),");

WriteLiteral("\r\n");

            
            #line 285 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 286 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsWatchAvailable)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n                    watchCommand: function() {\r\n                        return " +
"watcher.watch(\"");

            
            #line 290 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                         Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(@""", entity().Id())
                            .done(function(){
                                watching(true);
                            });
                    },
                    unwatchCommand: function() {
                        return watcher.unwatch(""");

            
            #line 296 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                           Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\", entity().Id())\r\n                            .done(function(){\r\n               " +
"                 watching(false);\r\n                            });\r\n            " +
"        },\r\n                    watching: watching,");

WriteLiteral("\r\n");

            
            #line 302 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 303 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (!string.IsNullOrWhiteSpace(@saveOperation))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n                    saveCommand : ");

            
            #line 306 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                             Write(saveOperation.ToCamelCase());

            
            #line default
            #line hidden
WriteLiteral(",\r\n                    ");

WriteLiteral("\r\n");

            
            #line 308 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    commands : ko.observableArray(");

            
            #line 309 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                             Write(Html.Raw(commandsJs));

            
            #line default
            #line hidden
WriteLiteral(")\r\n                }\r\n            };\r\n\r\n            return vm;\r\n        });\r\n</sc" +
"ript>");

        }
    }
}
#pragma warning restore 1591
