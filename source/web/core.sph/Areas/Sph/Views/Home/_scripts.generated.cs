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
    
    #line 2 "..\..\Areas\Sph\Views\Home\_scripts.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Home/_scripts.cshtml")]
    public partial class _Areas_Sph_Views_Home__scripts_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.SphIndexViewModel>
    {
        public _Areas_Sph_Views_Home__scripts_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n\r\n<script");

WriteLiteral(" src=\"/scripts/__vendor.dev.min.js\"");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral("></script>\r\n\r\n<!-- END PAGE LEVEL SCRIPTS -->\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        jQuery(document).ready(function () {\r\n            Metronic.init(); // " +
"init metronic core componets\r\n            Layout.init(); // init layout\r\n       " +
"     QuickSidebar.init(); // init quick sidebar\r\n        });\r\n</script>\r\n");

            
            #line 15 "..\..\Areas\Sph\Views\Home\_scripts.cshtml"
 if (HttpContext.Current.IsDebuggingEnabled)
{

            
            #line default
            #line hidden
WriteLiteral("        <script");

WriteAttribute("src", Tuple.Create(" src=\"", 525), Tuple.Create("\"", 557)
, Tuple.Create(Tuple.Create("", 531), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/objectbuilders.js")
, 531), false)
);

WriteLiteral("></script>\r\n");

WriteLiteral("        <script");

WriteLiteral(" src=\"/Scripts/__core.js\"");

WriteLiteral("></script>\r\n");

WriteLiteral("        <script");

WriteLiteral(" src=\"/SphApp/schemas/__domain.js\"");

WriteLiteral("></script>\r\n");

WriteLiteral("        <script");

WriteLiteral(" src=\"/SphApp/prototypes/prototypes.js\"");

WriteLiteral("></script>\r\n");

WriteLiteral("        <script");

WriteLiteral(" src=\"/SphApp/partial/__partial.js\"");

WriteLiteral("></script>\r\n");

WriteLiteral("        <script");

WriteLiteral(" src=\"/kendo/js/kendo.custom.min.js\"");

WriteLiteral("></script>\r\n");

            
            #line 23 "..\..\Areas\Sph\Views\Home\_scripts.cshtml"
}
else
{

            
            #line default
            #line hidden
WriteLiteral("    <script");

WriteLiteral(" src=\"/Scripts/__rx.min.js\"");

WriteLiteral("></script>\r\n");

            
            #line 27 "..\..\Areas\Sph\Views\Home\_scripts.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" src=\"/sph/entitydefinition/schemas\"");

WriteLiteral("></script>\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" src=\"/Scripts/require.js\"");

WriteLiteral(" data-main=\"SphApp/main\"");

WriteLiteral("></script>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
