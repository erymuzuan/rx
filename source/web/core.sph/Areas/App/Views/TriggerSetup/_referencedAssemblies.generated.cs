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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/TriggerSetup/_referencedAssemblies.cshtml")]
    public partial class _Areas_App_Views_TriggerSetup__referencedAssemblies_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_App_Views_TriggerSetup__referencedAssemblies_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("<table");

WriteLiteral(" class=\"table table-striped table-condensed\"");

WriteLiteral(">\r\n    <thead>\r\n        <tr>\r\n            <th>Name</th>\r\n            <th>Version<" +
"/th>\r\n            <th></th>\r\n        </tr>\r\n    </thead>\r\n    <tbody");

WriteLiteral(" data-bind=\"foreach : ReferencedAssemblyCollection\"");

WriteLiteral(">\r\n        <tr>\r\n            <td");

WriteLiteral(" data-bind=\"text:Name\"");

WriteLiteral("></td>\r\n            <td");

WriteLiteral(" data-bind=\"text:Version\"");

WriteLiteral("></td>\r\n            <td>\r\n                <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : $parent.removeReferencedAssembly.call($parent,$data)\"");

WriteLiteral(">\r\n                    <i");

WriteLiteral(" class=\"fa fa-times\"");

WriteLiteral("></i>\r\n                </a>\r\n            </td>\r\n        </tr>\r\n    </tbody>\r\n\r\n</" +
"table>\r\n<a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : addReferencedAssembly\"");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(">\r\n    <i");

WriteLiteral(" class=\"fa fa-plus-circle\"");

WriteLiteral("></i>\r\n    Add an assembly\r\n</a>\r\n");

        }
    }
}
#pragma warning restore 1591
