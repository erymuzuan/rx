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
    
    #line 1 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/ReportDefinitionExecuteHtml.cshtml")]
    public partial class ReportDefinitionExecuteHtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.RdlExecutionViewModel>
    {
        public ReportDefinitionExecuteHtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n");

            
            #line 9 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
 if (null != Model.Rdl && Model.IsPostback)
{
    foreach (var layout in Model.Rdl.ReportLayoutCollection)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n");

            
            #line 14 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
            
            
            #line default
            #line hidden
            
            #line 14 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
             foreach (var item in layout.ReportItemCollection)
            {
                
            
            #line default
            #line hidden
            
            #line 16 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
           Write(Html.DisplayFor(m => item));

            
            #line default
            #line hidden
            
            #line 16 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                           
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n");

            
            #line 19 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
    }
}

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 23 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
 if (!Model.IsPostback && null != Model.Rdl)
{
    var count = -1;


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" id=\"report-layout-panel\"");

WriteLiteral(">\r\n\r\n    </div>\r\n");

WriteLiteral("    <section");

WriteLiteral(" class=\"modal hide\"");

WriteLiteral(" id=\"parameters-dialog\"");

WriteLiteral(">\r\n\r\n        <div");

WriteLiteral(" class=\"modal-header\"");

WriteLiteral(">\r\n            <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">&times;</button>\r\n            <h3>Parameters</h3>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"modal-body\"");

WriteLiteral(" data-bind=\"with : datasource\"");

WriteLiteral(">\r\n            <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(">\r\n");

            
            #line 38 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                
            
            #line default
            #line hidden
            
            #line 38 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                 foreach (var parm in Model.Rdl.DataSource.ParameterCollection)
                {
                    count++;
                    if (parm.Type == typeof(string) && !string.IsNullOrWhiteSpace(parm.AvailableValues))
                    {


            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 1240), Tuple.Create("\"", 1263)
, Tuple.Create(Tuple.Create("", 1246), Tuple.Create("parameter", 1246), true)
            
            #line 45 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
, Tuple.Create(Tuple.Create("", 1255), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 1255), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 45 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                            Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                            <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                <select");

WriteLiteral(" data-bind=\"value: ParameterCollection()[");

            
            #line 47 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                           Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 1454), Tuple.Create("\"", 1476)
, Tuple.Create(Tuple.Create("", 1459), Tuple.Create("parameter", 1459), true)
            
            #line 47 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                             , Tuple.Create(Tuple.Create("", 1468), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 1468), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 1477), Tuple.Create("\"", 1501)
, Tuple.Create(Tuple.Create("", 1484), Tuple.Create("parameter", 1484), true)
            
            #line 47 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                      , Tuple.Create(Tuple.Create("", 1493), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 1493), false)
);

WriteLiteral(">\r\n                                    <option>[Please Select]</option>\r\n");

            
            #line 49 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 49 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                     foreach (var opt in parm.AvailableValues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <option");

WriteAttribute("value", Tuple.Create(" value=\"", 1801), Tuple.Create("\"", 1813)
            
            #line 51 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
, Tuple.Create(Tuple.Create("", 1809), Tuple.Create<System.Object, System.Int32>(opt
            
            #line default
            #line hidden
, 1809), false)
);

WriteLiteral(">");

            
            #line 51 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                        Write(opt);

            
            #line default
            #line hidden
WriteLiteral("</option>\r\n");

            
            #line 52 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                </select>\r\n                            </div>\r\n  " +
"                      </div>\r\n");

            
            #line 56 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"

                    }

                    if (parm.Type == typeof(string) && string.IsNullOrWhiteSpace(parm.AvailableValues))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 2222), Tuple.Create("\"", 2245)
, Tuple.Create(Tuple.Create("", 2228), Tuple.Create("parameter", 2228), true)
            
            #line 62 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
, Tuple.Create(Tuple.Create("", 2237), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 2237), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 62 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                            Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                            <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                <input");

WriteLiteral(" data-bind=\"value: ParameterCollection()[");

            
            #line 64 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                          Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 2435), Tuple.Create("\"", 2457)
, Tuple.Create(Tuple.Create("", 2440), Tuple.Create("parameter", 2440), true)
            
            #line 64 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                            , Tuple.Create(Tuple.Create("", 2449), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 2449), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 2458), Tuple.Create("\"", 2482)
, Tuple.Create(Tuple.Create("", 2465), Tuple.Create("parameter", 2465), true)
            
            #line 64 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                     , Tuple.Create(Tuple.Create("", 2474), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 2474), false)
);

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                            </div>\r\n                        </div>\r\n");

            
            #line 67 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                    }
                    if (parm.Type == typeof(DateTime))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 2757), Tuple.Create("\"", 2780)
, Tuple.Create(Tuple.Create("", 2763), Tuple.Create("parameter", 2763), true)
            
            #line 71 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
, Tuple.Create(Tuple.Create("", 2772), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 2772), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 71 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                            Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                            <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                <input");

WriteLiteral(" data-bind=\"kendoDate: ParameterCollection()[");

            
            #line 73 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                              Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 2974), Tuple.Create("\"", 2996)
, Tuple.Create(Tuple.Create("", 2979), Tuple.Create("parameter", 2979), true)
            
            #line 73 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                , Tuple.Create(Tuple.Create("", 2988), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 2988), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 2997), Tuple.Create("\"", 3021)
, Tuple.Create(Tuple.Create("", 3004), Tuple.Create("parameter", 3004), true)
            
            #line 73 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                         , Tuple.Create(Tuple.Create("", 3013), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3013), false)
);

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                            </div>\r\n                        </div>\r\n");

            
            #line 76 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                    }
                    if (parm.Type == typeof(decimal))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 3295), Tuple.Create("\"", 3318)
, Tuple.Create(Tuple.Create("", 3301), Tuple.Create("parameter", 3301), true)
            
            #line 80 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
, Tuple.Create(Tuple.Create("", 3310), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3310), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 80 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                            Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                            <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                <input");

WriteLiteral(" data-bind=\"value: ParameterCollection()[");

            
            #line 82 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                          Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 3508), Tuple.Create("\"", 3530)
, Tuple.Create(Tuple.Create("", 3513), Tuple.Create("parameter", 3513), true)
            
            #line 82 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                            , Tuple.Create(Tuple.Create("", 3522), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3522), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 3531), Tuple.Create("\"", 3555)
, Tuple.Create(Tuple.Create("", 3538), Tuple.Create("parameter", 3538), true)
            
            #line 82 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                     , Tuple.Create(Tuple.Create("", 3547), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3547), false)
);

WriteLiteral(" type=\"text\"");

WriteLiteral(" />\r\n                            </div>\r\n                        </div>\r\n");

            
            #line 85 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                    }
                    if (parm.Type == typeof(int))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n                            <label");

WriteAttribute("for", Tuple.Create(" for=\"", 3825), Tuple.Create("\"", 3848)
, Tuple.Create(Tuple.Create("", 3831), Tuple.Create("parameter", 3831), true)
            
            #line 89 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
, Tuple.Create(Tuple.Create("", 3840), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 3840), false)
);

WriteLiteral(" class=\"control-label\"");

WriteLiteral(">");

            
            #line 89 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                            Write(parm.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                            <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n                                <input");

WriteLiteral(" data-bind=\"value: ParameterCollection()[");

            
            #line 91 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                                          Write(count);

            
            #line default
            #line hidden
WriteLiteral("].Value\"");

WriteAttribute("id", Tuple.Create(" id=\"", 4038), Tuple.Create("\"", 4060)
, Tuple.Create(Tuple.Create("", 4043), Tuple.Create("parameter", 4043), true)
            
            #line 91 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                            , Tuple.Create(Tuple.Create("", 4052), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 4052), false)
);

WriteAttribute("name", Tuple.Create(" name=\"", 4061), Tuple.Create("\"", 4085)
, Tuple.Create(Tuple.Create("", 4068), Tuple.Create("parameter", 4068), true)
            
            #line 91 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                                                     , Tuple.Create(Tuple.Create("", 4077), Tuple.Create<System.Object, System.Int32>(count
            
            #line default
            #line hidden
, 4077), false)
);

WriteLiteral(" type=\"number\"");

WriteLiteral(" />\r\n                            </div>\r\n                        </div>\r\n");

            
            #line 94 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"
                    }

                }

            
            #line default
            #line hidden
WriteLiteral("            </form>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" class=\"modal-footer\"");

WriteLiteral(">\r\n            <button");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-primary\"");

WriteLiteral(" data-bind=\"click: executeCommand\"");

WriteLiteral(">\r\n                <i");

WriteLiteral(" class=\"fa fa-play\"");

WriteLiteral("></i>\r\n                Run\r\n            </button>\r\n            <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"btn\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(">Close</a>\r\n        </div>\r\n    </section>\r\n");

            
            #line 108 "..\..\Areas\Sph\Views\App\ReportDefinitionExecuteHtml.cshtml"






}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
