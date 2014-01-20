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
"            var id = parseInt(entityid),\r\n                        query = String" +
".format(\"EntityDefinitionId eq {0}\", id),\r\n                        tcs = new $.D" +
"eferred();\r\n\r\n                    context.loadOneAsync(\"EntityDefinition\", query" +
")\r\n                        .done(function (b) {\r\n                            ent" +
"ity(b);\r\n                            tcs.resolve(true);\r\n                       " +
"     //b.loadSchema();\r\n                        });\r\n                    var fid" +
" = parseInt(formid);\r\n                    if (fid) {\r\n                        co" +
"ntext.loadOneAsync(\"EntityForm\", \"EntityFormId eq \" + fid)\r\n                    " +
"    .done(function(f) {\r\n                            form(f);\r\n                 " +
"       });\r\n                    }\r\n                    form().EntityDefinitionId" +
"(id);\r\n\r\n                    return tcs.promise();\r\n\r\n                },\r\n      " +
"          attached = function(view) {\r\n\r\n                    var fd = ko.unwrap(" +
"form().FormDesign);\r\n\r\n                    var dropDown = function (e) {\r\n      " +
"                  e.preventDefault();\r\n                        e.stopPropagation" +
"();\r\n\r\n                        var button = $(this);\r\n                        bu" +
"tton.parent().addClass(\"open\")\r\n                            .find(\"input:first\")" +
".focus()\r\n                            .select();\r\n                    };\r\n\r\n    " +
"                // Fix input element click problem\r\n                    $(view)." +
"on(\'click mouseup mousedown\', \'.dropdown-menu input, .dropdown-menu label\',\r\n   " +
"                     function (e) {\r\n                            e.stopPropagati" +
"on();\r\n                        });\r\n                    $(\'#template-form-design" +
"er\').on(\'click\', \'button.dropdown-toggle\', dropDown);\r\n\r\n\r\n                    /" +
"/toolbox item clicked\r\n                    $(\'#add-field\').on(\"click\", \'a\', func" +
"tion (e) {\r\n                        e.preventDefault();\r\n                       " +
" _(fd.FormElementCollection()).each(function (f) {\r\n                            " +
"f.isSelected(false);\r\n                        });\r\n\r\n                        // " +
"clone\r\n                        var fe = ko.mapping.fromJS(ko.mapping.toJS(ko.dat" +
"aFor(this)));\r\n                        fe.isSelected = ko.observable(true);\r\n   " +
"                     fe.Label(\"Label \" + fd.FormElementCollection().length);\r\n  " +
"                      fe.CssClass(\"\");\r\n                        fe.Visible(\"true" +
"\");\r\n                        fe.Size(\"input-large\");\r\n                        fe" +
".ElementId(system.guid());\r\n\r\n                        fd.FormElementCollection.p" +
"ush(fe);\r\n                        vm.selectedFormElement(fe);\r\n\r\n\r\n             " +
"       });\r\n\r\n                    // kendoEditor\r\n                    $(\'#templa" +
"te-form-designer\').on(\'click\', \'textarea\', function () {\r\n                      " +
"  var $editor = $(this),\r\n                            kendoEditor = $editor.data" +
"(\"kendoEditor\");\r\n                        if (!kendoEditor) {\r\n                 " +
"           var htmlElement = ko.dataFor(this),\r\n                                " +
"editor = $editor.kendoEditor({\r\n                                    change:funct" +
"ion() {\r\n                                        htmlElement.Text(this.value());" +
"\r\n                                    }\r\n                                }).data" +
"(\"kendoEditor\");\r\n\r\n                            htmlElement.Text.subscribe(funct" +
"ion(t) {\r\n                                editor.value(ko.unwrap(t));\r\n         " +
"                   });\r\n\r\n                        }\r\n                    }\r\n    " +
"                );\r\n                    $.getScript(\'/Scripts/jquery-ui-1.10.3.c" +
"ustom.min.js\')// only contains UI core and interactions API\r\n                   " +
"     .done(function () {\r\n\r\n                            var initDesigner = funct" +
"ion () {\r\n                                $(\'#template-form-designer>form\').sort" +
"able({\r\n                                    items: \'>div\',\r\n                    " +
"                placeholder: \'ph\',\r\n                                    helper: " +
"\'original\',\r\n                                    dropOnEmpty: true,\r\n           " +
"                         forcePlaceholderSize: true,\r\n                          " +
"          forceHelperSize: false,\r\n                                    receive: " +
"receive\r\n                                });\r\n\r\n                            },\r\n" +
"                                receive = function (evt, ui) {\r\n                " +
"                    var elements = _($(\'#template-form-designer>form>div\')).map(" +
"function (div) {\r\n                                        return ko.dataFor(div)" +
";\r\n                                    });\r\n                                    " +
"var fe = ko.dataFor(ui.item[0]);\r\n                                    fe.isSelec" +
"ted = ko.observable(true);\r\n                                    elements.splice(" +
"2, 0, fe);\r\n                                    $(\'#template-form-designer>form\'" +
").sortable(\"destroy\");\r\n\r\n\r\n                                    fd.FormElementCo" +
"llection(elements);\r\n                                };\r\n\r\n                     " +
"       initDesigner();\r\n                            $(\'#add-field>ul>li\').dragga" +
"ble({\r\n                                helper: \'clone\',\r\n                       " +
"         connectToSortable: \"#template-form-designer>form\"\r\n                    " +
"        });\r\n                        });\r\n\r\n                    $(\'section.conte" +
"xt-action-panel\').on(\'click\', \'buton.close\', function() {\r\n                     " +
"   $(this).parents(\'div.context-action\').hide();\r\n                    });\r\n     " +
"           },\r\n                supportsHtml5Storage = function () {\r\n           " +
"         try {\r\n                        return \'localStorage\' in window && windo" +
"w[\'localStorage\'] !== null;\r\n                    } catch (e) {\r\n                " +
"        return false;\r\n                    }\r\n                },\r\n              " +
"  okClick = function (data, ev) {\r\n                    if (bespoke.utils.form.ch" +
"eckValidity(ev.target)) {\r\n\r\n                        var fd = ko.unwrap(form().F" +
"ormDesign);\r\n                        // get the sorted element\r\n                " +
"        var elements = _($(\'#template-form-designer>form>div\')).map(function (di" +
"v) {\r\n                            return ko.dataFor(div);\r\n                     " +
"   });\r\n                        fd.FormElementCollection(elements);\r\n           " +
"             dialog.close(this, \"OK\");\r\n                        if (supportsHtml" +
"5Storage()) {\r\n                            localStorage.removeItem(form().WebId(" +
"));\r\n                        }\r\n                    }\r\n                },\r\n     " +
"           cancelClick = function () {\r\n                    if (supportsHtml5Sto" +
"rage()) {\r\n                        localStorage.removeItem(form().WebId());\r\n   " +
"                 }\r\n                    dialog.close(this, \"Cancel\");\r\n         " +
"       },\r\n                selectFormElement = function (fe) {\r\n\r\n              " +
"      var fd = ko.unwrap(form().FormDesign);\r\n                    _(fd.FormEleme" +
"ntCollection()).each(function (f) {\r\n                        f.isSelected(false)" +
";\r\n                    });\r\n                    fe.isSelected(true);\r\n          " +
"          vm.selectedFormElement(fe);\r\n                    if (supportsHtml5Stor" +
"age()) {\r\n                        localStorage.setItem(form().WebId(), ko.mappin" +
"g.toJSON(form));\r\n                    }\r\n                },\r\n                rem" +
"oveFormElement = function (fe) {\r\n                    var fd = ko.unwrap(form()." +
"FormDesign);\r\n                    fd.FormElementCollection.remove(fe);\r\n        " +
"        },\r\n                exportScreen = function() {\r\n                    ret" +
"urn eximp.exportJson(ko.unwrap(form().Name) + \".json\", ko.mapping.toJSON(form));" +
"\r\n                },\r\n                open = function() {\r\n\r\n                },\r" +
"\n                importCommand = function() {\r\n                    return eximp." +
"importJson()\r\n                 .done(function (json) {\r\n                     try" +
" {\r\n\r\n                         var obj = JSON.parse(json),\r\n                    " +
"         clone = context.toObservable(obj);\r\n\r\n                         form().F" +
"ormDesign(clone.FormDesign());\r\n\r\n                     } catch (error) {\r\n      " +
"                   logger.logError(\'Fail template import tidak sah\', error, this" +
", true);\r\n                     }\r\n                 });\r\n                },\r\n    " +
"            save = function() {\r\n                    var tcs = new $.Deferred();" +
"\r\n                    var data = ko.mapping.toJSON(form);\r\n\r\n                   " +
" context.post(data, \"/Sph/EntityForm/Save\")\r\n                        .then(funct" +
"ion(result) {\r\n                            tcs.resolve(result);\r\n               " +
"         });\r\n                    return tcs.promise();\r\n                };\r\n\r\n " +
"           var vm = {\r\n                attached: attached,\r\n                acti" +
"vate: activate,\r\n                formElements: ko.observableArray(),\r\n          " +
"      selectedFormElement: ko.observable(),\r\n                selectFormElement :" +
" selectFormElement,\r\n                removeFormElement : removeFormElement,\r\n   " +
"             form: form,\r\n                entity : entity,\r\n                okCl" +
"ick: okClick,\r\n                cancelClick: cancelClick,\r\n                import" +
"Command :importCommand,\r\n                toolbar : {\r\n                    comman" +
"ds :ko.observableArray([{\r\n                        caption : \'Create Pull Reques" +
"t\',\r\n                        icon : \'fa fa-folder-open-o\',\r\n                    " +
"    command : open\r\n                    }\r\n                    ]),\r\n            " +
"        exportCommand : exportScreen,\r\n                    saveCommand : save\r\n " +
"               }\r\n            };\r\n\r\n            return vm;\r\n\r\n        });\r\n\r\n\r\n<" +
"/script>\r\n");

        }
    }
}
#pragma warning restore 1591
