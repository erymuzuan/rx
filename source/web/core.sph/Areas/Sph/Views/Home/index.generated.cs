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

namespace Bespoke.Sph.Web.Areas.Sph.Views.Home
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
    
    #line 3 "..\..\Areas\Sph\Views\Home\index.cshtml"
    using System.Web.Optimization;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Home/index.cshtml")]
    public partial class index : System.Web.Mvc.WebViewPage<dynamic>
    {
        public index()
        {
        }
        public override void Execute()
        {




            
            #line 4 "..\..\Areas\Sph\Views\Home\index.cshtml"
  
    Layout = null;
    var theme = ConfigurationManager.AppSettings["theme"];


            
            #line default
            #line hidden
WriteLiteral("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <title>");


            
            #line 11 "..\..\Areas\Sph\Views\Home\index.cshtml"
      Write(Bespoke.Sph.Domain.ConfigurationManager.ApplicationName);

            
            #line default
            #line hidden
WriteLiteral(@"</title>
    <meta charset=""utf-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge, chrome=1"" />
    <meta name=""apple-mobile-web-app-capable"" content=""yes"" />
    <meta name=""apple-mobile-web-app-status-bar-style"" content=""black"" />
    <meta name=""format-detection"" content=""telephone=no"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />

    ");



WriteLiteral(@"
    <link href=""/Content/css.css"" rel=""stylesheet"" />
    <link href=""/Content/nprogress.css"" rel=""stylesheet"" />
    <link href=""/Content/daterangepicker-bs3.css"" rel=""stylesheet"" />
    <link href=""/Content/site.css"" rel=""stylesheet"" />
    <meta name=""description"" content=""The description of my page"" />
</head>
<body>
    <div id=""applicationHost"">
        ");


            
            #line 28 "..\..\Areas\Sph\Views\Home\index.cshtml"
   Write(Html.Partial("_splash"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n\r\n    ");



WriteLiteral("\r\n    <script src=\"~/SphApp/objectbuilders.js\"></script>\r\n");


            
            #line 38 "..\..\Areas\Sph\Views\Home\index.cshtml"
     if (HttpContext.Current.IsDebuggingEnabled)
    {

            
            #line default
            #line hidden
WriteLiteral("        <script src=\"/Scripts/__vendor.js\"></script>\r\n");



WriteLiteral("        <script src=\"/Scripts/nprogress.js\"></script>\r\n");



WriteLiteral("        <script src=\"/Scripts/jstree.min.js\"></script>\r\n");



WriteLiteral("        <script src=\"/Scripts/__core.js\"></script>\r\n");



WriteLiteral("        <script src=\"/Scripts/_ko.entity.js\"></script>\r\n");



WriteLiteral("        <script src=\"/SphApp/schemas/domain.js\"></script>\r\n");



WriteLiteral("        <script src=\"/SphApp/prototypes/prototypes.js\"></script>\r\n");



WriteLiteral("        <script src=\"/SphApp/partial/__partial.js\"></script>\r\n");



WriteLiteral("        <script type=\"text/javascript\" src=\"/sph/entitydefinition/schemas\"></scri" +
"pt>\r\n");



WriteLiteral("        <script src=\"/Content/theme.");


            
            #line 49 "..\..\Areas\Sph\Views\Home\index.cshtml"
                               Write(theme);

            
            #line default
            #line hidden
WriteLiteral("/theme.js\"></script>\r\n");



WriteLiteral("        <script src=\"/kendo/js/kendo.all.js\"></script>\r\n");


            
            #line 51 "..\..\Areas\Sph\Views\Home\index.cshtml"
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("        <script src=\"/Scripts/__vendor.min.js\"></script>\r\n");



WriteLiteral("        <script src=\"/Scripts/nprogress.js\"></script>\r\n");



WriteLiteral("        <script src=\"/Scripts/jstree.min.js\"></script>\r\n");



WriteLiteral("        <script src=\"/Scripts/__core.min.js\"></script>\r\n");



WriteLiteral("        <script src=\"/Scripts/_ko.entity.js\"></script>\r\n");



WriteLiteral("        <script src=\"/SphApp/schemas/domain.min.js\"></script>\r\n");



WriteLiteral("        <script src=\"/SphApp/prototypes/prototypes.min.js\"></script>\r\n");



WriteLiteral("        <script src=\"/SphApp/partial/__partial.min.js\"></script>\r\n");



WriteLiteral("        <script type=\"text/javascript\" src=\"/sph/entitydefinition/schemas\"></scri" +
"pt>\r\n");



WriteLiteral("        <script src=\"~/Content/theme.");


            
            #line 63 "..\..\Areas\Sph\Views\Home\index.cshtml"
                                Write(theme);

            
            #line default
            #line hidden
WriteLiteral("/theme.js\"></script>\r\n");



WriteLiteral("        <script src=\"/kendo/js/kendo.all.min.js\"></script>\r\n");


            
            #line 65 "..\..\Areas\Sph\Views\Home\index.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <script type=\"text/javascript\" src=\"/Scripts/require.js\" data-main=\"SphAp" +
"p/main\"></script>\r\n\r\n\r\n\r\n    ");



WriteLiteral("\r\n</body>\r\n</html>\r\n");


        }
    }
}
#pragma warning restore 1591
