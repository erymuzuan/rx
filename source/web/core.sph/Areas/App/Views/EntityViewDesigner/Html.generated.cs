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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Areas\App\Views\EntityViewDesigner\Html.cshtml"
    using Bespoke.Sph.Web.Models;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityViewDesigner/Html.cshtml")]
    public partial class Html : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Html()
        {
        }
        public override void Execute()
        {


            
            #line 2 "..\..\Areas\App\Views\EntityViewDesigner\Html.cshtml"
  
    Layout = null;


            
            #line default
            #line hidden
WriteLiteral("<h1 data-bind=\"with : entity\">View Designer for <span data-bind=\"text:Plural\"></s" +
"pan></h1>\r\n<div id=\"error-list-entity-view\" class=\"row\" data-bind=\"visible:error" +
"s().length\">\r\n    <!-- ko foreach : errors -->\r\n    <div class=\"col-lg-8 col-lg-" +
"offset-2 alert alert-dismissable alert-danger\">\r\n        <button type=\"button\" c" +
"lass=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>\r\n        <" +
"i class=\"fa fa-exclamation\"></i>\r\n        <span data-bind=\"text:Message\"></span>" +
"\r\n        <!-- ko if : Code -->\r\n        <strong class=\"icon-read-more\" data-bin" +
"d=\"bootstrapPopover : Code\"> ..more</strong>\r\n        <!-- /ko-->\r\n    </div>\r\n " +
"   <div class=\"col-lg-2\"></div>\r\n    <!-- /ko-->\r\n</div>\r\n<div class=\"row\">\r\n   " +
" <a data-toggle=\"collapse\" href=\"#general-ev-panel\">\r\n        <h3>\r\n            " +
"<i class=\"fa fa-chevron-down\"></i>\r\n            General options\r\n        </h3>\r\n" +
"    </a>\r\n    <div class=\"collapse in\" id=\"general-ev-panel\">\r\n\r\n        <form c" +
"lass=\"form-horizontal\" data-bind=\"with : view\">\r\n            <div class=\"form-gr" +
"oup\">\r\n                <label for=\"view-name\" class=\"col-lg-2 control-label\">Nam" +
"e</label>\r\n                <div class=\"col-lg-6\">\r\n                    <input ty" +
"pe=\"text\" data-bind=\"value: Name, valueUpdate: \'keyup\'\"\r\n                       " +
"    required pattern=\"^[A-Za-z_][A-Za-z0-9_ ]*$\"\r\n                           pla" +
"ceholder=\"Name for the view\"\r\n                           class=\"form-control\" id" +
"=\"view-name\">\r\n                </div>\r\n            </div>\r\n            <div clas" +
"s=\"form-group\">\r\n                <label for=\"view-route\" class=\"col-lg-2 control" +
"-label\">Route</label>\r\n                <div class=\"col-lg-6\">\r\n                 " +
"   <input type=\"text\" data-bind=\"value: Route\"\r\n                           requi" +
"red pattern=\"^[A-Za-z_][A-Za-z0-9_-]*$\"\r\n                           placeholder=" +
"\"route url\"\r\n                           class=\"form-control\" id=\"view-route\">\r\n " +
"               </div>\r\n            </div>\r\n            <div class=\"form-group\">\r" +
"\n                <label for=\"view-note\" class=\"col-lg-2 control-label\">Note</lab" +
"el>\r\n                <div class=\"col-lg-6\">\r\n                    <textarea data-" +
"bind=\"value: Note, valueUpdate: \'keyup\'\"\r\n                              placehol" +
"der=\"Note about the view\"\r\n                              class=\"form-control\" id" +
"=\"view-note\"></textarea>\r\n                </div>\r\n            </div>\r\n          " +
"  <div class=\"form-group\">\r\n                <label for=\"icon-storeid\" class=\"col" +
"-lg-2 control-label\">Icon/Logo</label>\r\n                <div class=\"col-lg-6\">\r\n" +
"\r\n                    <div data-bind=\"attr : {\'class\':TileColour}\" style=\"paddin" +
"g: 10px;\">\r\n                        <div class=\"pull-left\">\r\n                   " +
"         <img data-bind=\"attr:{src: \'/sph/image/store/\' + IconStoreId() }\" alt=\"" +
"Icon\">\r\n                        </div>\r\n                        <div>\r\n         " +
"                   <span style=\"font-size: 32px; font-weight: bold; margin: 5px\"" +
">2631</span>\r\n                        </div>\r\n\r\n                        <div>\r\n " +
"                           <h5 data-bind=\"text: Name\"></h5>\r\n                   " +
"         <label data-bind=\"text: Note\"></label>\r\n                        </div>\r" +
"\n                    </div>\r\n\r\n                    <input type=\"file\" data-bind=" +
"\"kendoUpload: IconStoreId\" name=\"files\" class=\"form-control\" id=\"icon-storeid\">\r" +
"\n                </div>\r\n            </div>\r\n\r\n            <div class=\"form-grou" +
"p\">\r\n                <label for=\"ev-tl-color\" class=\"col-lg-2 control-label\">Til" +
"e Color</label>\r\n                <div class=\"col-lg-6\">\r\n                    <se" +
"lect class=\"form-control\" id=\"ev-tl-color\" data-bind=\"value:TileColour\">\r\n      " +
"                  <option value=\"bblue\">Blue</option>\r\n                        <" +
"option value=\"blightblue\">Light blue</option>\r\n                        <option v" +
"alue=\"bred\">Red</option>\r\n                        <option value=\"bgreen\">Green</" +
"option>\r\n                        <option value=\"borange\">Orange</option>\r\n      " +
"                  <option value=\"bviolet\">Violet</option>\r\n                    <" +
"/select>\r\n                </div>\r\n            </div>\r\n        </form>\r\n    </div" +
">\r\n</div>\r\n<div class=\"row\" data-bind=\"with : view\">\r\n    <a data-toggle=\"collap" +
"se\" href=\"#filters-ev-panel\">\r\n        <h3>\r\n            <i class=\"fa fa-chevron" +
"-down\"></i>\r\n            Filters\r\n        </h3>\r\n    </a>\r\n\r\n    <div class=\"col" +
"lapse collapsed\" id=\"filters-ev-panel\">\r\n        <table class=\"table table-strip" +
"ed\">\r\n            <thead>\r\n                <tr>\r\n                    <th>Term</t" +
"h>\r\n                    <th>Operator</th>\r\n                    <th>Value</th>\r\n " +
"                   <th></th>\r\n                </tr>\r\n            </thead>\r\n     " +
"       <tbody data-bind=\"foreach :FilterCollection\">\r\n                <tr>\r\n    " +
"                <td>\r\n                        <input class=\"form-control\" type=\"" +
"text\"\r\n                               data-bind=\"value:Term,entityTypeaheadPath " +
":$root.entity().EntityDefinitionId()\"\r\n                               required p" +
"attern=\"^[A-Za-z][A-Za-z0-9_.]*$\" />\r\n                    </td>\r\n               " +
"     <td>\r\n                        <select name=\"rule-operator\" class=\"form-cont" +
"rol\" data-bind=\"value: Operator\">\r\n                            <option value=\"Eq" +
"\">=</option>\r\n                            <option value=\"Neq\">!=</option>\r\n     " +
"                       <option value=\"Le\">&lt;=</option>\r\n                      " +
"      <option value=\"Lt\">&lt;</option>\r\n                            <option valu" +
"e=\"Ge\">&gt;=</option>\r\n                            <option value=\"Gt\">&gt;</opti" +
"on>\r\n                            <option value=\"Substringof\">Substringof</option" +
">\r\n                            <option value=\"StartsWith\">StartsWith</option>\r\n " +
"                           <option value=\"EndsWith\">EndsWith</option>\r\n         " +
"                   <option value=\"NotContains\">Not Substringof</option>\r\n       " +
"                     <option value=\"NotStartsWith\">Not StartsWith</option>\r\n    " +
"                        <option value=\"NotEndsWith\">Not EndsWith</option>\r\n     " +
"                   </select>\r\n                    </td>\r\n                    <td" +
">                    ");


            
            #line 134 "..\..\Areas\App\Views\EntityViewDesigner\Html.cshtml"
                                       Write(Html.Partial("_TriggerFieldDropDown", new TypeModel { Path = "Field" }));

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                    <td>\r\n                        <a href=\"#\" data-bind=\"c" +
"lick : $parent.removeFilter.call($parent,$data)\">\r\n                            <" +
"span class=\"glyphicon glyphicon-remove\"></span>\r\n                        </a>\r\n " +
"                   </td>\r\n                </tr>\r\n            </tbody>\r\n        <" +
"/table>\r\n\r\n\r\n        <a class=\"btn btn-link\" data-bind=\"click : addFilter\">\r\n   " +
"         <i class=\"fa fa-plus-circle\"></i> Add a filter\r\n        </a>\r\n    </div" +
">\r\n</div>\r\n\r\n\r\n<div class=\"row\" data-bind=\"with : view\">\r\n    <a data-toggle=\"co" +
"llapse\" href=\"#sorts-ev-panel\">\r\n        <h3>\r\n            <i class=\"fa fa-chevr" +
"on-down\"></i>\r\n            Sorts\r\n        </h3>\r\n    </a>\r\n\r\n    <div class=\"col" +
"lapse collapsed\" id=\"sorts-ev-panel\">\r\n        <table class=\"table table-striped" +
"\">\r\n            <thead>\r\n                <tr>\r\n                    <th>Path</th>" +
"\r\n                    <th>Direction</th>\r\n                    <th></th>\r\n       " +
"         </tr>\r\n            </thead>\r\n            <tbody data-bind=\"foreach :Sor" +
"tCollection\">\r\n                <tr>\r\n                    <td>\r\n                 " +
"       <input class=\"form-control\" type=\"text\"\r\n                               d" +
"ata-bind=\"value:Path,entityTypeaheadPath :$root.entity().EntityDefinitionId()\"\r\n" +
"                               required pattern=\"^[A-Za-z][A-Za-z0-9_.]*$\" />\r\n " +
"                   </td>\r\n                    <td>\r\n                        <sel" +
"ect name=\"rule-operator\" class=\"form-control\" data-bind=\"value: Direction\">\r\n   " +
"                         <option value=\"Asc\">Ascending</option>\r\n               " +
"             <option value=\"Desc\">Desc</option>\r\n                        </selec" +
"t>\r\n                    </td>\r\n                    <td>\r\n                       " +
" <a href=\"#\" data-bind=\"click : $parent.removeSort.call($parent,$data)\">\r\n      " +
"                      <span class=\"glyphicon glyphicon-remove\"></span>\r\n        " +
"                </a>\r\n                    </td>\r\n                </tr>\r\n        " +
"    </tbody>\r\n        </table>\r\n\r\n        <a class=\"btn btn-link\" data-bind=\"cli" +
"ck : addSort\">\r\n            <i class=\"fa fa-plus-circle\"></i>\r\n            Add a" +
" sort\r\n        </a>\r\n    </div>\r\n</div>\r\n\r\n<div>\r\n\r\n    <!-- ko compose : {model" +
": \'viewmodels/_view.columns.designer\', activationData:$root.view()}-->\r\n    <!--" +
"/ko-->\r\n\r\n</div>\r\n<div class=\"row\" data-bind=\"with : view\">\r\n    <a data-toggle=" +
"\"collapse\" href=\"#cf-ev-panel\">\r\n        <h3>\r\n            <i class=\"fa fa-chevr" +
"on-down\"></i>\r\n            Conditional Formatting\r\n        </h3>\r\n    </a>\r\n\r\n  " +
"  <div class=\"collapse collapsed\" id=\"cf-ev-panel\">\r\n        <table class=\"table" +
" table-striped\">\r\n            <thead>\r\n                <tr>\r\n                   " +
" <th>Css Class</th>\r\n                    <th>Condition</th>\r\n                   " +
" <th></th>\r\n                </tr>\r\n            </thead>\r\n            <tbody data" +
"-bind=\"foreach :ConditionalFormattingCollection\">\r\n                <tr>\r\n       " +
"             <td>\r\n                        <select name=\"rule-operator\" class=\"f" +
"orm-control\" data-bind=\"value: CssClass\">\r\n                            <option v" +
"alue=\"active\">active</option>\r\n                            <option value=\"succes" +
"s\">success</option>\r\n                            <option value=\"warning\">warning" +
"</option>\r\n                            <option value=\"danger\">danger</option>\r\n " +
"                           <option value=\"info\">info</option>\r\n                 " +
"       </select>\r\n                    </td>\r\n                    <td>\r\n         " +
"               <input type=\"text\" required class=\"form-control\" data-bind=\"value" +
":Condition\" />\r\n                    </td>\r\n                    <td>\r\n           " +
"             <a href=\"#\" data-bind=\"click : $parent.removeConditionalFormatting." +
"call($parent,$data)\">\r\n                            <span class=\"fa fa-times\"></s" +
"pan>\r\n                        </a>\r\n                    </td>\r\n                <" +
"/tr>\r\n            </tbody>\r\n        </table>\r\n\r\n        <a class=\"btn btn-link\" " +
"data-bind=\"click : addConditionalFormatting\">\r\n            <i class=\"fa fa-plus-" +
"circle\"></i> Add a conditional formatting\r\n        </a>\r\n    </div>\r\n</div>\r\n\r\n\r" +
"\n<div class=\"row\" style=\"height: 200px;\"></div>");


        }
    }
}
#pragma warning restore 1591
