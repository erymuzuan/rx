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

namespace Bespoke.Sph.Web.Areas.App.Views.TriggerSetup
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
    
    #line 1 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    #line 2 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
    using Bespoke.Sph.Web.Models;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/TriggerSetup/Html.cshtml")]
    public partial class Html : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Html()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1>Trigger</h1>\r\n<form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(" data-bind=\"with: trigger\"");

WriteLiteral(" role=\"form\"");

WriteLiteral(" style=\"clear: both\"");

WriteLiteral(">\r\n\r\n\r\n    <a");

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral(" href=\"#general-trigger-panel\"");

WriteLiteral(">\r\n        <h3>\r\n            <i");

WriteLiteral(" class=\"fa fa-chevron-down\"");

WriteLiteral("></i>\r\n            General options\r\n        </h3>\r\n    </a>\r\n\r\n    <div");

WriteLiteral(" class=\"collapse in\"");

WriteLiteral(" id=\"general-trigger-panel\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"Name\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Name</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                <input required");

WriteLiteral(" placeholder=\"Trigger\'s\' name\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Name\"");

WriteLiteral(" id=\"Name\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Name\"");

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"Entity\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Entity</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                <select required");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Entity, options: $root.entities, optionsCaption:\'[--SELECT TRI" +
"GGER ENTITY--]\'\"");

WriteLiteral(" id=\"Entity\"");

WriteLiteral(" name=\"Entity\"");

WriteLiteral("></select>\r\n            </div>\r\n        </div>\r\n        ");

WriteLiteral("\r\n\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"Note\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Note</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                <textarea");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Note\"");

WriteLiteral(" id=\"Note\"");

WriteLiteral(" name=\"Note\"");

WriteLiteral("></textarea>\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n            <label>\r\n                <input");

WriteLiteral(" data-bind=\"checked: IsActive\"");

WriteLiteral(" id=\"isActive\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsActive\"");

WriteLiteral(" />\r\n                Active\r\n            </label>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"checkbox pull-left\"");

WriteLiteral(">\r\n            <label>\r\n                <input");

WriteLiteral(" data-bind=\"checked: IsFiredOnAdded\"");

WriteLiteral(" id=\"fired-on-added\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsFiredOnAdded\"");

WriteLiteral(" />\r\n                New item added\r\n            </label>\r\n        </div>\r\n      " +
"  <div");

WriteLiteral(" class=\"checkbox pull-left\"");

WriteLiteral(">\r\n            <label>\r\n                <input");

WriteLiteral(" data-bind=\"checked: IsFiredOnChanged\"");

WriteLiteral(" id=\"fired-on-changed\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsFireOnChanged\"");

WriteLiteral(" />\r\n                When item has changed\r\n            </label>\r\n        </div>\r" +
"\n        <div");

WriteLiteral(" class=\"checkbox pull-left\"");

WriteLiteral(">\r\n            <label>\r\n                <input");

WriteLiteral(" data-bind=\"checked: IsFiredOnDeleted\"");

WriteLiteral(" id=\"fire-on-deleted\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsFireOnDeleted\"");

WriteLiteral(" />\r\n                When the item has been deleted\r\n            </label>\r\n      " +
"  </div>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group clear clearfix\"");

WriteLiteral(" style=\"clear: both\"");

WriteLiteral(">\r\n            <label");

WriteLiteral(" for=\"fired-on-operations\"");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">Operation</label>\r\n            <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: FiredOnOperations\"");

WriteLiteral(" id=\"fired-on-operations\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"FireOnOperations\"");

WriteLiteral(" />\r\n                <span");

WriteLiteral(" class=\"help-block\"");

WriteLiteral(">When this operation is invoked. Use \",\" to add more than 1 operations</span>\r\n  " +
"          </div>\r\n        </div>\r\n    </div>\r\n\r\n    <a");

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral(" href=\"#rules-panel\"");

WriteLiteral(">\r\n        <h3>\r\n            <i");

WriteLiteral(" class=\"fa fa-chevron-down\"");

WriteLiteral("></i>\r\n            Rules\r\n        </h3>\r\n    </a>\r\n    <div");

WriteLiteral(" id=\"rules-panel\"");

WriteLiteral(" class=\"collapsed collapse\"");

WriteLiteral(">\r\n        <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"pull-right\"");

WriteLiteral(" data-bind=\"click: addRule\"");

WriteLiteral(">+ Rule</a>\r\n        <table");

WriteLiteral(" id=\"rules-table\"");

WriteLiteral(" class=\"table table-striped\"");

WriteLiteral(">\r\n            <thead>\r\n                <tr>\r\n                    <th>Left</th>\r\n" +
"                    <th>Operator</th>\r\n                    <th>Right</th>\r\n     " +
"               <th></th>\r\n                </tr>\r\n            </thead>\r\n         " +
"   <tbody");

WriteLiteral(" data-bind=\"foreach: RuleCollection\"");

WriteLiteral(">\r\n                <tr>\r\n                    <td>\r\n");

WriteLiteral("                        ");

            
            #line 95 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
                   Write(Html.Partial("_TriggerFieldDropDown", new TypeModel { Path = "Left" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        <s" +
"elect");

WriteLiteral(" name=\"rule-operator\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Operator\"");

WriteLiteral(">\r\n                            <option");

WriteLiteral(" value=\"Eq\"");

WriteLiteral(">=</option>\r\n                            <option");

WriteLiteral(" value=\"Neq\"");

WriteLiteral(">!=</option>\r\n                            <option");

WriteLiteral(" value=\"Le\"");

WriteLiteral(">&lt;=</option>\r\n                            <option");

WriteLiteral(" value=\"Lt\"");

WriteLiteral(">&lt;</option>\r\n                            <option");

WriteLiteral(" value=\"Ge\"");

WriteLiteral(">&gt;=</option>\r\n                            <option");

WriteLiteral(" value=\"Gt\"");

WriteLiteral(">&gt;</option>\r\n                            <option");

WriteLiteral(" value=\"Substringof\"");

WriteLiteral(">Substringof</option>\r\n                            <option");

WriteLiteral(" value=\"StartsWith\"");

WriteLiteral(">StartsWith</option>\r\n                            <option");

WriteLiteral(" value=\"EndsWith\"");

WriteLiteral(">EndsWith</option>\r\n                        </select>\r\n                    </td>\r" +
"\n                    <td>\r\n");

WriteLiteral("                        ");

            
            #line 111 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
                   Write(Html.Partial("_TriggerFieldDropDown", new TypeModel { Path = "Right" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        <a" +
"");

WriteLiteral(" rel=\"nofollow\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : $parent.removeRule.call($parent,$data)\"");

WriteLiteral(">\r\n                            Remove\r\n                        </a>\r\n            " +
"        </td>\r\n                </tr>\r\n            </tbody>\r\n        </table>\r\n  " +
"  </div>\r\n\r\n    <a");

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral(" href=\"#actions-panel\"");

WriteLiteral(">\r\n        <h3>\r\n            <i");

WriteLiteral(" class=\"fa fa-chevron-down\"");

WriteLiteral("></i>\r\n\r\n            Action\r\n        </h3>\r\n    </a>\r\n\r\n    <div");

WriteLiteral(" id=\"actions-panel\"");

WriteLiteral(" class=\"collapsed collapse\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" id=\"action-panel\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"btn-group\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" class=\"btn btn-link dropdown-toggle\"");

WriteLiteral(" data-toggle=\"drop-down\"");

WriteLiteral(">\r\n                    Add Action &nbsp;<span");

WriteLiteral(" class=\"caret\"");

WriteLiteral("></span>\r\n                </a>\r\n                <ul");

WriteLiteral(" class=\"dropdown-menu\"");

WriteLiteral(">\r\n                    <li>\r\n                        <a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"click: addAction(\'Email\')\"");

WriteLiteral(">\r\n                            <i");

WriteLiteral(" class=\"fa fa-envelope\"");

WriteLiteral("></i>\r\n                            Add Email Action\r\n                        </a>" +
"\r\n                    </li>\r\n                    <li>\r\n                        <" +
"a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"click: addAction(\'Setter\')\"");

WriteLiteral(">\r\n                            <i");

WriteLiteral(" class=\"fa fa-gear\"");

WriteLiteral("></i>\r\n                            Add Setter Action\r\n                        </a" +
">\r\n                    </li>\r\n                    <li>\r\n                        " +
"<a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"click: addAction(\'StartWorkflow\')\"");

WriteLiteral(">\r\n                            <i");

WriteLiteral(" class=\"fa fa-gears\"");

WriteLiteral("></i>\r\n                            Start Workflow\r\n                        </a>\r\n" +
"                    </li>\r\n\r\n                </ul>\r\n            </div>\r\n\r\n      " +
"  </div>\r\n        <table");

WriteLiteral(" class=\"table table-striped\"");

WriteLiteral(">\r\n            <thead>\r\n                <tr>\r\n                    <th>Title</th>\r" +
"\n                    <th>Note</th>\r\n                    <th>Is active</th>\r\n    " +
"                <th></th>\r\n                </tr>\r\n            </thead>\r\n        " +
"    <tbody");

WriteLiteral(" data-bind=\"foreach: ActionCollection\"");

WriteLiteral(">\r\n                <tr>\r\n                    <td>\r\n                        <a");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"click : $parent.editAction.call($parent,$data)\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(">\r\n\r\n                            <img");

WriteLiteral(" data-bind=\"fieldImage : $type\"");

WriteLiteral(" class=\"pull-left\"");

WriteLiteral(" alt=\".\"");

WriteLiteral(" />\r\n                            &nbsp;\r\n                            <!-- ko text" +
" : Title -->\r\n                            <!-- /ko -->\r\n                        " +
"</a>\r\n                    </td>\r\n                    <td>\r\n                     " +
"   <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" class=\"input-action-note form-control\"");

WriteLiteral(" data-bind=\"value: Note\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td>\r\n                       " +
" <input");

WriteLiteral(" class=\"input-action-isactive\"");

WriteLiteral(" data-bind=\"checked: IsActive\"");

WriteLiteral(" id=\"IsActive\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"IsActive\"");

WriteLiteral(" />\r\n                    </td>\r\n                    <td>\r\n                       " +
" <a");

WriteLiteral(" class=\"btn btn-mini\"");

WriteLiteral(" rel=\"nofollow\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click: $parent.removeAction.call($parent,$data)\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-times\"");

WriteLiteral("></i></a>\r\n                    </td>\r\n                </tr>\r\n            </tbody>" +
"\r\n        </table>\r\n    </div>\r\n</form>\r\n");

        }
    }
}
#pragma warning restore 1591
