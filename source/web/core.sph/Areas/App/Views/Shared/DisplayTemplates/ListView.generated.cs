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

namespace Bespoke.Sph.Web.Areas.App.Views.Shared.DisplayTemplates
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
    public partial class ListView_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ListView>
    {
        public ListView_()
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

WriteLiteral(" />\r\n    <input");

WriteLiteral(" data-bind=\"click: $root.selectFormElement,value: Label\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-default pull-right\"");

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

            
            #line 27 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
            
            
            #line default
            #line hidden
            
            #line 27 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
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

WriteAttribute("checked", Tuple.Create(" checked=\"", 2527), Tuple.Create("\"", 2564)
            
            #line 46 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
, Tuple.Create(Tuple.Create("", 2537), Tuple.Create<System.Object, System.Int32>(i % 2 ==0? "checked": ""
            
            #line default
            #line hidden
, 2537), false)
);

WriteLiteral(" />\r\n                        <!--/ko-->\r\n                        <!-- ko if: ko.u" +
"nwrap($type) === \'Bespoke.Sph.Domain.DatePicker, domain.sph\'-->\r\n               " +
"         <div");

WriteLiteral(" class=\"input-group\"");

WriteLiteral(">\r\n                            <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" style=\"width: 80%\"");

WriteLiteral(" type=\"text\"");

WriteAttribute("value", Tuple.Create(" value=\"", 2848), Tuple.Create("\"", 2891)
            
            #line 50 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
              , Tuple.Create(Tuple.Create("", 2856), Tuple.Create<System.Object, System.Int32>(DateTime.Today.ToShortDateString()
            
            #line default
            #line hidden
, 2856), false)
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

WriteAttribute("value", Tuple.Create(" value=\"", 3289), Tuple.Create("\"", 3332)
            
            #line 56 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
              , Tuple.Create(Tuple.Create("", 3297), Tuple.Create<System.Object, System.Int32>(DateTime.Today.ToShortDateString()
            
            #line default
            #line hidden
, 3297), false)
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

WriteAttribute("value", Tuple.Create(" value=\"", 3734), Tuple.Create("\"", 3744)
            
            #line 62 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"
, Tuple.Create(Tuple.Create("", 3742), Tuple.Create<System.Object, System.Int32>(i
            
            #line default
            #line hidden
, 3742), false)
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

            
            #line 80 "..\..\Areas\App\Views\Shared\DisplayTemplates\ListView.cshtml"

            }

            
            #line default
            #line hidden
WriteLiteral("        </tbody>\r\n    </table>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591