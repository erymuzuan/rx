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
    
    #line 1 "..\..\Areas\App\Views\Shared\DisplayTemplates\FileUploadElement.cshtml"
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/FileUploadElement.cshtml")]
    public partial class FileUploadElement_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.FileUploadElement>
    {
        public FileUploadElement_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if:$data[\'$type\']() === \"Bespoke.Sph.Domain.FileUploadElement, domain.sph\"" +
" -->\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectForm" +
"Element\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 6 "..\..\Areas\App\Views\Shared\DisplayTemplates\FileUploadElement.cshtml"
Write(Html.Partial("_DesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <label");

WriteLiteral(" data-bind=\"text: Label\"");

WriteLiteral(" class=\"control-label col-lg-2\"");

WriteLiteral(">LABEL</label>\r\n    <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" data-bind=\"attr: { \'class\': CssClass() + \' form-control\' + \' \' + Size(), \'title\'" +
": Tooltip }, text:\' [ \' + Path() + \' ] \'\"");

WriteLiteral("></button>\r\n        <span");

WriteLiteral(" data-bind=\"text: HelpText\"");

WriteLiteral(" class=\"help-block\"");

WriteLiteral("></span>\r\n    </div>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
