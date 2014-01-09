﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Views.Shared
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_Layout.cshtml")]
    public partial class Layout : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Layout()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!DOCTYPE html>\r\n<html");

WriteLiteral(" lang=\"en\"");

WriteLiteral(">\r\n<head>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(" />\r\n    <title>");

            
            #line 5 "..\..\Views\Shared\_Layout.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral(" - SPH</title>\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 123), Tuple.Create("\"", 143)
, Tuple.Create(Tuple.Create("", 130), Tuple.Create<System.Object, System.Int32>(Href("~/favicon.ico")
, 130), false)
);

WriteLiteral(" rel=\"shortcut icon\"");

WriteLiteral(" type=\"image/x-icon\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width\"");

WriteLiteral(" />\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 257), Tuple.Create("\"", 281)
, Tuple.Create(Tuple.Create("", 264), Tuple.Create<System.Object, System.Int32>(Href("~/Content/css.css")
, 264), false)
);

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 315), Tuple.Create("\"", 340)
, Tuple.Create(Tuple.Create("", 321), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/vendor.js")
, 321), false)
);

WriteLiteral("></script>\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 364), Tuple.Create("\"", 387)
, Tuple.Create(Tuple.Create("", 370), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/core.js")
, 370), false)
);

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" src=\"https://www.google.com/jsapi\"");

WriteLiteral("></script>\r\n</head>\r\n<body>\r\n    <div");

WriteLiteral(" id=\"body\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 15 "..\..\Views\Shared\_Layout.cshtml"
   Write(RenderSection("featured", required: false));

            
            #line default
            #line hidden
WriteLiteral("\r\n        <section");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 17 "..\..\Views\Shared\_Layout.cshtml"
       Write(RenderBody());

            
            #line default
            #line hidden
WriteLiteral("\r\n        </section>\r\n    </div>\r\n\r\n\r\n\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 685), Tuple.Create("\"", 717)
, Tuple.Create(Tuple.Create("", 691), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/objectbuilders.js")
, 691), false)
);

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral("></script>\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 764), Tuple.Create("\"", 790)
, Tuple.Create(Tuple.Create("", 770), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/require.js")
, 770), false)
);

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">

        var bespoke = bespoke || {};
        bespoke.sph = bespoke.sph || {};
        bespoke.sph.domain = bespoke.sph.domain || {};


        require.config({
            baseUrl: ""/SphApp"",
            waitSeconds: 15,
            paths: {
                'durandal': '/Scripts/durandal',
                'plugins': '/Scripts/durandal/plugins'
            }
        });
        define('jquery', function () { return jQuery; });
        define('knockout', ko);
    </script>

");

WriteLiteral("    ");

            
            #line 44 "..\..\Views\Shared\_Layout.cshtml"
Write(RenderSection("scripts", required: false));

            
            #line default
            #line hidden
WriteLiteral("\r\n</body>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
