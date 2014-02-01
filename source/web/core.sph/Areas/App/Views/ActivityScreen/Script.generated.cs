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
, Tuple.Create(Tuple.Create("", 300), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout-3.0.0.debug.js")
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

            var activate = function (wdid, screenid) {
                var elements = ");

            
            #line 20 "..\..\Areas\App\Views\ActivityScreen\Script.cshtml"
                          Write(Html.Raw(JsonConvert.SerializeObject(Model.FormElements,setting)));

            
            #line default
            #line hidden
WriteLiteral(@",
                    oels = _(elements.$values).map(function (v){return context.toObservable(v);});
                vm.formElements(oels);

                if (wdid) {
                    var id = parseInt(wdid),
                        query = String.format(""WorkflowDefinitionId eq {0}"", id),
                        tcs = new $.Deferred();

                    context.loadOneAsync(""WorkflowDefinition"", query)
                        .done(function (b) {
                            vm.wd(b);
                            tcs.resolve(true);
                            b.loadSchema();
                            var act = _(b.ActivityCollection()).find(function(v) { return v.WebId() == screenid; });
                            vm.activity(act);

                        });
                    return tcs.promise();
                }
                return Task.fromResult(true);
            },
                attached = function(view) {

                    if (!vm.activity().InvitationMessageBody())
                        vm.activity().InvitationMessageBody(""");

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
"true\");\r\n                        fe.Size(\"input-large\");\r\n                      " +
"  fe.ElementId(system.guid());\r\n\r\n                        fd.FormElementCollecti" +
"on.push(fe);\r\n                        vm.selectedFormElement(fe);\r\n\r\n\r\n         " +
"           });\r\n\r\n                    // kendoEditor\r\n                    $(\'#te" +
"mplate-form-designer\').on(\'click\', \'textarea\', function () {\r\n                  " +
"      var $editor = $(this),\r\n                            kendoEditor = $editor." +
"data(\"kendoEditor\");\r\n                        if (!kendoEditor) {\r\n             " +
"               var htmlElement = ko.dataFor(this),\r\n                            " +
"    editor = $editor.kendoEditor({\r\n                                    change:f" +
"unction() {\r\n                                        htmlElement.Text(this.value" +
"());\r\n                                    }\r\n                                })." +
"data(\"kendoEditor\");\r\n\r\n                            htmlElement.Text.subscribe(f" +
"unction(t) {\r\n                                editor.value(ko.unwrap(t));\r\n     " +
"                       });\r\n\r\n                        }\r\n                    }\r\n" +
"                    );\r\n                    $.getScript(\'/Scripts/jquery-ui-1.10" +
".3.custom.min.js\')// only contains UI core and interactions API\r\n               " +
"         .done(function () {\r\n\r\n                            var initDesigner = f" +
"unction () {\r\n                                $(\'#template-form-designer>form\')." +
"sortable({\r\n                                    items: \'>div\',\r\n                " +
"                    placeholder: \'ph\',\r\n                                    help" +
"er: \'original\',\r\n                                    dropOnEmpty: true,\r\n       " +
"                             forcePlaceholderSize: true,\r\n                      " +
"              forceHelperSize: false,\r\n                                    recei" +
"ve: receive\r\n                                });\r\n\r\n                            " +
"},\r\n                                receive = function (evt, ui) {\r\n            " +
"                        var elements = _($(\'#template-form-designer>form>div\'))." +
"map(function (div) {\r\n                                        return ko.dataFor(" +
"div);\r\n                                    });\r\n                                " +
"    var fe = ko.dataFor(ui.item[0]);\r\n                                    fe.isS" +
"elected = ko.observable(true);\r\n                                    elements.spl" +
"ice(2, 0, fe);\r\n                                    $(\'#template-form-designer>f" +
"orm\').sortable(\"destroy\");\r\n\r\n\r\n                                    fd.FormEleme" +
"ntCollection(elements);\r\n                                };\r\n\r\n                 " +
"           initDesigner();\r\n                            $(\'#add-field>ul>li\').dr" +
"aggable({\r\n                                helper: \'clone\',\r\n                   " +
"             connectToSortable: \"#template-form-designer>form\"\r\n                " +
"            });\r\n                            vm.wd().isBusy(false);\r\n           " +
"             });\r\n\r\n                    $(\'section.context-action-panel\').on(\'cl" +
"ick\', \'buton.close\', function() {\r\n                        $(this).parents(\'div." +
"context-action\').hide();\r\n                    });\r\n                },\r\n         " +
"       supportsHtml5Storage = function () {\r\n                    try {\r\n        " +
"                return \'localStorage\' in window && window[\'localStorage\'] !== nu" +
"ll;\r\n                    } catch (e) {\r\n                        return false;\r\n " +
"                   }\r\n                },\r\n                okClick = function (da" +
"ta, ev) {\r\n                    if (bespoke.utils.form.checkValidity(ev.target)) " +
"{\r\n\r\n                        var fd = ko.unwrap(vm.activity().FormDesign);\r\n    " +
"                    // get the sorted element\r\n                        var eleme" +
"nts = _($(\'#template-form-designer>form>div\')).map(function (div) {\r\n           " +
"                 return ko.dataFor(div);\r\n                        });\r\n         " +
"               fd.FormElementCollection(elements);\r\n                        dial" +
"og.close(this, \"OK\");\r\n                        if (supportsHtml5Storage()) {\r\n  " +
"                          localStorage.removeItem(vm.activity().WebId());\r\n     " +
"                   }\r\n                    }\r\n                },\r\n               " +
" cancelClick = function () {\r\n                    if (supportsHtml5Storage()) {\r" +
"\n                        localStorage.removeItem(vm.activity().WebId());\r\n      " +
"              }\r\n                    dialog.close(this, \"Cancel\");\r\n            " +
"    },\r\n                selectFormElement = function (fe) {\r\n\r\n                 " +
"   var fd = ko.unwrap(vm.activity().FormDesign);\r\n                    _(fd.FormE" +
"lementCollection()).each(function (f) {\r\n                        f.isSelected(fa" +
"lse);\r\n                    });\r\n                    fe.isSelected(true);\r\n      " +
"              vm.selectedFormElement(fe);\r\n                    if (supportsHtml5" +
"Storage()) {\r\n                        localStorage.setItem(vm.activity().WebId()" +
", ko.mapping.toJSON(vm.activity));\r\n                    }\r\n                },\r\n " +
"               removeFormElement = function (fe) {\r\n                    var fd =" +
" ko.unwrap(vm.activity().FormDesign);\r\n                    fd.FormElementCollect" +
"ion.remove(fe);\r\n                },\r\n                exportScreen = function() {" +
"\r\n                    return eximp.exportJson(ko.unwrap(vm.activity().Name) + \"." +
"json\", ko.mapping.toJSON(vm.activity));\r\n                },\r\n                ope" +
"n = function() {\r\n\r\n                },\r\n                importCommand = function" +
"() {\r\n                    return eximp.importJson()\r\n                 .done(func" +
"tion (json) {\r\n                     try {\r\n\r\n                         var obj = " +
"JSON.parse(json),\r\n                             clone = context.toObservable(obj" +
");\r\n\r\n                         vm.activity().FormDesign(clone.FormDesign());\r\n\r\n" +
"                     } catch (error) {\r\n                         logger.logError" +
"(\'Fail template import tidak sah\', error, this, true);\r\n                     }\r\n" +
"                 });\r\n                };\r\n\r\n            var vm = {\r\n            " +
"    attached: attached,\r\n                activate: activate,\r\n                fo" +
"rmElements: ko.observableArray(),\r\n                selectedFormElement: ko.obser" +
"vable(),\r\n                selectFormElement : selectFormElement,\r\n              " +
"  removeFormElement : removeFormElement,\r\n                activity: ko.observabl" +
"e(new bespoke.sph.domain.ScreenActivity()),\r\n                wd : ko.observable(" +
"new bespoke.sph.domain.WorkflowDefinition(system.guid())),\r\n                okCl" +
"ick: okClick,\r\n                cancelClick: cancelClick,\r\n                import" +
"Command :importCommand,\r\n                toolbar : {\r\n                    comman" +
"ds :ko.observableArray([{\r\n                        caption : \'Create Pull Reques" +
"t\',\r\n                        icon : \'fa fa-folder-open-o\',\r\n                    " +
"    command : open\r\n                    }\r\n                    ]),\r\n            " +
"        exportCommand : exportScreen\r\n                }\r\n            };\r\n\r\n     " +
"       vm.activity.subscribe(function(screen) {\r\n\r\n                var cached = " +
"localStorage.getItem(screen.WebId());\r\n                if (cached) {\r\n          " +
"          app.showMessage(\"There\'s cached data in your local storage, do you wan" +
"t to restore this?\", \"Local storage\", [\"Yes\", \"No\"])\r\n                        .d" +
"one(function(dr) {\r\n                            if (dr === \"Yes\") {\r\n           " +
"                     var screen2 = context.toObservable(JSON.parse(cached)),\r\n  " +
"                                  fd2 = ko.unwrap(screen2.FormDesign);\r\n\r\n      " +
"                          _(fd2.FormElementCollection()).each(function(v) {\r\n   " +
"                                 v.isSelected = ko.observable(false);\r\n         " +
"                       });\r\n                                if (typeof screen.Fo" +
"rmDesign === \"function\") {\r\n                                    screen.FormDesig" +
"n(screen2.FormDesign());\r\n                                } else {\r\n            " +
"                        screen.FormDesign.FormElementCollection(screen2.FormDesi" +
"gn().FormElementCollection());\r\n                                }\r\n\r\n           " +
"                     return;\r\n                            }\r\n                   " +
"     });\r\n                }\r\n\r\n                var fd = ko.unwrap(screen.FormDes" +
"ign);\r\n                _(fd.FormElementCollection()).each(function(v) {\r\n       " +
"             v.isSelected = ko.observable(false);\r\n                });\r\n        " +
"    });\r\n\r\n\r\n            return vm;\r\n\r\n        });\r\n\r\n\r\n</script>\r\n");

        }
    }
}
#pragma warning restore 1591
