﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    
    #line 1 "..\..\Areas\Sph\Views\Home\index.cshtml"
    using System.Configuration;
    
    #line default
    #line hidden
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    
    #line 2 "..\..\Areas\Sph\Views\Home\index.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Home/index.cshtml")]
    public partial class _Areas_Sph_Views_Home_index_cshtml : System.Web.Mvc.WebViewPage<dynamic>
    {
        public _Areas_Sph_Views_Home_index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\Sph\Views\Home\index.cshtml"
  
    Layout = null;
    var theme = ConfigurationManager.AppSettings["theme"];
    var splash = Server.MapPath("~/_splash.html");
    var splashHtml = "";
    if (System.IO.File.Exists(splash))
    {
        splashHtml = (File.ReadAllText(splash));
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <title>");

            
            #line 16 "..\..\Areas\Sph\Views\Home\index.cshtml"
      Write(Bespoke.Sph.Domain.ConfigurationManager.ApplicationName);

            
            #line default
            #line hidden
WriteLiteral("</title>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" http-equiv=\"X-UA-Compatible\"");

WriteLiteral(" content=\"IE=edge, chrome=1\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"apple-mobile-web-app-capable\"");

WriteLiteral(" content=\"yes\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"apple-mobile-web-app-status-bar-style\"");

WriteLiteral(" content=\"black\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"format-detection\"");

WriteLiteral(" content=\"telephone=no\"");

WriteLiteral(" />\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1.0\"");

WriteLiteral(" />\r\n");

            
            #line 23 "..\..\Areas\Sph\Views\Home\index.cshtml"
    
            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\Sph\Views\Home\index.cshtml"
     if (HttpContext.Current.IsDebuggingEnabled)
    {

            
            #line default
            #line hidden
WriteLiteral("        <link");

WriteLiteral(" href=\"/Content/__css.css\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n");

            
            #line 26 "..\..\Areas\Sph\Views\Home\index.cshtml"
        foreach (var css in System.IO.Directory.GetFiles(Server.MapPath("~/Content"), "*.css"))
        {


            
            #line default
            #line hidden
WriteLiteral("            <link");

WriteAttribute("href", Tuple.Create(" href=\"", 1060), Tuple.Create("\"", 1108)
, Tuple.Create(Tuple.Create("", 1067), Tuple.Create("/content/", 1067), true)
            
            #line 29 "..\..\Areas\Sph\Views\Home\index.cshtml"
, Tuple.Create(Tuple.Create("", 1076), Tuple.Create<System.Object, System.Int32>(System.IO.Path.GetFileName(css)
            
            #line default
            #line hidden
, 1076), false)
);

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n");

            
            #line 30 "..\..\Areas\Sph\Views\Home\index.cshtml"
        }
    }
    else
    {


            
            #line default
            #line hidden
WriteLiteral("        <link");

WriteLiteral(" href=\"/content/release.css\"");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n");

            
            #line 36 "..\..\Areas\Sph\Views\Home\index.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("    <script");

WriteLiteral(" src=\"/Scripts/modernizr-2.8.3.js\"");

WriteLiteral("></script>\r\n    <meta");

WriteLiteral(" name=\"description\"");

WriteLiteral(" content=\"The description of my page\"");

WriteLiteral(" />\r\n</head>\r\n<body>\r\n    <div");

WriteLiteral(" id=\"applicationHost\"");

WriteLiteral(">\r\n");

            
            #line 42 "..\..\Areas\Sph\Views\Home\index.cshtml"
        
            
            #line default
            #line hidden
            
            #line 42 "..\..\Areas\Sph\Views\Home\index.cshtml"
         if (!string.IsNullOrWhiteSpace(splashHtml))
        {
            
            
            #line default
            #line hidden
            
            #line 44 "..\..\Areas\Sph\Views\Home\index.cshtml"
       Write(Html.Raw(splashHtml));

            
            #line default
            #line hidden
            
            #line 44 "..\..\Areas\Sph\Views\Home\index.cshtml"
                                 
        }
        else
        {
            
            
            #line default
            #line hidden
            
            #line 48 "..\..\Areas\Sph\Views\Home\index.cshtml"
       Write(Html.Partial("_splash"));

            
            #line default
            #line hidden
            
            #line 48 "..\..\Areas\Sph\Views\Home\index.cshtml"
                                    
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n\r\n");

            
            #line 52 "..\..\Areas\Sph\Views\Home\index.cshtml"
    
            
            #line default
            #line hidden
            
            #line 52 "..\..\Areas\Sph\Views\Home\index.cshtml"
     if (HttpContext.Current.IsDebuggingEnabled)
    {

            
            #line default
            #line hidden
WriteLiteral("        <script");

WriteAttribute("src", Tuple.Create(" src=\"", 1685), Tuple.Create("\"", 1717)
, Tuple.Create(Tuple.Create("", 1691), Tuple.Create<System.Object, System.Int32>(Href("~/SphApp/objectbuilders.js")
, 1691), false)
);

WriteLiteral("></script>\r\n");

WriteLiteral("        <script");

WriteLiteral(" src=\"/Scripts/__vendor.js\"");

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

            
            #line 61 "..\..\Areas\Sph\Views\Home\index.cshtml"
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("        <script");

WriteLiteral(" src=\"/Scripts/__rx.min.js\"");

WriteLiteral("></script>\r\n");

            
            #line 65 "..\..\Areas\Sph\Views\Home\index.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" src=\"/sph/entitydefinition/schemas\"");

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" src=\"/Scripts/require.js\"");

WriteLiteral(" data-main=\"SphApp/main\"");

WriteLiteral("></script>\r\n\r\n</body>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
