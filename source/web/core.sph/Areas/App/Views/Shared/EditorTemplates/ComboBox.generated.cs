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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/EditorTemplates/ComboBox.cshtml")]
    public partial class ComboBox : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ComboBox>
    {
        public ComboBox()
        {
        }
        public override void Execute()
        {

WriteLiteral("\r\n<!--ko if: ko.unwrap($type) === \"Bespoke.Sph.Domain.ComboBox, domain.sph\" -->\r\n" +
"<a href=\"#\" data-bind=\"click: addItem\">+ option</a>\r\n<div class=\"row\">\r\n    <spa" +
"n class=\"col-lg-5\">Value</span>\r\n    <span class=\"col-lg-5\">Caption</span>\r\n</di" +
"v>\r\n\r\n<!-- ko foreach : ComboBoxItemCollection -->\r\n\r\n<div class=\"row\">\r\n    <in" +
"put class=\"col-lg-5\" type=\"text\" data-bind=\"value: Value, valueUpdate: \'keyup\'\" " +
"/>\r\n    <input class=\"col-lg-5\" type=\"text\" data-bind=\"value: Caption, valueUpda" +
"te: \'keyup\'\" />\r\n    <a href=\"#\" data-bind=\"click: $parent.removeItem.call($pare" +
"nt, $data)\">\r\n        <i class=\"glyphicon glyphicon-remove\"></i>\r\n    </a>\r\n</di" +
"v>\r\n\r\n\r\n<!-- /ko -->\r\n\r\n\r\n<!-- ko with : ComboBoxLookup -->\r\n\r\n\r\n<div class=\"for" +
"m-group\">\r\n    <label>Lookup Entity</label>\r\n    <input type=\"text\" class=\"form-" +
"control\" data-bind=\"value:Entity, tooltip :\'The entity name for the lookup items" +
"\'\" />\r\n</div>\r\n<div class=\"form-group\">\r\n    <label>Value Path</label>\r\n    <inp" +
"ut type=\"text\" class=\"form-control\" data-bind=\"value:ValuePath, tooltip :\'The me" +
"mber name of the entity to be the value of the combobox item\'\" />\r\n</div>\r\n\r\n<di" +
"v class=\"form-group\">\r\n    <label>Display Path</label>\r\n    <input type=\"text\" c" +
"lass=\"form-control\" data-bind=\"value:DisplayPath, tooltip :\'The member name of t" +
"he entity to be the caption of the combobox item\'\" />\r\n</div>\r\n<div class=\"form-" +
"group\">\r\n    <label for=\"IsComputedQuery\">Computed Query</label>\r\n    <input id=" +
"\"IsComputedQuery\" type=\"checkbox\" data-bind=\"checked:IsComputedQuery, tooltip :\'" +
"If you need to update the options when the Query value changed\'\" />\r\n    \r\n</div" +
">\r\n\r\n\r\n\r\n<div class=\"form-group\">\r\n    <label>Query</label>\r\n    <input type=\"te" +
"xt\" class=\"form-control\" data-bind=\"value:Query, tooltip :\'The filter query for " +
"the items from entity, if you need the query to be dynamically evaluated, check " +
"the ComputedQuery\'\" />\r\n</div>\r\n\r\n<!-- /ko -->\r\n<!--/ko-->\r\n");


        }
    }
}
#pragma warning restore 1591
