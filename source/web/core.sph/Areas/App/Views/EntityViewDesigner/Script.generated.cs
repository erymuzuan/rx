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
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityViewDesigner/Script.cshtml")]
    public partial class _Areas_App_Views_EntityViewDesigner_Script_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public _Areas_App_Views_EntityViewDesigner_Script_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\App\Views\EntityViewDesigner\Script.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 93), Tuple.Create("\"", 125)
, Tuple.Create(Tuple.Create("", 99), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/objectbuilders.js")
, 99), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 145), Tuple.Create("\"", 184)
, Tuple.Create(Tuple.Create("", 151), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout-3.4.0.debug.js")
, 151), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 204), Tuple.Create("\"", 252)
, Tuple.Create(Tuple.Create("", 210), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout.mapping-latest.debug.js")
, 210), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 272), Tuple.Create("\"", 313)
, Tuple.Create(Tuple.Create("", 278), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/schemas/form.designer.g.js")
, 278), false)
);

WriteLiteral("></script>\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" data-script=\"true\"");

WriteLiteral(">\r\n\r\n    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilder" +
"s.router, objectbuilders.system, objectbuilders.app],\r\n        function (context" +
", logger, router, system, app) {\r\n\r\n            var errors = ko.observableArray(" +
"),\r\n                endpointOptions = ko.observableArray(),\r\n                tem" +
"plateOptions = ko.observableArray(),\r\n                originalEntity = \"\",\r\n    " +
"            warnings = ko.observableArray(),\r\n                entity = ko.observ" +
"able(new bespoke.sph.domain.EntityDefinition()),\r\n                view = ko.obse" +
"rvable(new bespoke.sph.domain.EntityView({ WebId: system.guid() })),\r\n          " +
"      activate = function (id) {\r\n\r\n\r\n                    context.getListAsync(\"" +
"ViewTemplate\", \"Id ne \'0\'\", \"Name\")\r\n                            .done(templateO" +
"ptions);\r\n\r\n                    return context.loadOneAsync(\"EntityView\", \"Id eq" +
" \'\" + id + \"\'\")\r\n                            .then(function (f) {\r\n             " +
"                   view(f);\r\n                                return context.load" +
"OneAsync(\"EntityDefinition\", \"Id eq \'\" + ko.unwrap(f.EntityDefinitionId) + \"\'\");" +
"\r\n                            })\r\n                            .then(function (b)" +
" {\r\n                                entity(b);\r\n                                " +
"window.typeaheadEntity = b.Name();\r\n                                return conte" +
"xt.getListAsync(\"QueryEndpoint\", \"Entity eq \'\" + ko.unwrap(b.Name) + \"\'\", \"Id\");" +
"\r\n                            })\r\n                            .then(endpointOpti" +
"ons);\r\n\r\n\r\n                },\r\n                attached = function (vw) {\r\n\r\n   " +
"                 originalEntity = ko.toJSON(view);\r\n                    $(vw).on" +
"(\"click\", \"#expand-collapse-property-tabe\", function () {\r\n                     " +
"   if ($(this).html().indexOf(\"fa-expand\") > -1) {\r\n\r\n                          " +
"  $(\"#view-column-designer\")\r\n                                    .removeClass(\"" +
"col-lg-8\").addClass(\"col-lg-4\")\r\n                                    .removeClas" +
"s(\"col-md-8\").addClass(\"col-md-4\");\r\n                            $(\"#view-proper" +
"ties-tab\")\r\n                                .removeClass(\"col-lg-4\").addClass(\"c" +
"ol-lg-8\")\r\n                                .removeClass(\"col-md-4\").addClass(\"co" +
"l-md-8\");\r\n                            $(this).html(\'<i class=\"fa fa-compress\"><" +
"/i>\');\r\n                        } else {\r\n\r\n                            $(\"#view" +
"-column-designer\")\r\n                                .removeClass(\"col-lg-4\").add" +
"Class(\"col-lg-8\")\r\n                                .removeClass(\"col-md-4\").addC" +
"lass(\"col-md-8\");\r\n                            $(\"#view-properties-tab\")\r\n      " +
"                          .removeClass(\"col-lg-8\").addClass(\"col-lg-4\")\r\n       " +
"                         .removeClass(\"col-md-8\").addClass(\"col-md-4\");\r\n       " +
"                     $(this).html(\'<i class=\"fa fa-expand\"></i>\');\r\n            " +
"            }\r\n                    });\r\n\r\n                },\r\n                pu" +
"blish = function () {\r\n\r\n                    // get the sorted element\r\n        " +
"            var columns = _($(\'ul#column-design>li:not(:last)\')).map(function (d" +
"iv) {\r\n                        return ko.dataFor(div);\r\n                    });\r" +
"\n                    view().ViewColumnCollection(columns);\r\n\r\n                  " +
"  var data = ko.mapping.toJSON(view);\r\n\r\n                    return context.put(" +
"data, \"/api/entity-views/\" + ko.unwrap(view().Id) + \"/publish\")\r\n               " +
"         .then(function (result) {\r\n                            if (result.succe" +
"ss) {\r\n                                logger.info(result.message);\r\n           " +
"                     view().Id(result.id);\r\n                                erro" +
"rs.removeAll();\r\n                                view().IsPublished(true);\r\n    " +
"                            originalEntity = ko.toJSON(view);\r\n                 " +
"           } else {\r\n                                errors(result.Errors);\r\n   " +
"                             logger.error(\"There are errors in your entity, !!!\"" +
");\r\n                            }\r\n\r\n                        });\r\n              " +
"  },\r\n                save = function () {\r\n                    // get the sorte" +
"d element\r\n                    var columns = _($(\"ul#column-design>li:not(:last)" +
"\")).map(function (div) {\r\n                        return ko.dataFor(div);\r\n     " +
"               });\r\n                    view().ViewColumnCollection(columns);\r\n\r" +
"\n                    var data = ko.mapping.toJSON(view);\r\n\r\n                    " +
"return context.put(data, \"/api/entity-views/\")\r\n                        .then(fu" +
"nction (result) {\r\n                            view().Id(result.id);\r\n          " +
"                  logger.info(result.message);\r\n                            orig" +
"inalEntity = ko.toJSON(view);\r\n                        });\r\n                },\r\n" +
"            canDeactivate = function () {\r\n                var tcs = new $.Defer" +
"red();\r\n                if (originalEntity !== ko.toJSON(view)) {\r\n             " +
"       app.showMessage(\"Save change to the item\", \"Rx Developer\", [\"Yes\", \"No\", " +
"\"Cancel\"])\r\n                        .done(function (dialogResult) {\r\n           " +
"                 if (dialogResult === \"Yes\") {\r\n                                " +
"save().done(function () {\r\n                                    tcs.resolve(true)" +
";\r\n                                });\r\n                            }\r\n         " +
"                   if (dialogResult === \"No\") {\r\n                               " +
" tcs.resolve(true);\r\n                            }\r\n                            " +
"if (dialogResult === \"Cancel\") {\r\n                                tcs.resolve(fa" +
"lse);\r\n                            }\r\n\r\n                        });\r\n           " +
"     } else {\r\n                    return true;\r\n                }\r\n            " +
"    return tcs.promise();\r\n            },\r\n                remove = function () " +
"{\r\n\r\n                    var tcs = new $.Deferred(),\r\n                        da" +
"ta = ko.mapping.toJSON(view);\r\n                    app.showMessage(\"Are you sure" +
" you want to delete this view? This action cannot be undone.\", \"Reactive Develop" +
"er\", [\"Yes\", \"No\"])\r\n                        .done(function (dialogResult) {\r\n  " +
"                          if (dialogResult === \"Yes\") {\r\n                       " +
"         context.send(data, \"/api/entity-views/\" + ko.unwrap(view().Id) , \"DELET" +
"E\")\r\n                                    .done(function () {\r\n                  " +
"                      window.location = \"/sph#dev.home\";\r\n                      " +
"              })\r\n                                    .fail(function (v) {\r\n    " +
"                                    logger.error(v.statusText);\r\n               " +
"                         tcs.reject(v);\r\n                                    })\r" +
"\n                                    .then(tcs.resolve);\r\n                      " +
"      }\r\n                            tcs.resolve(false);\r\n                      " +
"  });\r\n                    return tcs.promise();\r\n                },\r\n\r\n        " +
"    depublishAsync = function () {\r\n\r\n                var data = ko.mapping.toJS" +
"ON(view);\r\n\r\n                return context.put(data, \"/api/entity-views/\" + ko." +
"unwrap(view().Id) + \"/depublish\")\r\n                     .then(function (result) " +
"{\r\n                         if (result.success) {\r\n                             " +
"view().IsPublished(false);\r\n                             logger.info(result.mess" +
"age);\r\n                             errors.removeAll();\r\n                       " +
"  } else {\r\n                             logger.error(\"There are errors in your " +
"view, !!!\");\r\n                         }\r\n                     });\r\n            " +
"},\r\n            partialEditor = null,\r\n            editCode = function () {\r\n   " +
"             if (null == partialEditor || partialEditor.closed) {\r\n             " +
"       var partial = \"partial/\" + view().Route();\r\n                    partialEd" +
"itor = window.open(\"/sph/editor/file?id=/sphapp/\" + partial + \".js\", \'_blank\', \'" +
"height=600px,width=800px,toolbar=0,location=0\');\r\n                    view().Par" +
"tial(partial);\r\n                } else {\r\n                    partialEditor.focu" +
"s();\r\n                }\r\n\r\n                return Task.fromResult(true);\r\n\r\n    " +
"        },\r\n            translateLabels = function () {\r\n                var tcs" +
" = new $.Deferred(),\r\n                    columns = view().ViewColumnCollection(" +
");\r\n                require([\"viewmodels/resource.table.dialog\", \"durandal/app\"]" +
", function (dg, app2) {\r\n                    dg.keys(_(columns).map(function (v)" +
" {\r\n                        return ko.unwrap(v.Header);\r\n                    }))" +
";\r\n                    dg.resource(view().Route());\r\n                    app2.sh" +
"owDialog(dg).done(tcs.resolve);\r\n\r\n                });\r\n\r\n                return" +
" tcs.promise();\r\n            };\r\n\r\n            var vm = {\r\n                warni" +
"ngs: warnings,\r\n                errors: errors,\r\n                endpointOptions" +
": endpointOptions,\r\n                templateOptions: templateOptions,\r\n         " +
"       attached: attached,\r\n                activate: activate,\r\n               " +
" canDeactivate: canDeactivate,\r\n                view: view,\r\n                ent" +
"ity: entity,\r\n                formsQuery: ko.computed(function () {\r\n           " +
"         return String.format(\"EntityDefinitionId eq \'{0}\'\", ko.unwrap(entity()." +
"Id));\r\n                }),\r\n                toolbar: {\r\n                    comm" +
"ands: ko.observableArray([{\r\n                        caption: \"Clone\",\r\n        " +
"                icon: \"fa fa-copy\",\r\n                        command: function (" +
") {\r\n                            view().Name(view().Name() + \' Copy (1)\');\r\n    " +
"                        view().Route(\'\');\r\n                            view().Id" +
"(\"0\");\r\n                            return Task.fromResult(0);\r\n                " +
"        }\r\n                    },\r\n                    {\r\n                      " +
"  caption: \"Publish\",\r\n                        icon: \"fa fa-sign-in\",\r\n         " +
"               command: publish,\r\n                        enable: ko.computed(fu" +
"nction () {\r\n                            return view().Id() && view().Id() !== \"" +
"0\";\r\n                        })\r\n                    },\r\n                    {\r\n" +
"                        caption: \"Depublish\",\r\n                        icon: \"fa" +
" fa-sign-out\",\r\n                        command: depublishAsync,\r\n              " +
"          enable: ko.computed(function () {\r\n                            return " +
"view().Id() && view().Id() !== \"0\" && view().IsPublished();\r\n                   " +
"     })\r\n                    },\r\n                    {\r\n                        " +
"command: editCode,\r\n                        caption: \"Edit Code\",\r\n             " +
"           icon: \"fa fa-code\",\r\n                        enable: ko.computed(func" +
"tion () {\r\n                            return view().Route();\r\n                 " +
"       })\r\n                    },\r\n                    {\r\n                      " +
"  command: translateLabels,\r\n                        caption: \"Translate\",\r\n    " +
"                    icon: \"fa fa-language\",\r\n                        enable: ko." +
"computed(function () {\r\n                            return view().Route();\r\n    " +
"                    })\r\n                    }\r\n                    ]),\r\n        " +
"            saveCommand: save,\r\n                    removeCommand: remove\r\n     " +
"           }\r\n            };\r\n\r\n            return vm;\r\n\r\n        });\r\n\r\n\r\n</scr" +
"ipt>\r\n");

        }
    }
}
#pragma warning restore 1591
