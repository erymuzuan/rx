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

namespace Bespoke.Sph.Web.Areas.Sph.Views.App
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
    
    #line 1 "..\..\Areas\Sph\Views\App\_DataGridItemRow.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/_DataGridItemRow.cshtml")]
    public partial class DataGridItemRow : System.Web.Mvc.WebViewPage<System.Collections.Generic.IEnumerable<ReportRow>>
    {
        public DataGridItemRow()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\Sph\Views\App\_DataGridItemRow.cshtml"
  
    var index = 0;

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 6 "..\..\Areas\Sph\Views\App\_DataGridItemRow.cshtml"
 foreach (var r in Model)
{
    index++;

            
            #line default
            #line hidden
WriteLiteral("    <tr>\r\n\r\n        ");

WriteLiteral("\r\n    </tr>\r\n");

            
            #line 24 "..\..\Areas\Sph\Views\App\_DataGridItemRow.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591