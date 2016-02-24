﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/_TriggerFieldDropDown.cshtml")]
    public partial class _Areas_App_Views_Shared__TriggerFieldDropDown_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.Models.TypeModel>
    {
        public _Areas_App_Views_Shared__TriggerFieldDropDown_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
  
    var fieldTypes = System.Configuration.ConfigurationManager.AppSettings["rx:TriggerFilter:RuleFields"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"btn-group\"");

WriteLiteral(">\r\n    <a");

WriteLiteral(" title=\"Click to specify your value\"");

WriteLiteral(" data-bind=\"with : ");

            
            #line 7 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                                                        Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral(", bootstrapDropDown : \'[Select a field]\'\"");

WriteLiteral(" class=\"btn btn-link dropdown\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(">\r\n        <img");

WriteLiteral(" data-bind=\"fieldImage : $type\"");

WriteLiteral(" class=\"pull-left\"");

WriteLiteral(" alt=\".\"");

WriteLiteral(" />\r\n        <!-- ko text : Name -->\r\n        <!-- /ko -->\r\n        <!-- ko ifnot" +
" : Name -->\r\n           Your field\r\n        <!-- /ko -->\r\n        <i");

WriteLiteral(" class=\"fa fa-caret-down\"");

WriteLiteral("></i>\r\n    </a>\r\n    <ul");

WriteLiteral(" class=\"dropdown-menu\"");

WriteLiteral(">\r\n        <li");

WriteLiteral(" data-bind=\"with : ");

            
            #line 17 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                         Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n            <a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"visible: Name() !== \'+ Field\',unwrapClick: $parent.editField.call($pa" +
"rent, $data, $parent.");

            
            #line 18 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                                                                                                                                    Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral(")\"");

WriteLiteral(">\r\n                <i");

WriteLiteral(" class=\"fa fa-edit\"");

WriteLiteral("></i>\r\n                Edit Field\r\n            </a>\r\n        </li>\r\n\r\n");

            
            #line 24 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
        
            
            #line default
            #line hidden
            
            #line 24 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
         foreach (var type in fieldTypes)
        {

            
            #line default
            #line hidden
WriteLiteral("            <li>\r\n                <a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" data-bind=\"unwrapClick: addField, field : \'");

            
            #line 27 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                                                                              Write(type);

            
            #line default
            #line hidden
WriteLiteral("\', accessor : $data.");

            
            #line 27 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                                                                                                       Write(Model.Path);

            
            #line default
            #line hidden
WriteLiteral(", entity: $root.typeaheadEntity\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(">\r\n                    <img");

WriteAttribute("src", Tuple.Create(" src=\"", 1254), Tuple.Create("\"", 1289)
            
            #line 28 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
, Tuple.Create(Tuple.Create("", 1260), Tuple.Create<System.Object, System.Int32>($"/images/{type}Field.png"
            
            #line default
            #line hidden
, 1260), false)
);

WriteLiteral(" class=\"pull-left\"");

WriteAttribute("alt", Tuple.Create(" alt=\"", 1308), Tuple.Create("\"", 1319)
            
            #line 28 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
    , Tuple.Create(Tuple.Create("", 1314), Tuple.Create<System.Object, System.Int32>(type
            
            #line default
            #line hidden
, 1314), false)
);

WriteLiteral("/>\r\n                    + ");

            
            #line 29 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
                 Write(type);

            
            #line default
            #line hidden
WriteLiteral("\r\n                </a>\r\n            </li>\r\n");

            
            #line 32 "..\..\Areas\App\Views\Shared\_TriggerFieldDropDown.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </ul>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
