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

namespace Bespoke.Sph.Web.Areas.App.Views.ReportDefinitionEdit.DisplayTemplates
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
    
    #line 1 "..\..\Areas\App\Views\ReportDefinitionEdit\DisplayTemplates\LineChartItem.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ReportDefinitionEdit/DisplayTemplates/LineChartItem.cshtml")]
    public partial class LineChartItem_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.LineChartItem>
    {
        public LineChartItem_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<!--ko if: $data[\'$type\']() === \"Bespoke.Sph.Domain.LineChartItem, domain.sph\" --" +
">\r\n<div");

WriteLiteral("  class=\"report-item\"");

WriteLiteral(" data-bind=\"css: { \'selected-form-element\': isSelected }, click: $root.selectedRe" +
"portItem\"");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 6 "..\..\Areas\App\Views\ReportDefinitionEdit\DisplayTemplates\LineChartItem.cshtml"
Write(Html.Partial("_LineChartDesignerContextAction"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <img");

WriteLiteral(" src=\"/Images/line-chart256.png\"");

WriteLiteral(" alt=\"line chart\"");

WriteLiteral("/>\r\n</div>\r\n<!--/ko-->\r\n");

        }
    }
}
#pragma warning restore 1591