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

namespace Bespoke.Sph.Web.Areas.App.Views.EntityFormDesigner
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
    
    #line 1 "..\..\Areas\App\Views\EntityFormDesigner\Script.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityFormDesigner/Script.cshtml")]
    public partial class Script : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public Script()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\App\Views\EntityFormDesigner\Script.cshtml"
  
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

WriteAttribute("src", Tuple.Create(" src=\"", 421), Tuple.Create("\"", 462)
, Tuple.Create(Tuple.Create("", 427), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/schemas/form.designer.g.js")
, 427), false)
);

WriteLiteral("></script>\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" data-script=\"true\"");

WriteLiteral(@">

    define([objectbuilders.datacontext, objectbuilders.logger, objectbuilders.router, objectbuilders.system, objectbuilders.app, objectbuilders.eximp, objectbuilders.dialog],
        function (context, logger, router, system, app, eximp, dialog) {

            var entity = ko.observable(new bespoke.sph.domain.EntityDefinition()),
                form = ko.observable(new bespoke.sph.domain.EntityForm({WebId:system.guid()})),
                activate = function (entityid, formid) {
                    var elements = ");

            
            #line 21 "..\..\Areas\App\Views\EntityFormDesigner\Script.cshtml"
                              Write(Html.Raw(JsonConvert.SerializeObject(Model.FormElements,setting)));

            
            #line default
            #line hidden
WriteLiteral(",\r\n                        oels = _(elements.$values).map(function (v){return con" +
"text.toObservable(v);});\r\n                    vm.formElements(oels);\r\n\r\n        " +
"            var fid = parseInt(formid),\r\n                        id = parseInt(e" +
"ntityid),\r\n                        query = String.format(\"EntityDefinitionId eq " +
"{0}\", id),\r\n                        tcs = new $.Deferred();\r\n\r\n                 " +
"   context.loadOneAsync(\"EntityDefinition\", query)\r\n                        .don" +
"e(function (b) {\r\n                            entity(b);\r\n                      " +
"      if (!fid) {\r\n                                tcs.resolve(true);\r\n         " +
"                   }\r\n                            //b.loadSchema();\r\n           " +
"             });\r\n                    if (fid) {\r\n                        contex" +
"t.loadOneAsync(\"EntityForm\", \"EntityFormId eq \" + fid)\r\n                        " +
".done(function(f) {\r\n                            _(f.FormDesign().FormElementCol" +
"lection()).each(function(v) {\r\n                                v.isSelected = ko" +
".observable(false);\r\n                            });\r\n                          " +
"  form(f);\r\n                            tcs.resolve(true);\r\n                    " +
"    });\r\n                    }\r\n                    form().EntityDefinitionId(id" +
");\r\n\r\n                    return tcs.promise();\r\n\r\n                },\r\n         " +
"       attached = function(view) {\r\n\r\n                    var fd = ko.unwrap(for" +
"m().FormDesign);\r\n\r\n                    var dropDown = function (e) {\r\n         " +
"               e.preventDefault();\r\n                        e.stopPropagation();" +
"\r\n\r\n                        var button = $(this);\r\n                        butto" +
"n.parent().addClass(\"open\")\r\n                            .find(\"input:first\").fo" +
"cus()\r\n                            .select();\r\n                    };\r\n\r\n       " +
"             // Fix input element click problem\r\n                    $(view).on(" +
"\'click mouseup mousedown\', \'.dropdown-menu input, .dropdown-menu label\',\r\n      " +
"                  function (e) {\r\n                            e.stopPropagation(" +
");\r\n                        });\r\n                    $(\'#template-form-designer\'" +
").on(\'click\', \'button.dropdown-toggle\', dropDown);\r\n\r\n\r\n                    //to" +
"olbox item clicked\r\n                    $(\'#add-field\').on(\"click\", \'a\', functio" +
"n (e) {\r\n                        e.preventDefault();\r\n                        _(" +
"fd.FormElementCollection()).each(function (f) {\r\n                            f.i" +
"sSelected(false);\r\n                        });\r\n\r\n                        // clo" +
"ne\r\n                        var fe = ko.mapping.fromJS(ko.mapping.toJS(ko.dataFo" +
"r(this)));\r\n                        fe.isSelected = ko.observable(true);\r\n      " +
"                  fe.Label(\"Label \" + fd.FormElementCollection().length);\r\n     " +
"                   fe.CssClass(\"\");\r\n                        fe.Visible(\"true\");" +
"\r\n                        fe.Size(\"input-large\");\r\n                        fe.El" +
"ementId(system.guid());\r\n\r\n                        fd.FormElementCollection.push" +
"(fe);\r\n                        vm.selectedFormElement(fe);\r\n\r\n\r\n                " +
"    });\r\n\r\n                    // kendoEditor\r\n                    $(\'#template-" +
"form-designer\').on(\'click\', \'textarea\', function () {\r\n                        v" +
"ar $editor = $(this),\r\n                            kendoEditor = $editor.data(\"k" +
"endoEditor\");\r\n                        if (!kendoEditor) {\r\n                    " +
"        var htmlElement = ko.dataFor(this),\r\n                                edi" +
"tor = $editor.kendoEditor({\r\n                                    change:function" +
"() {\r\n                                        htmlElement.Text(this.value());\r\n " +
"                                   }\r\n                                }).data(\"k" +
"endoEditor\");\r\n\r\n                            htmlElement.Text.subscribe(function" +
"(t) {\r\n                                editor.value(ko.unwrap(t));\r\n            " +
"                });\r\n\r\n                        }\r\n                    }\r\n       " +
"             );\r\n                    $.getScript(\'/Scripts/jquery-ui-1.10.3.cust" +
"om.min.js\')// only contains UI core and interactions API\r\n                      " +
"  .done(function () {\r\n\r\n                            var initDesigner = function" +
" () {\r\n                                $(\'#template-form-designer>form\').sortabl" +
"e({\r\n                                    items: \'>div\',\r\n                       " +
"             placeholder: \'ph\',\r\n                                    helper: \'or" +
"iginal\',\r\n                                    dropOnEmpty: true,\r\n              " +
"                      forcePlaceholderSize: true,\r\n                             " +
"       forceHelperSize: false,\r\n                                    receive: rec" +
"eive\r\n                                });\r\n\r\n                            },\r\n   " +
"                             receive = function (evt, ui) {\r\n                   " +
"                 var elements = _($(\'#template-form-designer>form>div\')).map(fun" +
"ction (div) {\r\n                                        return ko.dataFor(div);\r\n" +
"                                    });\r\n                                    var" +
" fe = ko.dataFor(ui.item[0]);\r\n                                    fe.isSelected" +
" = ko.observable(true);\r\n                                    elements.splice(2, " +
"0, fe);\r\n                                    $(\'#template-form-designer>form\').s" +
"ortable(\"destroy\");\r\n\r\n\r\n                                    fd.FormElementColle" +
"ction(elements);\r\n                                };\r\n\r\n                        " +
"    initDesigner();\r\n                            $(\'#add-field>ul>li\').draggable" +
"({\r\n                                helper: \'clone\',\r\n                          " +
"      connectToSortable: \"#template-form-designer>form\"\r\n                       " +
"     });\r\n                        });\r\n\r\n                    $(\'section.context-" +
"action-panel\').on(\'click\', \'buton.close\', function() {\r\n                        " +
"$(this).parents(\'div.context-action\').hide();\r\n                    });\r\n        " +
"        },\r\n                supportsHtml5Storage = function () {\r\n              " +
"      try {\r\n                        return \'localStorage\' in window && window[\'" +
"localStorage\'] !== null;\r\n                    } catch (e) {\r\n                   " +
"     return false;\r\n                    }\r\n                },\r\n                o" +
"kClick = function (data, ev) {\r\n                    if (bespoke.utils.form.check" +
"Validity(ev.target)) {\r\n\r\n                        var fd = ko.unwrap(form().Form" +
"Design);\r\n                        // get the sorted element\r\n                   " +
"     var elements = _($(\'#template-form-designer>form>div\')).map(function (div) " +
"{\r\n                            return ko.dataFor(div);\r\n                        " +
"});\r\n                        fd.FormElementCollection(elements);\r\n              " +
"          dialog.close(this, \"OK\");\r\n                        if (supportsHtml5St" +
"orage()) {\r\n                            localStorage.removeItem(form().WebId());" +
"\r\n                        }\r\n                    }\r\n                },\r\n        " +
"        cancelClick = function () {\r\n                    if (supportsHtml5Storag" +
"e()) {\r\n                        localStorage.removeItem(form().WebId());\r\n      " +
"              }\r\n                    dialog.close(this, \"Cancel\");\r\n            " +
"    },\r\n                selectFormElement = function (fe) {\r\n\r\n                 " +
"   var fd = ko.unwrap(form().FormDesign);\r\n                    _(fd.FormElementC" +
"ollection()).each(function (f) {\r\n                        f.isSelected(false);\r\n" +
"                    });\r\n                    fe.isSelected(true);\r\n             " +
"       vm.selectedFormElement(fe);\r\n                    if (supportsHtml5Storage" +
"()) {\r\n                        localStorage.setItem(form().WebId(), ko.mapping.t" +
"oJSON(form));\r\n                    }\r\n                },\r\n                remove" +
"FormElement = function (fe) {\r\n                    var fd = ko.unwrap(form().For" +
"mDesign);\r\n                    fd.FormElementCollection.remove(fe);\r\n           " +
"     },\r\n                exportScreen = function() {\r\n                    return" +
" eximp.exportJson(ko.unwrap(form().Name) + \".json\", ko.mapping.toJSON(form));\r\n " +
"               },\r\n                open = function() {\r\n\r\n                },\r\n  " +
"              importCommand = function() {\r\n                    return eximp.imp" +
"ortJson()\r\n                 .done(function (json) {\r\n                     try {\r" +
"\n\r\n                         var obj = JSON.parse(json),\r\n                       " +
"      clone = context.toObservable(obj);\r\n\r\n                         form().Form" +
"Design(clone.FormDesign());\r\n\r\n                     } catch (error) {\r\n         " +
"                logger.logError(\'Fail template import tidak sah\', error, this, t" +
"rue);\r\n                     }\r\n                 });\r\n                },\r\n       " +
"         save = function() {\r\n                    var tcs = new $.Deferred();\r\n " +
"                   var data = ko.mapping.toJSON(form);\r\n\r\n                    co" +
"ntext.post(data, \"/Sph/EntityForm/Save\")\r\n                        .then(function" +
"(result) {\r\n                            tcs.resolve(result);\r\n                  " +
"      });\r\n                    return tcs.promise();\r\n                };\r\n\r\n    " +
"        var vm = {\r\n                attached: attached,\r\n                activat" +
"e: activate,\r\n                formElements: ko.observableArray(),\r\n             " +
"   selectedFormElement: ko.observable(),\r\n                selectFormElement : se" +
"lectFormElement,\r\n                removeFormElement : removeFormElement,\r\n      " +
"          form: form,\r\n                entity : entity,\r\n                okClick" +
": okClick,\r\n                cancelClick: cancelClick,\r\n                importCom" +
"mand :importCommand,\r\n                toolbar : {\r\n                    commands " +
":ko.observableArray([{\r\n                        caption : \'Create Pull Request\'," +
"\r\n                        icon : \'fa fa-folder-open-o\',\r\n                       " +
" command : open\r\n                    }\r\n                    ]),\r\n               " +
"     exportCommand : exportScreen,\r\n                    saveCommand : save\r\n    " +
"            }\r\n            };\r\n\r\n            return vm;\r\n\r\n        });\r\n\r\n\r\n</sc" +
"ript>\r\n");

        }
    }
}
#pragma warning restore 1591
