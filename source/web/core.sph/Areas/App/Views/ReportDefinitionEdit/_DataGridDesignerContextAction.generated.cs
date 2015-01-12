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

namespace Bespoke.Sph.Web.Areas.App.Views.ReportDefinitionEdit
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ReportDefinitionEdit/_DataGridDesignerContextAction.cshtml")]
    public partial class DataGridDesignerContextAction : System.Web.Mvc.WebViewPage<dynamic>
    {
        public DataGridDesignerContextAction()
        {
        }
        public override void Execute()
        {
WriteLiteral("<section");

WriteLiteral(" class=\"context-action-panel\"");

WriteLiteral(" data-bind=\"visible: isSelected\"");

WriteLiteral(">\r\n    <button");

WriteLiteral(" class=\"btn btn-link btn-context-action\"");

WriteLiteral(">\r\n        <i");

WriteLiteral(" class=\"fa fa-chevron-circle-right\"");

WriteLiteral("></i>\r\n    </button>\r\n\r\n    <div");

WriteLiteral(" class=\"context-action\"");

WriteLiteral(" style=\"width: 720px\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"modal-header\"");

WriteLiteral(">\r\n            <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">&times;</button>\r\n            <span>DataGrid Properties</span>\r\n        </div>\r\n" +
"        <table");

WriteLiteral(" class=\"table table-striped\"");

WriteLiteral(">\r\n            <colgroup>\r\n                <col");

WriteLiteral(" style=\"width: 240px\"");

WriteLiteral(" />\r\n                <col");

WriteLiteral(" style=\"width: 200px\"");

WriteLiteral(" />\r\n                <col");

WriteLiteral(" style=\"width: 100px\"");

WriteLiteral(" />\r\n                <col");

WriteLiteral(" style=\"width: 200px\"");

WriteLiteral(@" />
            </colgroup>
            <thead>
                <tr>
                    <th>Expression</th>
                    <th>Header</th>
                    <th>Format</th>
                    <th>Footer</th>
                    <th></th>
                </tr>
            </thead>
            <tbody");

WriteLiteral(" data-bind=\"foreach: DataGridColumnCollection\"");

WriteLiteral(">\r\n                <tr>\r\n                    <td>\r\n                        <input" +
"");

WriteLiteral(" class=\"input-datagrid-column-expression form-control \"");

WriteLiteral(" data-bind=\"kendoComboBox: Expression, source: $root.dataGridColumnOptions\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td><input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"input-datagrid-column-header form-control\"");

WriteLiteral(" data-bind=\"value: Header\"");

WriteLiteral(" /></td>\r\n                    <td>\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"kendoComboBox:Format, source :ko.observableArray([\'{0:d}\',\'{0:F2}\',\'{" +
"0:g}\', \'{0:c}\'])\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td>\r\n                       " +
" <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"input-datagrid-column-footer form-control\"");

WriteLiteral(" data-bind=\"value: FooterExpression\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td><a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : $parent.removeColumn.call($parent,$data)\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-times\"");

WriteLiteral("></i></a></td>\r\n                </tr>\r\n            </tbody>\r\n        </table>\r\n\r\n" +
"\r\n        <a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"click : addColumn\"");

WriteLiteral(">\r\n            <i");

WriteLiteral(" class=\"fa fa-plus-circle\"");

WriteLiteral("></i>\r\n\r\n            Add a column\r\n        </a>\r\n        <table");

WriteLiteral(" class=\"table table-striped\"");

WriteLiteral(">\r\n            <colgroup>\r\n                <col");

WriteLiteral(" style=\"width: 100px\"");

WriteLiteral(" />\r\n                <col");

WriteLiteral(" style=\"width: 100px\"");

WriteLiteral(" />\r\n                <col");

WriteLiteral(" style=\"width: 30px\"");

WriteLiteral(" />\r\n                <col");

WriteLiteral(" style=\"width: 100px\"");

WriteLiteral(@" />
            </colgroup>
            <thead>
                <tr>
                    <th>Column</th>
                    <th>Text</th>
                    <th>Footer</th>
                    <th></th>
                </tr>
            </thead>
            <tbody");

WriteLiteral(" data-bind=\"foreach: DataGridGroupDefinitionCollection\"");

WriteLiteral(">\r\n                <tr>\r\n                    <td>\r\n                        <input" +
"");

WriteLiteral(" class=\"input-datagrid-group-column form-control \"");

WriteLiteral(" data-bind=\"kendoComboBox: Column, source: $root.columnOptions\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td><input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"form-control input-datagrid-group-expression\"");

WriteLiteral(" data-bind=\"value: Expression\"");

WriteLiteral(" /></td>\r\n                    <td>\r\n                        <input");

WriteLiteral(" class=\"form-control input-datagrid-group-footer\"");

WriteLiteral(" data-bind=\"value: FooterExpression\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td><a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click: $parent.removeDataGridGroupDefinition.call($parent, $data)\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-times\"");

WriteLiteral("></i></a></td>\r\n                </tr>\r\n            </tbody>\r\n        </table>\r\n\r\n" +
"        <a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"click: addDataGridGroupDefinition\"");

WriteLiteral(">\r\n            <i");

WriteLiteral(" class=\"fa fa-plus-circle\"");

WriteLiteral("></i>\r\n            Add a group\r\n        </a>\r\n\r\n        <a");

WriteLiteral(" class=\"btn btn-link pull-right\"");

WriteLiteral(" data-bind=\"click: $root.removeReportItem\"");

WriteLiteral(">\r\n            <i");

WriteLiteral(" class=\"fa fa-times\"");

WriteLiteral("></i>\r\n            Remove\r\n        </a>\r\n\r\n\r\n    </div>\r\n</section>\r\n");

        }
    }
}
#pragma warning restore 1591
