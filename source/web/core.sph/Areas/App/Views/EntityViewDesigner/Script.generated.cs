﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Areas.App.Views.EntityViewDesigner
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
    public partial class Script : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public Script()
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
, Tuple.Create(Tuple.Create("", 151), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/knockout-3.1.0.debug.js")
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
"s.router, objectbuilders.system],\r\n        function (context, logger, router, sy" +
"stem) {\r\n\r\n            var errors = ko.observableArray(),\r\n                entit" +
"y = ko.observable(new bespoke.sph.domain.EntityDefinition()),\r\n                v" +
"iew = ko.observable(new bespoke.sph.domain.EntityView({ WebId: system.guid() }))" +
",\r\n                activate = function (entityid, viewid) {\r\n\r\n\r\n               " +
"     var vid = parseInt(viewid),\r\n                        id = parseInt(entityid" +
"),\r\n                        query = String.format(\"EntityDefinitionId eq {0}\", i" +
"d),\r\n                        tcs = new $.Deferred();\r\n\r\n                    cont" +
"ext.loadOneAsync(\"EntityDefinition\", query)\r\n                        .done(funct" +
"ion (b) {\r\n                            entity(b);\r\n                            i" +
"f (!vid) {\r\n                                tcs.resolve(true);\r\n                " +
"            }\r\n\r\n                        });\r\n                    if (vid) {\r\n  " +
"                      context.loadOneAsync(\"EntityView\", \"EntityViewId eq \" + vi" +
"d)\r\n                        .done(function (f) {\r\n\r\n                            " +
"view(f);\r\n                            tcs.resolve(true);\r\n                      " +
"  });\r\n                    } else {\r\n\r\n                        view(new bespoke." +
"sph.domain.EntityView({ WebId: system.guid() }));\r\n                    }\r\n      " +
"              view().EntityDefinitionId(id);\r\n\r\n                    return tcs.p" +
"romise();\r\n\r\n                },\r\n                attached = function () {\r\n\r\n\r\n\r" +
"\n                },\r\n                publish = function () {\r\n                  " +
"  var tcs = new $.Deferred(),\r\n                        data = ko.mapping.toJSON(" +
"view);\r\n\r\n                    context.post(data, \"/Sph/EntityView/Publish\")\r\n   " +
"                     .then(function (result) {\r\n                            if (" +
"result.success) {\r\n                                logger.info(result.message);\r" +
"\n                                view().EntityViewId(result.id);\r\n              " +
"                  errors.removeAll();\r\n                            } else {\r\n   " +
"                             errors(result.Errors);\r\n                           " +
"     logger.error(\"There are errors in your entity, !!!\");\r\n                    " +
"        }\r\n\r\n                            tcs.resolve(result);\r\n                 " +
"       });\r\n                    return tcs.promise();\r\n                },\r\n     " +
"           save = function () {\r\n                    var tcs = new $.Deferred()," +
"\r\n                        data = ko.mapping.toJSON(view);\r\n\r\n                   " +
" context.post(data, \"/Sph/EntityView/Save\")\r\n                        .then(funct" +
"ion (result) {\r\n                            view().EntityViewId(result.id);\r\n   " +
"                         tcs.resolve(result);\r\n                        });\r\n    " +
"                return tcs.promise();\r\n                };\r\n\r\n            var vm " +
"= {\r\n                errors: errors,\r\n                attached: attached,\r\n     " +
"           activate: activate,\r\n                view: view,\r\n                ent" +
"ity: entity,\r\n                toolbar: {\r\n                    commands: ko.obser" +
"vableArray([{\r\n                        caption: \'Publish\',\r\n                    " +
"    icon: \'fa fa-sign-out\',\r\n                        command: publish,\r\n        " +
"                enable : ko.computed(function() {\r\n                            r" +
"eturn view().EntityViewId() > 0;\r\n                        })\r\n                  " +
"  }\r\n                    ]),\r\n                    saveCommand: save\r\n           " +
"     }\r\n            };\r\n\r\n            return vm;\r\n\r\n        });\r\n\r\n\r\n</script>\r\n" +
"");

        }
    }
}
#pragma warning restore 1591
