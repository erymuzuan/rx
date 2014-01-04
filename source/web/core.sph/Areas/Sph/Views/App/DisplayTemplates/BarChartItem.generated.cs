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

namespace Bespoke.Sph.Web.Areas.Sph.Views.App.DisplayTemplates
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
    
    #line 1 "..\..\Areas\Sph\Views\App\DisplayTemplates\BarChartItem.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/DisplayTemplates/BarChartItem.cshtml")]
    public partial class BarChartItem : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.BarChartItem>
    {
        public BarChartItem()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\Sph\Views\App\DisplayTemplates\BarChartItem.cshtml"
  
    var id = Guid.NewGuid().ToString();
    var series = from s in Model.ChartSeriesCollection
                 select new
                 {
                     name = s.Header,
                     data = s.Values
                 };

    var seriesJson = JsonConvert.SerializeObject(series);

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n<div>\r\n    <div");

WriteAttribute("id", Tuple.Create(" id=\"", 397), Tuple.Create("\"", 405)
            
            #line 18 "..\..\Areas\Sph\Views\App\DisplayTemplates\BarChartItem.cshtml"
, Tuple.Create(Tuple.Create("", 402), Tuple.Create<System.Object, System.Int32>(id
            
            #line default
            #line hidden
, 402), false)
);

WriteLiteral("></div>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n            (function () {\r\n\r\n                $(\"#");

            
            #line 22 "..\..\Areas\Sph\Views\App\DisplayTemplates\BarChartItem.cshtml"
               Write(id);

            
            #line default
            #line hidden
WriteLiteral("\").kendoChart({\r\n                    title: {\r\n                        text: \"");

            
            #line 24 "..\..\Areas\Sph\Views\App\DisplayTemplates\BarChartItem.cshtml"
                          Write(Model.Title);

            
            #line default
            #line hidden
WriteLiteral(@"""
                    },
                    legend: {
                        position: ""bottom""
                    },
                    chartArea: {
                        background: """"
                    },
                    seriesDefaults: {
                        type: ""bar""
                    },
                    series:");

            
            #line 35 "..\..\Areas\Sph\Views\App\DisplayTemplates\BarChartItem.cshtml"
                      Write(Html.Raw(seriesJson));

            
            #line default
            #line hidden
WriteLiteral(",\r\n                    valueAxis: {\r\n                        labels: {\r\n         " +
"                   format: \"");

            
            #line 38 "..\..\Areas\Sph\Views\App\DisplayTemplates\BarChartItem.cshtml"
                                Write(Model.ValueLabelFormat);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n                        },\r\n                        line: {\r\n                 " +
"           visible: false\r\n                        }\r\n                    },\r\n  " +
"                  categoryAxis: {\r\n                        categories: [");

            
            #line 45 "..\..\Areas\Sph\Views\App\DisplayTemplates\BarChartItem.cshtml"
                                Write(Html.Raw(Model.CategoryAxiesValues));

            
            #line default
            #line hidden
WriteLiteral(@"],
                        majorGridLines: {
                            visible: false
                        }
                    },
                    tooltip: {
                        visible: true,
                        format: ""{0}%"",
                        template: ""#= category #: #= value #""
                    }
                });



            })();

    </script>


</div>
");

        }
    }
}
#pragma warning restore 1591
