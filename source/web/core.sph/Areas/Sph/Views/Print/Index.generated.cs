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

namespace Bespoke.Sph.Web.Areas.Sph.Views.Print
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
    public partial class Index : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.PrintViewModel>
    {
        public Index()
        {
        }
        public override void Execute()
        {



WriteLiteral("\r\n");


            
            #line 5 "..\..\Areas\Sph\Views\Print\Index.cshtml"
  
    Layout = null;


            
            #line default
            #line hidden
WriteLiteral("\r\n<!DOCTYPE html>\r\n\r\n<html>\r\n<head>\r\n    <meta name=\"viewport\" content=\"width=dev" +
"ice-width\" />\r\n    <title>");


            
            #line 14 "..\..\Areas\Sph\Views\Print\Index.cshtml"
      Write(Bespoke.Sph.Domain.ConfigurationManager.ApplicationName);

            
            #line default
            #line hidden
WriteLiteral(" : ");


            
            #line 14 "..\..\Areas\Sph\Views\Print\Index.cshtml"
                                                                 Write(Model.Item.ToString());

            
            #line default
            #line hidden
WriteLiteral("</title>\r\n    <link href=\"~/Content/__css.css\" rel=\"stylesheet\" />\r\n</head>\r\n<bod" +
"y>\r\n\r\n    <div id=\"print-details\">\r\n        <h1>");


            
            #line 20 "..\..\Areas\Sph\Views\Print\Index.cshtml"
       Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n        <div class=\"container\">\r\n            <form class=\"form-horizontal\"" +
">\r\n");


            
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
WriteLiteral(@"
            </form>
        </div>

    </div>
    <script src=""~/Scripts/__vendor.js""></script>
    <script src=""~/Scripts/__core.js""></script>
    <script type=""text/javascript"">
        $(function () {
            var item = ko.mapping.fromJSON('");


            
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
