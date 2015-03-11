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
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/EditorTemplates/EntityLookupElement.cshtml")]
    public partial class EntityLookupElement_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.EntityLookupElement>
    {
        public EntityLookupElement_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if: ko.unwrap($type) === \"Bespoke.Sph.Domain.EntityLookupElement, domain.s" +
"ph\" -->\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"fe-ele-entity\"");

WriteLiteral(">Entity</label>\r\n    <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Entity,\r\n                options : $root.entityOptions,\r\n     " +
"           optionsValue : \'value\',\r\n                optionsText : \'text\',\r\n     " +
"           optionsCaption: \'[Select Entity]\'\"");

WriteLiteral(" id=\"fe-ele-entity\"");

WriteLiteral(" name=\"entity\"");

WriteLiteral("></select>\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"fe-ele-vmp\"");

WriteLiteral(">Value Member Path</label>\r\n    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: ValueMemberPath,entityTypeaheadPath : Entity\"");

WriteLiteral(" id=\"fe-ele-vmp\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"ValueMemberPath\"");

WriteLiteral(" />\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"fe-ele-dmp\"");

WriteLiteral(">Display Member Path</label>\r\n    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: DisplayMemberPath,entityTypeaheadPath:Entity\"");

WriteLiteral(" id=\"fe-ele-dmp\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"DisplayMemberPath\"");

WriteLiteral(" />\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"fe-ele-dt\"");

WriteLiteral(">Display Template</label>\r\n    <textarea");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: DisplayTemplate\"");

WriteLiteral(" id=\"fe-ele-dt\"");

WriteLiteral(" name=\"DispalyTemplate\"");

WriteLiteral("></textarea>\r\n\r\n    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : editDisplayTemplate,disable:DisplayMemberPath()\"");

WriteLiteral(">Edit</a>\r\n</div>\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"fe-ele-cols\"");

WriteLiteral(">Columns</label>\r\n    <span");

WriteLiteral(" data-bind=\"value: LookupColumnCollection\"");

WriteLiteral(" id=\"fe-ele-cols\"");

WriteLiteral("></span>\r\n\r\n    <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : editColumns\"");

WriteLiteral(">Edit</a>\r\n</div>\r\n\r\n\r\n\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591