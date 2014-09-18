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

namespace Bespoke.Sph.Web.Areas.App.Views.ActivityScreen
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
    
    #line 1 "..\..\Areas\App\Views\ActivityScreen\Script.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ActivityScreen/Script.cshtml")]
    public partial class Script : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public Script()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\App\Views\ActivityScreen\Script.cshtml"
  
    Layout = null;
    var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };

            
            #line default
            #line hidden
WriteLiteral("\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 242), Tuple.Create("\"", 274)
, Tuple.Create(Tuple.Create("", 248), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/objectbuilders.js")
, 248), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 294), Tuple.Create("\"", 333)
, Tuple.Create(Tuple.Create("", 300), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout-3.2.0.debug.js")
, 300), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 353), Tuple.Create("\"", 401)
, Tuple.Create(Tuple.Create("", 359), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout.mapping-latest.debug.js")
, 359), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 421), Tuple.Create("\"", 465)
, Tuple.Create(Tuple.Create("", 427), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/schemas/trigger.workflow.g.js")
, 427), false)
);

WriteLiteral("></script>\r\n<script");

WriteAttribute("src", Tuple.Create(" src=\"", 485), Tuple.Create("\"", 526)
, Tuple.Create(Tuple.Create("", 491), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/schemas/form.designer.g.js")
, 491), false)
);

WriteLiteral("></script>\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" data-script=\"true\"");

WriteLiteral(@">

    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app, objectbuilders.eximp, objectbuilders.dialog],
        function (context, logger, router, system, app, eximp, dialog) {

            var runningInDialog = ko.observable(),
                entityOptions = ko.observableArray(),
                activate = function (wdid, screenid) {
                    var elements = ");

            
            #line 22 "..\..\Areas\App\Views\ActivityScreen\Script.cshtml"
                              Write(Html.Raw(JsonConvert.SerializeObject(Model.FormElements,setting)));

            
            #line default
            #line hidden
WriteLiteral(",\r\n                        oels = _(elements.$values).map(function (v){return con" +
"text.toObservable(v);});\r\n                    vm.formElements(oels);\r\n          " +
"          \r\n                    context.getListAsync(\"EntityDefinition\", null, \"" +
"Name\")\r\n                        .then(function(entities) {\r\n                    " +
"        var list = _(entities).map(function(v) {\r\n                              " +
"  return {\r\n                                    text : v,\r\n                     " +
"               value :v\r\n                                };\r\n                   " +
"         });\r\n\r\n                            list.push({ text: \'UserProfile*\', va" +
"lue: \'UserProfile\' });\r\n                            list.push({ text: \'Designati" +
"on*\', value: \'Designation\' });\r\n                            list.push({ text: \'D" +
"epartment*\', value: \'Department\' });\r\n                            entityOptions(" +
"list);\r\n                        });\r\n\r\n                    if (id) {\r\n          " +
"              var query = String.format(\"Id eq \'{0}\'\", id),\r\n                   " +
"         tcs = new $.Deferred();\r\n\r\n                        context.loadOneAsync" +
"(\"WorkflowDefinition\", query)\r\n                            .done(function (b) {\r" +
"\n                                vm.wd(b);\r\n                                tcs." +
"resolve(true);\r\n                                b.loadSchema();\r\n               " +
"                 var act = _(b.ActivityCollection()).find(function(v) { return v" +
".WebId() == screenid; });\r\n                                vm.activity(act);\r\n\r\n" +
"                            });\r\n                        return tcs.promise();\r\n" +
"                    }\r\n                    return Task.fromResult(true);\r\n      " +
"          },\r\n                attached = function(view) {\r\n                    r" +
"unningInDialog(window.location.href.indexOf(\"screen.editor\") < 0);\r\n            " +
"        if (!vm.activity().InvitationMessageBody())\r\n                        vm." +
"activity().InvitationMessageBody(\"");

WriteLiteral("@Model.Screen.Name task is assigned to you go here ");

WriteLiteral("@Model.Url\");\r\n                    if (!vm.activity().InvitationMessageSubject())" +
"\r\n                        vm.activity().InvitationMessageSubject(\"[Sph] ");

WriteLiteral("@Model.Screen.Name  task is assigned to you\");\r\n\r\n                    if (!vm.act" +
"ivity().CancelMessageBody())\r\n                        vm.activity().CancelMessag" +
"eBody(\"");

WriteLiteral("@Model.Screen.Name task was cancelled this url is not longer valid ");

WriteLiteral("@Model.Url\");\r\n                    if (!vm.activity().CancelMessageSubject())\r\n  " +
"                      vm.activity().CancelMessageSubject(\"[Sph] ");

WriteLiteral("@Model.Screen.Name was cancelled\");\r\n\r\n                    var fd = ko.unwrap(vm." +
"activity().FormDesign);\r\n\r\n                    var dropDown = function (e) {\r\n  " +
"                      e.preventDefault();\r\n                        e.stopPropaga" +
"tion();\r\n\r\n                        var button = $(this);\r\n                      " +
"  button.parent().addClass(\"open\")\r\n                            .find(\"input:fir" +
"st\").focus()\r\n                            .select();\r\n                    };\r\n\r\n" +
"                    // Fix input element click problem\r\n                    $(vi" +
"ew).on(\'click mouseup mousedown\', \'.dropdown-menu input, .dropdown-menu label\',\r" +
"\n                        function (e) {\r\n                            e.stopPropa" +
"gation();\r\n                        });\r\n                    $(\'#template-form-de" +
"signer\').on(\'click\', \'button.dropdown-toggle\', dropDown);\r\n\r\n\r\n                 " +
"   //toolbox item clicked\r\n                    $(\'#add-field\').on(\"click\", \'a\', " +
"function (e) {\r\n                        e.preventDefault();\r\n                   " +
"     _(fd.FormElementCollection()).each(function (f) {\r\n                        " +
"    f.isSelected(false);\r\n                        });\r\n\r\n                       " +
" // clone\r\n                        var fe = ko.mapping.fromJS(ko.mapping.toJS(ko" +
".dataFor(this)));\r\n                        fe.isSelected = ko.observable(true);\r" +
"\n                        fe.Label(\"Label \" + fd.FormElementCollection().length);" +
"\r\n                        fe.CssClass(\"\");\r\n                        fe.Visible(\"" +
"true\");\r\n                        fe.Enable(\"true\");\r\n                        fe." +
"Size(\"input-large\");\r\n                        fe.ElementId(system.guid());\r\n\r\n  " +
"                      fd.FormElementCollection.push(fe);\r\n                      " +
"  vm.selectedFormElement(fe);\r\n\r\n\r\n                    });\r\n\r\n                  " +
"  // kendoEditor\r\n                    $(\'#template-form-designer\').on(\'click\', \'" +
"textarea\', function () {\r\n                        var $editor = $(this),\r\n      " +
"                      kendoEditor = $editor.data(\"kendoEditor\");\r\n              " +
"          if (!kendoEditor) {\r\n                            var htmlElement = ko." +
"dataFor(this),\r\n                                editor = $editor.kendoEditor({\r\n" +
"                                    change:function() {\r\n                       " +
"                 htmlElement.Text(this.value());\r\n                              " +
"      }\r\n                                }).data(\"kendoEditor\");\r\n\r\n            " +
"                htmlElement.Text.subscribe(function(t) {\r\n                      " +
"          editor.value(ko.unwrap(t));\r\n                            });\r\n\r\n      " +
"                  }\r\n                    }\r\n                    );\r\n\r\n\r\n        " +
"            var initDesigner = function () {\r\n                        $(\'#templa" +
"te-form-designer>form\').sortable({\r\n                            items: \'>div\',\r\n" +
"                            placeholder: \'ph\',\r\n                            help" +
"er: \'original\',\r\n                            dropOnEmpty: true,\r\n               " +
"             forcePlaceholderSize: true,\r\n                            forceHelpe" +
"rSize: false,\r\n                            receive: receive\r\n                   " +
"     });\r\n\r\n                    },\r\n                        receive = function (" +
"evt, ui) {\r\n                            var elements = _($(\'#template-form-desig" +
"ner>form>div\')).map(function (div) {\r\n                                return ko." +
"dataFor(div);\r\n                            });\r\n                            var " +
"fe = ko.dataFor(ui.item[0]);\r\n                            fe.isSelected = ko.obs" +
"ervable(true);\r\n                            elements.splice(2, 0, fe);\r\n        " +
"                    $(\'#template-form-designer>form\').sortable(\"destroy\");\r\n\r\n\r\n" +
"                            fd.FormElementCollection(elements);\r\n               " +
"         };\r\n\r\n                    initDesigner();\r\n                    $(\'#add-" +
"field>ul>li\').draggable({\r\n                        helper: \'clone\',\r\n           " +
"             connectToSortable: \"#template-form-designer>form\"\r\n                " +
"    });\r\n                    vm.wd().isBusy(false);\r\n\r\n\r\n                    $(\'" +
"section.context-action-panel\').on(\'click\', \'buton.close\', function() {\r\n        " +
"                $(this).parents(\'div.context-action\').hide();\r\n                 " +
"   });\r\n                },\r\n                supportsHtml5Storage = function () {" +
"\r\n                    try {\r\n                        return \'localStorage\' in wi" +
"ndow && window[\'localStorage\'] !== null;\r\n                    } catch (e) {\r\n   " +
"                     return false;\r\n                    }\r\n                },\r\n " +
"               okClick = function (data, ev) {\r\n                    if (bespoke." +
"utils.form.checkValidity(ev.target)) {\r\n\r\n                        var fd = ko.un" +
"wrap(vm.activity().FormDesign);\r\n                        // get the sorted eleme" +
"nt\r\n                        var elements = _($(\'#template-form-designer>form>div" +
"\')).map(function (div) {\r\n                            return ko.dataFor(div);\r\n " +
"                       });\r\n                        fd.FormElementCollection(ele" +
"ments);\r\n                        dialog.close(this, \"OK\");\r\n                    " +
"    if (supportsHtml5Storage()) {\r\n                            localStorage.remo" +
"veItem(vm.activity().WebId());\r\n                        }\r\n                    }" +
"\r\n                },\r\n                cancelClick = function () {\r\n             " +
"       if (supportsHtml5Storage()) {\r\n                        localStorage.remov" +
"eItem(vm.activity().WebId());\r\n                    }\r\n                    dialog" +
".close(this, \"Cancel\");\r\n                },\r\n                selectFormElement =" +
" function (fe) {\r\n                    $(\'.selected-form-element\').each(function(" +
"e) {\r\n                        var kd = ko.dataFor(this);\r\n                      " +
"  if (typeof kd.isSelected === \"function\")\r\n                            kd.isSel" +
"ected(false);\r\n                    });\r\n\r\n                    if (typeof fe.isSe" +
"lected !== \"function\") {\r\n                        fe.isSelected = ko.observable(" +
"true);\r\n                    } else {\r\n                        fe.isSelected(true" +
");\r\n                    }\r\n                    vm.selectedFormElement(fe);\r\n    " +
"                if (supportsHtml5Storage()) {\r\n                        localStor" +
"age.setItem(vm.activity().WebId(), ko.mapping.toJSON(vm.activity));\r\n           " +
"         }\r\n                },\r\n                removeFormElement = function (fe" +
") {\r\n                    var fd = ko.unwrap(vm.activity().FormDesign);\r\n        " +
"            fd.FormElementCollection.remove(fe);\r\n                },\r\n          " +
"      exportScreen = function() {\r\n                    return eximp.exportJson(k" +
"o.unwrap(vm.activity().Name) + \".json\", ko.mapping.toJSON(vm.activity));\r\n      " +
"          },\r\n                open = function() {\r\n\r\n                },\r\n       " +
"         importCommand = function() {\r\n                   \r\n                    " +
"return eximp.importJson()\r\n                        .done(function (json) {\r\n    " +
"                         try {\r\n                                 var obj = JSON." +
"parse(json),\r\n                                     clone = context.toObservable(" +
"obj);\r\n\r\n                                 vm.activity().FormDesign(clone.FormDes" +
"ign());\r\n\r\n                             } catch (error) {\r\n                     " +
"            logger.logError(\'Fail template import tidak sah\', error, this, true)" +
";\r\n                             }\r\n                         });\r\n               " +
" };\r\n\r\n            var vm = {\r\n                runningInDialog: runningInDialog," +
"\r\n                entityOptions: entityOptions,\r\n                attached: attac" +
"hed,\r\n                activate: activate,\r\n                formElements: ko.obse" +
"rvableArray(),\r\n                selectedFormElement: ko.observable(),\r\n         " +
"       selectFormElement : selectFormElement,\r\n                removeFormElement" +
" : removeFormElement,\r\n                activity: ko.observable(new bespoke.sph.d" +
"omain.ScreenActivity()),\r\n                wd : ko.observable(new bespoke.sph.dom" +
"ain.WorkflowDefinition(system.guid())),\r\n                okClick: okClick,\r\n    " +
"            cancelClick: cancelClick,\r\n                importCommand :importComm" +
"and,\r\n                toolbar : {\r\n                    commands :ko.observableAr" +
"ray([\r\n                    ]),\r\n                    exportCommand : exportScreen" +
"\r\n                }\r\n            };\r\n\r\n            vm.activity.subscribe(functio" +
"n(screen) {\r\n\r\n                var cached = localStorage.getItem(screen.WebId())" +
";\r\n                if (cached) {\r\n                    setTimeout(function() {\r\n " +
"                       app.showMessage(\"There\'s cached data in your local storag" +
"e, do you want to restore this?\", \"Local storage\", [\"Yes\", \"No\"])\r\n             " +
"               .done(function(dr) {\r\n                                if (dr === " +
"\"Yes\") {\r\n                                    var screen2 = context.toObservable" +
"(JSON.parse(cached)),\r\n                                        fd2 = ko.unwrap(s" +
"creen2.FormDesign);\r\n\r\n                                    _(fd2.FormElementColl" +
"ection()).each(function(v) {\r\n                                        v.isSelect" +
"ed = ko.observable(false);\r\n                                    });\r\n           " +
"                         if (typeof screen.FormDesign === \"function\") {\r\n       " +
"                                 screen.FormDesign(screen2.FormDesign());\r\n     " +
"                               } else {\r\n                                       " +
" screen.FormDesign.FormElementCollection(screen2.FormDesign().FormElementCollect" +
"ion());\r\n                                    }\r\n\r\n                              " +
"      return;\r\n                                } else {\r\n                       " +
"             localStorage.removeItem(screen.WebId());\r\n                         " +
"       }\r\n                            });\r\n                    }, 2500);\r\n      " +
"          }\r\n\r\n                var fd = ko.unwrap(screen.FormDesign);\r\n         " +
"       _(fd.FormElementCollection()).each(function(v) {\r\n                    v.i" +
"sSelected = ko.observable(false);\r\n                });\r\n            });\r\n\r\n\r\n   " +
"         return vm;\r\n\r\n        });\r\n\r\n\r\n</script>\r\n");

        }
    }
}
#pragma warning restore 1591
