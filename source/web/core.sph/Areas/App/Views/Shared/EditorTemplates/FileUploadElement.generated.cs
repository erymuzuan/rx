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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/EditorTemplates/FileUploadElement.cshtml")]
    public partial class FileUploadElement_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.FileUploadElement>
    {
        public FileUploadElement_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if: ko.unwrap($type) === \"Bespoke.Sph.Domain.FileUploadElement, domain.sph" +
"\" -->\r\n\r\n<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"upload-extensions\"");

WriteLiteral(">Allowed  file extensions</label>\r\n    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: AllowedExtensions\"");

WriteLiteral("\r\n           placeholder=\"For more than one  extension use , e.g. .docx,.xlsx\"");

WriteLiteral("\r\n           id=\"upload-extensions\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"CommandName\"");

WriteLiteral(" />\r\n</div>\r\n\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591
