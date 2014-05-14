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

namespace Bespoke.Sph.Web.Areas.App.Views.Shared
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/_TriggerFieldDropDown.cshtml")]
    public partial class TriggerFieldDropDown : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.Models.TypeModel>
    {
        public TriggerFieldDropDown()
        {
        }
        public override void Execute()
        {


            
            #line 2 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
  
    var fieldTypes = System.Configuration.ConfigurationManager.AppSettings["RuleFields"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


            
            #line default
            #line hidden
WriteLiteral("\r\n<div class=\"btn-group\">\r\n    <a title=\"Click to specify your value\" data-bind=\"" +
"with : ");


            
            #line 7 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                                                        Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral(@", bootstrapDropDown : '[Select a field]'"" class=""btn btn-link dropdown"" href=""#"">
        <img data-bind=""fieldImage : $type"" class=""pull-left"" alt=""."" />
        <!-- ko text : Name -->
        <!-- /ko -->
        <!-- ko ifnot : Name -->
           Your field
        <!-- /ko -->
        <i class=""fa fa-caret-down""></i>
    </a>
    <ul class=""dropdown-menu"">
        <li data-bind=""with : ");


            
            #line 17 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                         Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\">\r\n            <a class=\"btn btn-link\" data-bind=\"visible: Name() !== \'+ Field\'," +
"unwrapClick: $parent.editField.call($parent, $data, $parent.");


            
            #line 18 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                                                                                                                                    Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral(")\">\r\n                <i class=\"fa fa-edit\"></i>\r\n                Edit Field\r\n    " +
"        </a>\r\n        </li>\r\n\r\n");


            
            #line 24 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
         foreach (var type in fieldTypes)
        {

            
            #line default
            #line hidden
WriteLiteral("            <li>\r\n                <a class=\"btn btn-link\" data-bind=\"unwrapClick:" +
" addField, field : \'");


            
            #line 27 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                                                                              Write(type);

            
            #line default
            #line hidden
WriteLiteral("\', accessor : $data.");


            
            #line 27 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                                                                                                       Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral(", entity: $root.typeaheadEntity\" href=\"#\">\r\n                    <img src=\"");


            
            #line 28 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                         Write(string.Format("/images/{0}Field.png",type));

            
            #line default
            #line hidden
WriteLiteral("\" class=\"pull-left\" />\r\n                    + ");


            
            #line 29 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                 Write(type);

            
            #line default
            #line hidden
WriteLiteral("\r\n                </a>\r\n            </li>\r\n");


            
            #line 32 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    </ul>\r\n</div>");


        }
    }
}
#pragma warning restore 1591
