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

namespace Bespoke.Sph.Web.Areas.App.Views.ReportDefinitionExecute
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
    
    #line 1 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ReportDefinitionExecute/Html.cshtml")]
    public partial class Html : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.RdlExecutionViewModel>
    {
        public Html()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n");

            
            #line 9 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
 if (null != Model.Rdl && Model.IsPostback)
{
    foreach (var layout in Model.Rdl.ReportLayoutCollection)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

            
            #line 14 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
            
            
            #line default
            #line hidden
            
            #line 14 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
             foreach (var item in layout.ReportItemCollection)
            {
                var ri = item;
                
            
            #line default
            #line hidden
            
            #line 17 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
           Write(Html.DisplayFor(m => ri));

            
            #line default
            #line hidden
            
            #line 17 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                         
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n");

            
            #line 20 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
    }
}

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 24 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
 if (!Model.IsPostback && null != Model.Rdl)
{
    var count = -1;


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" id=\"report-layout-panel\"");

WriteLiteral(">\r\n\r\n    </div>\r\n");

WriteLiteral("    <div");

WriteLiteral(" class=\"modal\"");

WriteLiteral(" id=\"parameters-dialog\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"modal-dialog\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"modal-content\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"modal-header\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">&times;</button>\r\n                    <h3>Parameters</h3>\r\n                </div" +
">\r\n                <div");

WriteLiteral(" class=\"modal-body\"");

WriteLiteral(" data-bind=\"with : datasource\"");

WriteLiteral(">\r\n                    <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n");

            
            #line 40 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 40 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                         foreach (var parm in Model.Rdl.DataSource.ParameterCollection)
                        {
                            count++;
                            if (parm.Type == typeof(string) && !string.IsNullOrWhiteSpace(parm.AvailableValues))
                            {


            
            #line default
            #line hidden
WriteLiteral("                                <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 1440), Tuple.Create("\"", 1463)
, Tuple.Create(Tuple.Create("", 1446), Tuple.Create("parameter", 1446), true)
            
            #line 47 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
, Tuple.Create(Tuple.Create("", 1455), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 1455), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 47 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                    Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                        <select");

WriteLiteral(" data-bind=\"value: ParameterCollection()[");

            
            #line 49 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                   Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 1670), Tuple.Create("\"", 1692)
, Tuple.Create(Tuple.Create("", 1675), Tuple.Create("parameter", 1675), true)
            
            #line 49 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                     , Tuple.Create(Tuple.Create("", 1684), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 1684), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 1693), Tuple.Create("\"", 1717)
, Tuple.Create(Tuple.Create("", 1700), Tuple.Create("parameter", 1700), true)
            
            #line 49 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                              , Tuple.Create(Tuple.Create("", 1709), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 1709), false)
);

WriteLiteral(">\r\n                                            <option>[Please Select]</option>\r\n" +
"");

            
            #line 51 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                            
            
            #line default
            #line hidden
            
            #line 51 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                             foreach (var opt in parm.AvailableValues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                            {

            
            #line default
            #line hidden
WriteLiteral("                                                <option");

WriteAttribute("value", Tuple.Create(" value=\"", 2049), Tuple.Create("\"", 2061)
            
            #line 53 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
, Tuple.Create(Tuple.Create("", 2057), Tuple.Create<System.Object, System.Int32>(opt
            
            #line default
            #line hidden
, 2057), false)
);

WriteLiteral(">");

            
            #line 53 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                Write(opt);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 54 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                            }

            
            #line default
            #line hidden
WriteLiteral("                                        </select>\r\n                              " +
"      </div>\r\n                                </div>\r\n");

            
            #line 58 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"

                            }

                            if (parm.Type == typeof(string) && string.IsNullOrWhiteSpace(parm.AvailableValues))
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 2542), Tuple.Create("\"", 2565)
, Tuple.Create(Tuple.Create("", 2548), Tuple.Create("parameter", 2548), true)
            
            #line 64 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
, Tuple.Create(Tuple.Create("", 2557), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 2557), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 64 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                    Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                        <input");

WriteLiteral(" data-bind=\"value: ParameterCollection()[");

            
            #line 66 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                  Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 2771), Tuple.Create("\"", 2793)
, Tuple.Create(Tuple.Create("", 2776), Tuple.Create("parameter", 2776), true)
            
            #line 66 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                    , Tuple.Create(Tuple.Create("", 2785), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 2785), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 2794), Tuple.Create("\"", 2818)
, Tuple.Create(Tuple.Create("", 2801), Tuple.Create("parameter", 2801), true)
            
            #line 66 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                             , Tuple.Create(Tuple.Create("", 2810), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 2810), false)
);

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                                    </div>\r\n                                " +
"</div>\r\n");

            
            #line 69 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                            }
                            if (parm.Type == typeof(DateTime))
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 3149), Tuple.Create("\"", 3172)
, Tuple.Create(Tuple.Create("", 3155), Tuple.Create("parameter", 3155), true)
            
            #line 73 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
, Tuple.Create(Tuple.Create("", 3164), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3164), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 73 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                    Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                        <input");

WriteLiteral(" data-bind=\"kendoDate: ParameterCollection()[");

            
            #line 75 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                      Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 3382), Tuple.Create("\"", 3404)
, Tuple.Create(Tuple.Create("", 3387), Tuple.Create("parameter", 3387), true)
            
            #line 75 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                        , Tuple.Create(Tuple.Create("", 3396), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3396), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 3405), Tuple.Create("\"", 3429)
, Tuple.Create(Tuple.Create("", 3412), Tuple.Create("parameter", 3412), true)
            
            #line 75 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                 , Tuple.Create(Tuple.Create("", 3421), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3421), false)
);

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                                    </div>\r\n                                " +
"</div>\r\n");

            
            #line 78 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                            }
                            if (parm.Type == typeof(decimal))
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 3759), Tuple.Create("\"", 3782)
, Tuple.Create(Tuple.Create("", 3765), Tuple.Create("parameter", 3765), true)
            
            #line 82 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
, Tuple.Create(Tuple.Create("", 3774), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3774), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 82 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                    Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                        <input");

WriteLiteral(" data-bind=\"value: ParameterCollection()[");

            
            #line 84 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                  Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 3988), Tuple.Create("\"", 4010)
, Tuple.Create(Tuple.Create("", 3993), Tuple.Create("parameter", 3993), true)
            
            #line 84 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                    , Tuple.Create(Tuple.Create("", 4002), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 4002), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 4011), Tuple.Create("\"", 4035)
, Tuple.Create(Tuple.Create("", 4018), Tuple.Create("parameter", 4018), true)
            
            #line 84 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                             , Tuple.Create(Tuple.Create("", 4027), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 4027), false)
);

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                                    </div>\r\n                                " +
"</div>\r\n");

            
            #line 87 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                            }
                            if (parm.Type == typeof(int))
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 4361), Tuple.Create("\"", 4384)
, Tuple.Create(Tuple.Create("", 4367), Tuple.Create("parameter", 4367), true)
            
            #line 91 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
, Tuple.Create(Tuple.Create("", 4376), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 4376), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 91 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                    Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                        <input");

WriteLiteral(" data-bind=\"value: ParameterCollection()[");

            
            #line 93 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                                                  Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 4590), Tuple.Create("\"", 4612)
, Tuple.Create(Tuple.Create("", 4595), Tuple.Create("parameter", 4595), true)
            
            #line 93 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                    , Tuple.Create(Tuple.Create("", 4604), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 4604), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 4613), Tuple.Create("\"", 4637)
, Tuple.Create(Tuple.Create("", 4620), Tuple.Create("parameter", 4620), true)
            
            #line 93 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                                                             , Tuple.Create(Tuple.Create("", 4629), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 4629), false)
);

WriteLiteral(" type=\"number\"");

WriteLiteral(" />\r\n                                    </div>\r\n                                " +
"</div>\r\n");

            
            #line 96 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"
                            }

                        }

            
            #line default
            #line hidden
WriteLiteral("                    </form>\r\n                </div>\r\n\r\n                <div");

WriteLiteral(" class=\"modal-footer\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-primary\"");

WriteLiteral(" data-bind=\"click: executeCommand\"");

WriteLiteral(">\r\n                        <i");

WriteLiteral(" class=\"fa fa-play\"");

WriteLiteral("></i>\r\n                        Run\r\n                    </button>\r\n              " +
"      <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"btn\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">Close</a>\r\n                </div>\r\n\r\n            </div>\r\n        </div>\r\n    </d" +
"iv>\r\n");

            
            #line 113 "..\..\Areas\App\Views\ReportDefinitionExecute\Html.cshtml"






}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
