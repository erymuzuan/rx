﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Areas.Sph.Views.Shared
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
    
    #line 1 "..\..\Areas\Sph\Views\Shared\_LayoutWorkflow.cshtml"
    using System.Web.Optimization;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Shared/_LayoutWorkflow.cshtml")]
    public partial class LayoutWorkflow : System.Web.Mvc.WebViewPage<dynamic>
    {
        public LayoutWorkflow()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!DOCTYPE html>\r\n<html");

WriteLiteral(" lang=\"en\"");

WriteLiteral(">\r\n<head>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(" />\r\n    <title>");

            
            #line 6 "..\..\Areas\Sph\Views\Shared\_LayoutWorkflow.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral(" - SPH workflow</title>\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 164), Tuple.Create("\"", 184)
, Tuple.Create(Tuple.Create("", 171), Tuple.Create<System.Object, System.Int32>(Href("~/favicon.ico")
, 171), false)
);

WriteLiteral(" rel=\"shortcut icon\"");

WriteLiteral(" type=\"image/x-icon\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width\"");

WriteLiteral(" />\r\n    ");

WriteLiteral("\r\n\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 341), Tuple.Create("\"", 365)
, Tuple.Create(Tuple.Create("", 348), Tuple.Create<System.Object, System.Int32>(Href("~/Content/css.css")
, 348), false)
);

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" src=\"https://www.google.com/jsapi\"");

WriteLiteral("></script>\r\n</head>\r\n    <body>\r\n        <div");

WriteLiteral(" id=\"body\"");

WriteLiteral(">\r\n            <section");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 17 "..\..\Areas\Sph\Views\Shared\_LayoutWorkflow.cshtml"
           Write(RenderBody());

            
            #line default
            #line hidden
WriteLiteral("\r\n            </section>\r\n        </div>\r\n\r\n\r\n\r\n        <script");

WriteAttribute("src", Tuple.Create(" src=\"", 648), Tuple.Create("\"", 677)
, Tuple.Create(Tuple.Create("", 654), Tuple.Create<System.Object, System.Int32>(Href("~/App/objectbuilders.js")
, 654), false)
);

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral("></script>\r\n        <script");

WriteAttribute("src", Tuple.Create(" src=\"", 728), Tuple.Create("\"", 763)
, Tuple.Create(Tuple.Create("", 734), Tuple.Create<System.Object, System.Int32>(Href("~/App/durandal/amd/require.js")
, 734), false)
);

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral("></script>\r\n        <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">
            require.config({
                baseUrl: ""/App"",
                waitSeconds: 15
            });
            define('jquery', function () { return jQuery; });
            define('knockout', ko);
        </script>

    </body>
</html>
");

        }
    }
}
#pragma warning restore 1591
