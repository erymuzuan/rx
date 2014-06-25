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

namespace Bespoke.Sph.Web.Areas.Sph.Views.EntityFormRenderer
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
    public partial class Script : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.FormRendererViewModel>
    {
        public Script()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n\r\n");

            
            #line 6 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
  
    ViewBag.Title = "title";
    Layout = null;
    var ns = ConfigurationManager.ApplicationName.ToCamelCase() + "_" + this.Model.EntityDefinition.EntityDefinitionId;
    var typeCtor = string.Format("bespoke.{0}.domain.{1}({{WebId:system.guid()}})", ns, Model.EntityDefinition.Name);
    var typeName = string.Format("bespoke.{0}.domain.{1}", ns, Model.EntityDefinition.Name);
    var saveUrl = string.Format("/{0}/Save", @Model.EntityDefinition.Name);
    var validateUrl = string.Format("/Sph/BusinessRule/Validate?{0};{1}", @Model.EntityDefinition.Name, string.Join(";", Model.Form.Rules.Select(r => r.Dehumanize())));
    var codeNamespace = ConfigurationManager.ApplicationName + "_" + Model.EntityDefinition.EntityDefinitionId;
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

WriteAttribute("src", Tuple.Create(" src=\"", 1971), Tuple.Create("\"", 2010)
, Tuple.Create(Tuple.Create("", 1977), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout-3.1.0.debug.js")
, 1977), false)
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
                activate = function (entityId) {
                    id(parseInt(entityId));

                    var query = String.format(""");

            
            #line 51 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                           Write(Model.EntityDefinition.Name + "Id");

            
            #line default
            #line hidden
WriteLiteral(" eq {0}\", entityId),\r\n                        tcs = new $.Deferred(),\r\n          " +
"              itemTask = context.loadOneAsync(\"");

            
            #line 53 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                    Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\", query),\r\n                        formTask = context.loadOneAsync(\"EntityForm\"," +
" \"Route eq \'");

            
            #line 54 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                            Write(Model.Form.Route);

            
            #line default
            #line hidden
WriteLiteral("\'\"),\r\n                        watcherTask = watcher.getIsWatchingAsync(\"");

            
            #line 55 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                             Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(@""", entityId);

                    $.when(itemTask, formTask, watcherTask).done(function(b,f,w) {
                        if (b) {
                            var item = context.toObservable(b);
                            entity(item);
                        }
                        else {
                            entity(new ");

            
            #line 63 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                  Write(Html.Raw(typeCtor));

            
            #line default
            #line hidden
WriteLiteral(");\r\n                        }\r\n                        form(f);\r\n                " +
"        watching(w);\r\n");

            
            #line 67 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 67 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
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

            
            #line 79 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                        }
                        else
                        {

            
            #line default
            #line hidden
WriteLiteral("                            ");

WriteLiteral("tcs.resolve(true);\r\n");

            
            #line 83 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        \r\n                    });\r\n\r\n\r\n\r\n                    retu" +
"rn tcs.promise();\r\n                },\r\n");

            
            #line 91 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 
            
            #line default
            #line hidden
            
            #line 91 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                  foreach (var operation in Model.EntityDefinition.EntityOperationCollection)
            {
                var opFunc = operation.Name.ToCamelCase();

            
            #line default
            #line hidden
WriteLiteral("                ");

            
            #line 94 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 Write(opFunc);

            
            #line default
            #line hidden
WriteLiteral(@" = function(){

                     if (!validation.valid()) {
                         return Task.fromResult(false);
                     }

                     var tcs = new $.Deferred(),
                         data = ko.mapping.toJSON(entity);

                     context.post(data, ""/");

            
            #line 103 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                     Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("/");

            
            #line 103 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                  Write(operation.Name);

            
            #line default
            #line hidden
WriteLiteral("\" )\r\n                         .then(function (result) {\r\n                        " +
"     if (result.success) {\r\n                                 logger.info(result." +
"message);\r\n                                 entity().");

            
            #line 107 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                      Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("Id(result.id);\r\n                                 errors.removeAll();\r\n\r\n");

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
                             tcs.resolve(result);
                         });
                     return tcs.promise();
                 },");

WriteLiteral("\r\n");

            
            #line 122 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("                attached = function (view) {\r\n                    // validation\r\n" +
"                    validation.init($(\'#");

            
            #line 125 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                   Write(formId);

            
            #line default
            #line hidden
WriteLiteral("\'), form());\r\n\r\n\r\n");

            
            #line 128 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 128 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (!string.IsNullOrWhiteSpace(Model.Form.Partial))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n                    if(typeof partial.attached === \"function\"){\r\n              " +
"          partial.attached(view);\r\n                    }\r\n\r\n                    " +
"");

WriteLiteral("\r\n");

            
            #line 136 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("\r\n                },\r\n\r\n");

            
            #line 140 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                
            
            #line default
            #line hidden
            
            #line 140 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var rule in Model.Form.Rules)
                {
                    var function = rule.Dehumanize().ToCamelCase();

            
            #line default
            #line hidden
WriteLiteral("                  ");

            
            #line 143 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                   Write(function);

            
            #line default
            #line hidden
WriteLiteral(" = function(){\r\n\r\n                    var tcs = new $.Deferred(),\r\n              " +
"          data = ko.mapping.toJSON(entity);\r\n\r\n                    context.post(" +
"data, \"/Sph/BusinessRule/Validate?");

            
            #line 148 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                              Write(function);

            
            #line default
            #line hidden
WriteLiteral("\" )\r\n                        .then(function (result) {\r\n                         " +
"   tcs.resolve(result);\r\n                        });\r\n                    return" +
" tcs.promise();\r\n                },");

WriteLiteral("\r\n");

            
            #line 154 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                ");

            
            #line 155 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
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

            
            #line 162 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                   Write(function);

            
            #line default
            #line hidden
WriteLiteral(" = function(){\r\n");

WriteLiteral("                    ");

            
            #line 163 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
               Write(Html.Raw(btn.Command));

            
            #line default
            #line hidden
WriteLiteral("\r\n                },");

WriteLiteral("\r\n");

            
            #line 165 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
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

            
            #line 174 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 174 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.Rules.Any())
                    {


            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n\r\n                    context.post(data, \"");

            
            #line 179 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                   Write(validateUrl);

            
            #line default
            #line hidden
WriteLiteral("\")\r\n                        .then(function(result) {\r\n                           " +
" if(result.success){\r\n                                context.post(data, \"");

            
            #line 182 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                               Write(saveUrl);

            
            #line default
            #line hidden
WriteLiteral("\")\r\n                                   .then(function(result) {\r\n                " +
"                       tcs.resolve(result);\r\n                                   " +
"    entity().");

            
            #line 185 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                            Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("Id(result.id);\r\n                                       app.showMessage(\"Your ");

            
            #line 186 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                        Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(" has been successfully saved\", \"");

            
            #line 186 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                                                                    Write(ConfigurationManager.ApplicationFullName);

            
            #line default
            #line hidden
WriteLiteral(@""", [""ok""]);
                                   });
                            }else{
                                var ve = _(result.validationErrors).map(function(v){
                                    return {
                                        Message : v.message
                                    };
                                });
                                errors(ve);
                                logger.error(""There are errors in your entity, !!!"");
                                tcs.resolve(result);
                            }
                        });
                    ");

WriteLiteral("\r\n");

            
            #line 200 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n\r\n                    context.post(data, \"");

            
            #line 205 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                   Write(saveUrl);

            
            #line default
            #line hidden
WriteLiteral("\")\r\n                        .then(function(result) {\r\n                           " +
" tcs.resolve(result);\r\n                            entity().");

            
            #line 208 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                 Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("Id(result.id);\r\n                            app.showMessage(\"Your ");

            
            #line 209 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                             Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(" has been successfully saved\", \"");

            
            #line 209 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                                                         Write(ConfigurationManager.ApplicationFullName);

            
            #line default
            #line hidden
WriteLiteral("\", [\"ok\"]);\r\n\r\n                        });\r\n                    ");

WriteLiteral("\r\n");

            
            #line 213 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("\r\n                    return tcs.promise();\r\n                },\r\n                " +
"remove = function() {\r\n                    var tcs = new $.Deferred();\r\n        " +
"            $.ajax({\r\n                        type: \"DELETE\",\r\n                 " +
"       url: \"/");

            
            #line 221 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                           Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("/Remove/\" + entity().");

            
            #line 221 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                              Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(@"Id(),
                        contentType: ""application/json; charset=utf-8"",
                        dataType: ""json"",
                        error: tcs.reject,
                        success: function() {
                            tcs.resolve(true);
                            app.showMessage(""Your item has been successfully removed"", ""Removed"", [""OK""])
                              .done(function () {
                                  window.location = ""#");

            
            #line 229 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                  Write(Model.EntityDefinition.Name.ToLowerInvariant());

            
            #line default
            #line hidden
WriteLiteral("\";\r\n                              });\r\n                        }\r\n               " +
"     });\r\n\r\n\r\n                    return tcs.promise();\r\n                };\r\n\r\n " +
"           var vm = {\r\n");

            
            #line 239 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                  
            
            #line default
            #line hidden
            
            #line 239 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                   if (!string.IsNullOrWhiteSpace(Model.Form.Partial))
                  {

            
            #line default
            #line hidden
WriteLiteral("                            ");

WriteLiteral("\r\n                            partial : partial,\r\n                            ");

WriteLiteral("\r\n");

            
            #line 244 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 245 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     foreach (var rule in Model.Form.Rules)
                    {
                        var function = rule.Dehumanize().ToCamelCase();

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 248 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     Write(function);

            
            #line default
            #line hidden
WriteLiteral(" : ");

            
            #line 248 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                 Write(function);

            
            #line default
            #line hidden
WriteLiteral(",");

WriteLiteral("\r\n");

            
            #line 249 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                activate: activate,\r\n                config: config,\r\n           " +
"     attached: attached,\r\n                entity: entity,\r\n                error" +
"s: errors,\r\n                save : save,\r\n");

            
            #line 256 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                
            
            #line default
            #line hidden
            
            #line 256 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var op in Model.EntityDefinition.EntityOperationCollection)
                {
                        var function = op.Name.ToCamelCase();

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 259 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     Write(function);

            
            #line default
            #line hidden
WriteLiteral(" : ");

            
            #line 259 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                 Write(function);

            
            #line default
            #line hidden
WriteLiteral(",");

WriteLiteral("\r\n");

            
            #line 260 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"

                }

            
            #line default
            #line hidden
WriteLiteral("                //\r\n\r\n");

            
            #line 264 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                
            
            #line default
            #line hidden
            
            #line 264 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                 foreach (var btn in Model.Form.FormDesign.FormElementCollection.OfType<Button>().Where(b => !string.IsNullOrWhiteSpace(b.CommandName)))
                {

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 266 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                      Write(btn.CommandName);

            
            #line default
            #line hidden
WriteLiteral(" : ");

            
            #line 266 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                           Write(btn.CommandName);

            
            #line default
            #line hidden
WriteLiteral(" ,\r\n                ");

WriteLiteral("\r\n");

            
            #line 268 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("\r\n                toolbar : {\r\n");

            
            #line 271 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 271 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsEmailAvailable)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("emailCommand : {\r\n                        entity : \"");

            
            #line 274 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                             Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\",\r\n                        id :id\r\n                    },");

WriteLiteral("\r\n");

            
            #line 277 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 278 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsPrintAvailable)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("printCommand :{\r\n                        entity : \'");

            
            #line 281 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                             Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                        id : id\r\n                    },");

WriteLiteral("\r\n");

            
            #line 284 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 285 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsRemoveAvailable)
            {

            
            #line default
            #line hidden
WriteLiteral("                ");

WriteLiteral("removeCommand :remove,\r\n                    canExecuteRemoveCommand : ko.computed" +
"(function(){\r\n                        return entity().");

            
            #line 289 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                    Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("Id();\r\n                    }),");

WriteLiteral("\r\n");

            
            #line 291 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 292 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (Model.Form.IsWatchAvailable)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n                    watchCommand: function() {\r\n                        return " +
"watcher.watch(\"");

            
            #line 296 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                         Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\", entity().");

            
            #line 296 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                                  Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral(@"Id())
                            .done(function(){
                                watching(true);
                            });
                    },
                    unwatchCommand: function() {
                        return watcher.unwatch(""");

            
            #line 302 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                           Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\", entity().");

            
            #line 302 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                                                                    Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("Id())\r\n                            .done(function(){\r\n                           " +
"     watching(false);\r\n                            });\r\n                    },\r\n" +
"                    watching: watching,");

WriteLiteral("\r\n");

            
            #line 308 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 309 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                     if (!string.IsNullOrWhiteSpace(@saveOperation))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        ");

WriteLiteral("\r\n                    saveCommand : ");

            
            #line 312 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                             Write(saveOperation.ToCamelCase());

            
            #line default
            #line hidden
WriteLiteral(",\r\n                    ");

WriteLiteral("\r\n");

            
            #line 314 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    commands : ko.observableArray(");

            
            #line 315 "..\..\Areas\Sph\Views\EntityFormRenderer\Script.cshtml"
                                             Write(Html.Raw(commandsJs));

            
            #line default
            #line hidden
WriteLiteral(")\r\n                }\r\n            };\r\n\r\n            return vm;\r\n        });\r\n</sc" +
"ript>");

        }
    }
}
#pragma warning restore 1591
