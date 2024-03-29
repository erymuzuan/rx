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
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    #line 1 "..\..\Areas\App\Views\Shared\DisplayTemplates\LineChartItem.cshtml"
    using Newtonsoft.Json;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/DisplayTemplates/LineChartItem.cshtml")]
    public partial class _Areas_App_Views_Shared_DisplayTemplates_LineChartItem_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.LineChartItem>
    {
        public _Areas_App_Views_Shared_DisplayTemplates_LineChartItem_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\App\Views\Shared\DisplayTemplates\LineChartItem.cshtml"
  
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
WriteLiteral("\r\n\r\n<div>\r\n    <div");

WriteAttribute("id", Tuple.Create(" id=\"", 349), Tuple.Create("\"", 357)
            
            #line 16 "..\..\Areas\App\Views\Shared\DisplayTemplates\LineChartItem.cshtml"
, Tuple.Create(Tuple.Create("", 354), Tuple.Create<System.Object, System.Int32>(id
            
            #line default
            #line hidden
, 354), false)
);

WriteLiteral("></div>\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        (function () {\r\n\r\n            $(\"#");

            
            #line 20 "..\..\Areas\App\Views\Shared\DisplayTemplates\LineChartItem.cshtml"
           Write(id);

            
            #line default
            #line hidden
WriteLiteral("\").kendoChart({\r\n                title: {\r\n                    text: \"");

            
            #line 22 "..\..\Areas\App\Views\Shared\DisplayTemplates\LineChartItem.cshtml"
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
                    type: ""line""
                },
                series:");

            
            #line 33 "..\..\Areas\App\Views\Shared\DisplayTemplates\LineChartItem.cshtml"
                  Write(Html.Raw(seriesJson));

            
            #line default
            #line hidden
WriteLiteral(",\r\n                valueAxis: {\r\n                    labels: {\r\n                 " +
"       format: \"");

            
            #line 36 "..\..\Areas\App\Views\Shared\DisplayTemplates\LineChartItem.cshtml"
                            Write(Model.ValueLabelFormat);

            
            #line default
            #line hidden
WriteLiteral("\"\r\n                        },\r\n                        line: {\r\n                 " +
"           visible: false\r\n                        }\r\n                    },\r\n  " +
"              categoryAxis: {\r\n                    categories: [");

            
            #line 43 "..\..\Areas\App\Views\Shared\DisplayTemplates\LineChartItem.cshtml"
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
