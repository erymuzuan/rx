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
    
    #line 1 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/ListView.cshtml")]
    public partial class _Areas_App_Views_Shared_DisplayTemplates_ListView_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ListView>
    {
        public _Areas_App_Views_Shared_DisplayTemplates_ListView_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n<!--ko if:ko.unwrap($type) === \"Bespoke.Sph.Domain.ListView, domain.sph\" -->\r\n<" +
"div");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 7 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
Write(Html.Partial("_DesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <input");

WriteLiteral(" data-bind=\"click: $root.selectFormElement\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" value=\"Click to select\"");

WriteLiteral(" />\r\n    <table");

WriteLiteral(" class=\"table table-condensed table-striped\"");

WriteLiteral(">\r\n        <thead>\r\n            <tr");

WriteLiteral(" data-bind=\"foreach: ListViewColumnCollection\"");

WriteLiteral(">\r\n                <th");

WriteLiteral(" data-bind=\"with : Input\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : $root.selectFormElement,css: { \'selected-form-element\': isSel" +
"ected }\"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" data-bind=\"text:$parent.Label\"");

WriteLiteral("></span>\r\n                        <i");

WriteLiteral(" class=\"fa fa-check\"");

WriteLiteral("></i>\r\n                        <!-- ko if : typeof isSelected === \"function\" -->\r" +
"\n                        <i");

WriteLiteral(" class=\"fa fa-check\"");

WriteLiteral(" data-bind=\"visible:isSelected\"");

WriteLiteral(" style=\"color:darkblue\"");

WriteLiteral("></i>\r\n                        <!-- /ko -->\r\n\r\n                    </a>\r\n        " +
"        </th>\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n");

            
            #line 26 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
            
            
            #line default
            #line hidden
            
            #line 26 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
             for (int i = 0; i < 5; i++)
            {

            
            #line default
            #line hidden
WriteLiteral("                <tr");

WriteLiteral(" data-bind=\"foreach: ListViewColumnCollection\"");

WriteLiteral(">\r\n                    <!-- ko with : Input -->\r\n                    <td>\r\n      " +
"                  <!-- ko if: ko.unwrap($type) === \'Bespoke.Sph.Domain.TextBox, " +
"domain.sph\'-->\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"attr : {\'placeholder\':\' [ \' + Path() + \' ] \'}\"");

WriteLiteral(" />\r\n                        <!--/ko-->\r\n                        <!-- ko if: ko.u" +
"nwrap($type) === \'Bespoke.Sph.Domain.DownloadLink, domain.sph\'-->\r\n             " +
"           <span>\r\n                            <i");

WriteLiteral(" class=\"fa fa-cloud-download\"");

WriteLiteral(@"></i>
                            <!-- ko text : ' [ ' + Path() + ' ] ' -->
                            <!-- /ko -->
                        </span>
                        <!--/ko-->
                        <!-- ko if: ko.unwrap($type) === 'Bespoke.Sph.Domain.Button, domain.sph'-->
                        <button");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" data-bind=\"attr : {\'class\':CssClass},text: $parent.Label\"");

WriteLiteral(">Button</button>\r\n                        <!--/ko-->\r\n                        <!-" +
"- ko if: ko.unwrap($type) === \'Bespoke.Sph.Domain.CheckBox, domain.sph\'-->\r\n    " +
"                    <input");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" value=\"\"");

WriteAttribute("checked", Tuple.Create(" checked=\"", 2407), Tuple.Create("\"", 2444)
            
            #line 45 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
, Tuple.Create(Tuple.Create("", 2417), Tuple.Create<System.Object, System.Int32>(i % 2 ==0? "checked": ""
            
            #line default
            #line hidden
, 2417), false)
);

WriteLiteral(" />\r\n                        <!--/ko-->\r\n                        <!-- ko if: ko.u" +
"nwrap($type) === \'Bespoke.Sph.Domain.DatePicker, domain.sph\'-->\r\n               " +
"         <div");

WriteLiteral(" class=\"input-group\"");

WriteLiteral(">\r\n                            <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" style=\"width: 80%\"");

WriteLiteral(" type=\"text\"");

WriteAttribute("value", Tuple.Create(" value=\"", 2728), Tuple.Create("\"", 2771)
            
            #line 49 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
              , Tuple.Create(Tuple.Create("", 2736), Tuple.Create<System.Object, System.Int32>(DateTime.Today.ToShortDateString()
            
            #line default
            #line hidden
, 2736), false)
);

WriteLiteral(" />\r\n                            <i");

WriteLiteral(" class=\"fa fa-calendar input-group-addon\"");

WriteLiteral("></i>\r\n                        </div>\r\n                        <!--/ko-->\r\n      " +
"                  <!-- ko if: ko.unwrap($type) === \'Bespoke.Sph.Domain.DateTimeP" +
"icker, domain.sph\'-->\r\n                        <div");

WriteLiteral(" class=\"input-group\"");

WriteLiteral(">\r\n                            <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" style=\"width: 80%\"");

WriteLiteral(" type=\"text\"");

WriteAttribute("value", Tuple.Create(" value=\"", 3169), Tuple.Create("\"", 3212)
            
            #line 55 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
              , Tuple.Create(Tuple.Create("", 3177), Tuple.Create<System.Object, System.Int32>(DateTime.Today.ToShortDateString()
            
            #line default
            #line hidden
, 3177), false)
);

WriteLiteral(" />\r\n                            <i");

WriteLiteral(" class=\"fa fa-calendar input-group-addon\"");

WriteLiteral("></i>\r\n                            <i");

WriteLiteral(" class=\"fa fa-clock-o input-group-addon\"");

WriteLiteral("></i>\r\n                        </div>\r\n                        <!--/ko-->\r\n      " +
"                  <!-- ko if: ko.unwrap($type) === \'Bespoke.Sph.Domain.NumberTex" +
"tBox, domain.sph\'-->\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" type=\"number\"");

WriteAttribute("value", Tuple.Create(" value=\"", 3614), Tuple.Create("\"", 3624)
            
            #line 61 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
, Tuple.Create(Tuple.Create("", 3622), Tuple.Create<System.Object, System.Int32>(i
            
            #line default
            #line hidden
, 3622), false)
);

WriteLiteral(" data-bind=\"attr : {\'placeholder\':\' [ \' + Path() + \' ] \'}\"");

WriteLiteral(" />\r\n                        <!--/ko-->\r\n                        <!-- ko if: ko.u" +
"nwrap($type) === \'Bespoke.Sph.Domain.ComboBox, domain.sph\'-->\r\n                 " +
"       <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(">\r\n                            <option");

WriteLiteral(" data-bind=\"text:\' [ \' + Path() + \' ] \'\"");

WriteLiteral("></option>\r\n                        </select>\r\n                        <!--/ko-->" +
"\r\n                        <!-- ko if: ko.unwrap($type) === \'Bespoke.Sph.Domain.F" +
"ileUploadElement, domain.sph\'-->\r\n                        <input");

WriteLiteral(" type=\"file\"");

WriteLiteral(" />\r\n                        <!--/ko-->\r\n                        <!-- ko if: ko.u" +
"nwrap($type) === \'Bespoke.Sph.Domain.ImageElement, domain.sph\'-->\r\n             " +
"           <img");

WriteLiteral(" src=\"/images/form.element.imageelement.png\"");

WriteLiteral(" alt=\"image\"");

WriteLiteral(" />\r\n                        <!--/ko-->\r\n                    </td>\r\n\r\n           " +
"         <!-- /ko -->\r\n\r\n                </tr>\r\n");

            
            #line 79 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"

            }

            
            #line default
            #line hidden
WriteLiteral("        </tbody>\r\n    </table>\r\n    <a");

WriteLiteral(" data-bind=\"click: $root.selectFormElement\"");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(">\r\n        <i");

WriteLiteral(" class=\"fa fa-plus-circle\"");

WriteLiteral("></i>\r\n        <!-- ko text: Label-->\r\n        <!-- /ko-->\r\n    </a>\r\n</div>\r\n<!-" +
"-/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
