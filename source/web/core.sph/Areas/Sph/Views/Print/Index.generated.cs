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
    
    #line 1 "..\..\Areas\Sph\Views\Print\Index.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Areas\Sph\Views\Print\Index.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Print/Index.cshtml")]
    public partial class _Areas_Sph_Views_Print_Index_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.PrintViewModel>
    {
        public _Areas_Sph_Views_Print_Index_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Areas\Sph\Views\Print\Index.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<!DOCTYPE html>\r\n\r\n<html>\r\n<head>\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width\"");

WriteLiteral(" />\r\n    <title>");

            
            #line 14 "..\..\Areas\Sph\Views\Print\Index.cshtml"
      Write(Bespoke.Sph.Domain.ConfigurationManager.ApplicationName);

            
            #line default
            #line hidden
WriteLiteral(" : ");

            
            #line 14 "..\..\Areas\Sph\Views\Print\Index.cshtml"
                                                                 Write(Model.Item.ToString());

            
            #line default
            #line hidden
WriteLiteral("</title>\r\n    <link");

WriteAttribute("href", Tuple.Create(" href=\"", 338), Tuple.Create("\"", 364)
, Tuple.Create(Tuple.Create("", 345), Tuple.Create<System.Object, System.Int32>(Href("~/Content/__css.css")
, 345), false)
);

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" />\r\n</head>\r\n<body>\r\n\r\n    <div");

WriteLiteral(" id=\"print-details\"");

WriteLiteral(">\r\n        <h1>");

            
            #line 20 "..\..\Areas\Sph\Views\Print\Index.cshtml"
       Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n        <div");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\r\n            <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n");

            
            #line 23 "..\..\Areas\Sph\Views\Print\Index.cshtml"
                
            
            #line default
            #line hidden
            
            #line 23 "..\..\Areas\Sph\Views\Print\Index.cshtml"
                 foreach (var fe in Model.FormDesign.FormElementCollection)
                {
                    var fe1 = fe;
                    
            
            #line default
            #line hidden
            
            #line 26 "..\..\Areas\Sph\Views\Print\Index.cshtml"
               Write(Html.DisplayFor(m => fe1));

            
            #line default
            #line hidden
            
            #line 26 "..\..\Areas\Sph\Views\Print\Index.cshtml"
                                              
                }

            
            #line default
            #line hidden
WriteLiteral("\r\n            </form>\r\n        </div>\r\n\r\n    </div>\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 805), Tuple.Create("\"", 832)
, Tuple.Create(Tuple.Create("", 811), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/__vendor.js")
, 811), false)
);

WriteLiteral("></script>\r\n    <script");

WriteAttribute("src", Tuple.Create(" src=\"", 856), Tuple.Create("\"", 881)
, Tuple.Create(Tuple.Create("", 862), Tuple.Create<System.Object, System.Int32>(Href("~/Scripts/__core.js")
, 862), false)
);

WriteLiteral("></script>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(function () {\r\n            var item = ko.mapping.fromJSON(\'");

            
            #line 37 "..\..\Areas\Sph\Views\Print\Index.cshtml"
                                       Write(Html.Raw(JsonConvert.SerializeObject(Model.Item)));

            
            #line default
            #line hidden
WriteLiteral("\');\r\n            ko.applyBindings(item);\r\n\r\n\r\n        });\r\n\r\n    </script>\r\n</bod" +
"y>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
