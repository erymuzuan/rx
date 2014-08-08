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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
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
WriteLiteral(@"
<h1>Trigger</h1>
<form class=""form-horizontal"" data-bind=""with: trigger"" role=""form"" style=""clear: both"">


    <a data-toggle=""collapse"" href=""#general-trigger-panel"">
        <h3>
            <i class=""fa fa-chevron-down""></i>
            General options
        </h3>
    </a>

    <div class=""collapse in"" id=""general-trigger-panel"">
        <div class=""form-group"">
            <label for=""Name"" class=""col-sm-2 control-label"">Name</label>
            <div class=""col-sm-6"">
                <input required placeholder=""Trigger's' name"" class=""form-control"" data-bind=""value: Name"" id=""Name"" type=""text"" name=""Name"" />
            </div>
        </div>
        <div class=""form-group"">
            <label for=""Entity"" class=""col-sm-2 control-label"">Entity</label>
            <div class=""col-sm-6"">
                <select required class=""form-control"" data-bind=""value: Entity, options: $root.entities, optionsCaption:'[--SELECT TRIGGER ENTITY--]'"" id=""Entity"" name=""Entity""></select>
            </div>
        </div>
        ");



WriteLiteral("\r\n\r\n\r\n        <div class=\"form-group\">\r\n            <label for=\"Note\" class=\"col-" +
"sm-2 control-label\">Note</label>\r\n            <div class=\"col-sm-6\">\r\n          " +
"      <textarea class=\"form-control\" data-bind=\"value: Note\" id=\"Note\" name=\"Not" +
"e\"></textarea>\r\n            </div>\r\n        </div>\r\n\r\n        <div class=\"form-g" +
"roup\">\r\n            <label class=\"control-label col-sm-2\">CRUD</label>\r\n        " +
"    <div class=\"col-sm-6\">\r\n                <div class=\"checkbox col-sm-3 checkb" +
"ox-no-padding-left\">\r\n                    <label>\r\n                        <inpu" +
"t data-bind=\"checked: IsFiredOnAdded\" id=\"fired-on-added\" type=\"checkbox\" name=\"" +
"IsFiredOnAdded\" />\r\n                        Added\r\n                    </label>\r" +
"\n                </div>\r\n                <div class=\"checkbox col-sm-3 checkbox-" +
"no-padding-left\">\r\n                    <label>\r\n                        <input d" +
"ata-bind=\"checked: IsFiredOnChanged\" id=\"fired-on-changed\" type=\"checkbox\" name=" +
"\"IsFireOnChanged\" />\r\n                        Changed\r\n                    </lab" +
"el>\r\n                </div>\r\n                <div class=\"checkbox col-sm-3 check" +
"box-no-padding-left\">\r\n                    <label>\r\n                        <inp" +
"ut data-bind=\"checked: IsFiredOnDeleted\" id=\"fire-on-deleted\" type=\"checkbox\" na" +
"me=\"IsFireOnDeleted\" />\r\n                        Deleted\r\n                    </" +
"label>\r\n                </div>\r\n            </div>\r\n        </div>\r\n\r\n        <d" +
"iv class=\"form-group clear clearfix\" style=\"clear: both\">\r\n            <label cl" +
"ass=\"control-label col-sm-2\" data-bind=\"tooltip:\'When this operation is invoked," +
" then fire this trigger\'\">Operation</label>\r\n            <div class=\"col-sm-6\" i" +
"d=\"operation-options\">\r\n                <!-- ko foreach: $root.operationOptions " +
"-->\r\n                <div class=\"checkbox col-sm-3 checkbox-no-padding-left\">\r\n " +
"                   <label>\r\n                        <input type=\"checkbox\" data-" +
"bind=\"value:$data, checked: $root.operations\" />\r\n                        <!-- k" +
"o text: $data-->\r\n                        <!-- /ko -->\r\n                    </la" +
"bel>\r\n                </div>\r\n                <!-- /ko -->\r\n            </div>\r\n" +
"        </div>\r\n        <div class=\"form-group\">\r\n\r\n        </div>\r\n    </div>\r\n" +
"\r\n    <a data-toggle=\"collapse\" href=\"#rules-panel\">\r\n        <h3>\r\n            " +
"<i class=\"fa fa-chevron-down\"></i>\r\n            Rules\r\n        </h3>\r\n    </a>\r\n" +
"    <div id=\"rules-panel\" class=\"collapsed collapse\">\r\n        <table id=\"rules-" +
"table\" class=\"table table-striped\">\r\n            <thead>\r\n                <tr>\r\n" +
"                    <th>Left</th>\r\n                    <th>Operator</th>\r\n      " +
"              <th>Right</th>\r\n                    <th></th>\r\n                </t" +
"r>\r\n            </thead>\r\n            <tbody data-bind=\"foreach: RuleCollection\"" +
">\r\n                <tr>\r\n                    <td>\r\n                        ");


            
            #line 103 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
                   Write(Html.Partial("_TriggerFieldDropDown", new TypeModel { Path = "Left" }));

            
            #line default
            #line hidden
WriteLiteral(@"
                    </td>
                    <td>
                        <select name=""rule-operator"" class=""form-control"" data-bind=""value: Operator"">
                            <option value=""Eq"">=</option>
                            <option value=""Neq"">!=</option>
                            <option value=""Le"">&lt;=</option>
                            <option value=""Lt"">&lt;</option>
                            <option value=""Ge"">&gt;=</option>
                            <option value=""Gt"">&gt;</option>
                            <option value=""Substringof"">Substringof</option>
                            <option value=""StartsWith"">StartsWith</option>
                            <option value=""EndsWith"">EndsWith</option>
                            <option value=""NotContains"">Not Substringof</option>
                            <option value=""NotStartsWith"">Not StartsWith</option>
                            <option value=""NotEndsWith"">Not EndsWith</option>
                        </select>
                    </td>
                    <td>
                        ");


            
            #line 122 "..\..\Areas\App\Views\TriggerSetup\Html.cshtml"
                   Write(Html.Partial("_TriggerFieldDropDown", new TypeModel { Path = "Right" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        <a" +
" rel=\"nofollow\" href=\"#\" data-bind=\"click : $parent.removeRule.call($parent,$dat" +
"a)\">\r\n                            <i class=\"fa fa-times\"></i>\r\n                 " +
"       </a>\r\n                    </td>\r\n                </tr>\r\n            </tbo" +
"dy>\r\n        </table>\r\n        <a href=\"#\" class=\"btn btn-link\" data-bind=\"click" +
": addRule\">\r\n\r\n            <i class=\"fa fa-plus-circle\"></i> Add a filter rule\r\n" +
"        </a>\r\n    </div>\r\n\r\n    <a data-toggle=\"collapse\" href=\"#actions-panel\">" +
"\r\n        <h3>\r\n            <i class=\"fa fa-chevron-down\"></i>\r\n\r\n            Ac" +
"tion\r\n        </h3>\r\n    </a>\r\n\r\n    <div id=\"actions-panel\" class=\"collapsed co" +
"llapse\">\r\n        <div id=\"action-panel\">\r\n            <div class=\"btn-group\">\r\n" +
"                <a class=\"btn btn-link dropdown-toggle\" data-toggle=\"drop-down\">" +
"\r\n                    <i class=\"fa fa-plus-circle\"></i>\r\n                    Add" +
" an action &nbsp;<span class=\"caret\"></span>\r\n                </a>\r\n            " +
"    <ul class=\"dropdown-menu\" data-bind=\"foreach :$root.actionOptions\">\r\n       " +
"             <li>\r\n                        <a class=\"btn btn-link\" data-bind=\"cl" +
"ick: $root.trigger().addAction(action.$type)\">\r\n\r\n                            <!" +
"-- ko if :designer.FontAwesomeIcon-->\r\n                            <i  data-bind" +
"=\"attr : {\'class\':\'fa fa-\'+ designer.FontAwesomeIcon}\"></i>\r\n                   " +
"         <!--/ko-->\r\n                            Add <!-- ko text: designer.Name" +
" -->\r\n                            <!-- /ko-->\r\n                        </a>\r\n   " +
"                 </li>\r\n\r\n                </ul>\r\n            </div>\r\n\r\n        <" +
"/div>\r\n        <table class=\"table table-striped\">\r\n            <thead>\r\n       " +
"         <tr>\r\n                    <th>Title</th>\r\n                    <th>Note<" +
"/th>\r\n                    <th>Is active</th>\r\n                    <th></th>\r\n   " +
"             </tr>\r\n            </thead>\r\n            <tbody data-bind=\"foreach:" +
" ActionCollection\">\r\n                <tr>\r\n                    <td>\r\n           " +
"             <a type=\"button\" class=\"btn btn-link\" data-bind=\"click : $parent.ed" +
"itAction.call($parent,$data)\" href=\"#\">\r\n\r\n                            <img data" +
"-bind=\"fieldImage : $type\" class=\"pull-left\" style=\"margin-right: 10px\" alt=\".\" " +
"/>\r\n                            &nbsp;\r\n                            <!-- ko text" +
" : Title -->\r\n                            <!-- /ko -->\r\n                        " +
"</a>\r\n                    </td>\r\n                    <td>\r\n                     " +
"   <input type=\"text\" class=\"input-action-note form-control\" data-bind=\"value: N" +
"ote\" />\r\n                    </td>\r\n                    <td>\r\n                  " +
"      <input class=\"input-action-isactive\" data-bind=\"checked: IsActive\" id=\"IsA" +
"ctive\" type=\"checkbox\" name=\"IsActive\" />\r\n                    </td>\r\n          " +
"          <td>\r\n                        <a class=\"btn btn-mini\" rel=\"nofollow\" h" +
"ref=\"#\" data-bind=\"click: $parent.removeAction.call($parent,$data)\"><i class=\"fa" +
" fa-times\"></i></a>\r\n                    </td>\r\n                </tr>\r\n         " +
"   </tbody>\r\n        </table>\r\n    </div>\r\n</form>\r\n");


        }
    }
}
#pragma warning restore 1591
