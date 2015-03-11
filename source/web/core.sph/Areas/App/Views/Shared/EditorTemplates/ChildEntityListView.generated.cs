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

namespace Bespoke.Sph.Web.Areas.App.Views.Shared.EditorTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/EditorTemplates/ChildEntityListView.cshtml")]
    public partial class ChildEntityListView_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ChildEntityListView>
    {
        public ChildEntityListView_()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n\r\n<!--ko if:ko.unwrap($type) === \"Bespoke.Sph.Domain.ChildEntityListView, domai" +
"n.sph\" -->\r\n\r\n\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"celv-entity\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Entity</label>\r\n\r\n    <select");

WriteLiteral(" data-bind=\"value: Entity, tooltip :\'The child entity name\',\r\n                opt" +
"ions : $root.entityOptions,\r\n                optionsValue : \'value\',\r\n          " +
"      optionsText : \'text\',\r\n                optionsCaption: \'[Select Entity]\'\"");

WriteLiteral("\r\n            required");

WriteLiteral("\r\n            class=\"form-control\"");

WriteLiteral(" id=\"celv-entity\"");

WriteLiteral("></select>\r\n\r\n</div>\r\n\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"celv-query\"");

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">Query</label>\r\n\r\n    <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: Query, tooltip : \'Query text to filter the child items\'\"");

WriteLiteral("\r\n           required");

WriteLiteral(" placeholder=\"Query\"");

WriteLiteral("\r\n           class=\"form-control\"");

WriteLiteral(" id=\"celv-query\"");

WriteLiteral(">\r\n\r\n</div>\r\n<div");

WriteLiteral(" class=\"btn-group\"");

WriteLiteral(">\r\n    <a");

WriteLiteral(" data-bind=\"click : addViewColumn\"");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(">\r\n        +  Column\r\n    </a>\r\n</div>\r\n\r\n\r\n\r\n\r\n<table");

WriteLiteral(" class=\"table table-condensed table-striped\"");

WriteLiteral(">\r\n    <thead>\r\n        <tr>\r\n            <th>Header</th>\r\n            <th>Path</" +
"th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody");

WriteLiteral(" data-bind=\"foreach: ViewColumnCollection\"");

WriteLiteral(">\r\n        <tr>\r\n            <td>\r\n                <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : $parent.editViewColumn.call($parent,$data),text: Header\"");

WriteLiteral("></a>\r\n            </td>\r\n            <td");

WriteLiteral(" data-bind=\"text:Path\"");

WriteLiteral("></td>\r\n            <td>\r\n                <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : $parent.removeViewColumn.call($parent,$data)\"");

WriteLiteral(">\r\n                    <span");

WriteLiteral(" class=\"glyphicon glyphicon-remove\"");

WriteLiteral("></span>\r\n                </a>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n<" +
"/table>\r\n\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591