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

namespace Bespoke.Sph.Web.Areas.Sph.Views.EntityDefinition
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
    
    #line 1 "..\..\Areas\Sph\Views\EntityDefinition\BusinessRuleDialog.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Areas\Sph\Views\EntityDefinition\BusinessRuleDialog.cshtml"
    using Bespoke.Sph.Web.Models;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/EntityDefinition/BusinessRuleDialog.cshtml")]
    public partial class BusinessRuleDialog : System.Web.Mvc.WebViewPage<dynamic>
    {
        public BusinessRuleDialog()
        {
        }
        public override void Execute()
        {


WriteLiteral(@"


<section class=""view-model-modal"" id=""rule-dialog"">
    <div class=""modal-dialog"">
        <div class=""modal-content"">

            <div class=""modal-header"">
                <button type=""button"" class=""close"" data-dismiss=""modal"" data-bind=""click : cancelClick"">&times;</button>
                <h3>
                    <i class=""fa fa-list-alt""></i>
                    Business Rule
                </h3>
            </div>
            <div class=""modal-body"" data-bind=""with: rule"">

                <form class=""form-horizontal"" id=""rule-dialog-form"">


                    <div class=""form-group"">
                        <label for=""Name"" class=""col-lg-2 control-label"">Name</label>
                        <div class=""col-lg-6"">
                            <input type=""text"" class=""form-control"" id=""Name"" placeholder=""Rule name...."" data-bind=""value:Name"">
                        </div>
                    </div>
                    <div class=""form-group"">
                        <label for=""Description"" class=""col-lg-2 control-label"">Description</label>
                        <div class=""col-lg-9"">
                            <input type=""text"" class=""form-control"" id=""Description"" placeholder=""Description...."" data-bind=""value:Description"">
                        </div>
                    </div>
                    ");



WriteLiteral(@"

                    <div class=""form-group"">
                        <label for=""br-message"" class=""col-lg-2 control-label"">Message</label>
                        <div class=""col-lg-9"">
                            <input type=""text"" class=""form-control"" id=""br-message"" placeholder=""Message"" data-bind=""value:ErrorMessage"">
                        </div>
                    </div>

                    <h3>

                        Filters
                    </h3>
                    <a class=""btn btn-link pull-right"" data-bind=""click: addFilter"">+ Filter</a>
                    <table id=""filters-table"" class=""table table-striped"">
                        <thead>
                            <tr>
                                <th>Left</th>
                                <th>Operator</th>
                                <th>Right</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody data-bind=""foreach: FilterCollection"">
                            <tr>
                                <td>
                                    ");


            
            #line 65 "..\..\Areas\Sph\Views\EntityDefinition\BusinessRuleDialog.cshtml"
                               Write(Html.Partial("_RuleFieldDropDown", new TypeModel { Path = "Left" }));

            
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
                                        <option value=""Substringof"">Contains</option>
                                        <option value=""StartsWith"">Starts with</option>
                                        <option value=""EndsWith"">Ends with</option>
                                        <option value=""NotContains"">Not Substringof</option>
                                        <option value=""NotStartsWith"">Not StartsWith</option>
                                        <option value=""NotEndsWith"">Not EndsWith</option>
                                    </select>
                                </td>
                                <td>
                                    ");


            
            #line 84 "..\..\Areas\Sph\Views\EntityDefinition\BusinessRuleDialog.cshtml"
                               Write(Html.Partial("_RuleFieldDropDown", new TypeModel { Path = "Right" }));

            
            #line default
            #line hidden
WriteLiteral(@"
                                </td>
                                <td>
                                    <a class=""btn btn-mini"" rel=""nofollow"" href=""#"" data-bind=""click : $parent.removeRule.call($parent,$data)"">
                                        <i class=""fa fa-times""></i>
                                    </a>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <h3>

                        Rules
                    </h3>
                    <a class=""btn btn-link pull-right"" data-bind=""click: addRule"">+ Rule</a>
                    <table id=""rules-table"" class=""table table-striped"">
                        <thead>
                            <tr>
                                <th>Left</th>
                                <th>Operator</th>
                                <th>Right</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody data-bind=""foreach: RuleCollection"">
                            <tr>
                                <td>
                                    ");


            
            #line 112 "..\..\Areas\Sph\Views\EntityDefinition\BusinessRuleDialog.cshtml"
                               Write(Html.Partial("_RuleFieldDropDown", new TypeModel { Path = "Left" }));

            
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
                                        <option value=""Substringof"">Contains</option>
                                        <option value=""StartsWith"">Starts with</option>
                                        <option value=""EndsWith"">Ends with</option>
                                        <option value=""NotContains"">Not Substringof</option>
                                        <option value=""NotStartsWith"">Not StartsWith</option>
                                        <option value=""NotEndsWith"">Not EndsWith</option>
                                    </select>
                                </td>
                                <td>
                                    ");


            
            #line 131 "..\..\Areas\Sph\Views\EntityDefinition\BusinessRuleDialog.cshtml"
                               Write(Html.Partial("_RuleFieldDropDown", new TypeModel { Path = "Right" }));

            
            #line default
            #line hidden
WriteLiteral(@"
                                </td>
                                <td>
                                    <a class=""btn btn-mini"" rel=""nofollow"" href=""#"" data-bind=""click : $parent.removeRule.call($parent,$data)"">
                                        <i class=""fa fa-times""></i>
                                    </a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </form>
            </div>
            <div class=""modal-footer"">
                <input form=""rule-dialog-form"" data-dismiss=""modal"" type=""submit"" class=""btn btn-default"" value=""OK"" data-bind=""click: okClick"" />
                <a href=""#"" class=""btn btn-default"" data-dismiss=""modal"" data-bind=""click : cancelClick"">Cancel</a>
            </div>
        </div>
    </div>
</section>");


        }
    }
}
#pragma warning restore 1591
