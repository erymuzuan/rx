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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/DownloadLink.cshtml")]
    public partial class _Views_Shared_DisplayTemplates_DownloadLink_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.DownloadLink>
    {
        public _Views_Shared_DisplayTemplates_DownloadLink_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
 if (string.IsNullOrWhiteSpace(Model.Enable))
{
    Model.Enable = "true";
}

            
            #line default
            #line hidden
            
            #line 6 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
  
    var path = string.Format("'/sph/binarystore/get/' + {0}()", Model.Path);
    if (Model.IsTransformTemplate)
    {
        path = string.Format("'/sph/documenttemplate/transform?entity={0}&templateId={1}&id=' + {2}()", Model.Entity, Model.TemplateId, Model.Path);
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 14 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
 if (Model.IsCompact)
{

            
            #line default
            #line hidden
WriteLiteral("    <a");

WriteLiteral(" data-bind=\"attr : {\'href\':");

            
            #line 16 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
                            Write(Html.Raw(path));

            
            #line default
            #line hidden
WriteLiteral("}, visible:");

            
            #line 16 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
                                                      Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" download>\r\n");

WriteLiteral("        ");

            
            #line 17 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
   Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\r\n    </a>\r\n");

            
            #line 19 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
}
else
{


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 23 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
                                          Write(Html.Raw(Model.Visible));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n        <label");

WriteAttribute("for", Tuple.Create(" for=\"", 655), Tuple.Create("\"", 677)
            
            #line 24 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
, Tuple.Create(Tuple.Create("", 661), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 661), false)
);

WriteLiteral(" class=\"\"");

WriteLiteral("></label>\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 710), Tuple.Create("\"", 743)
            
            #line 25 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
, Tuple.Create(Tuple.Create("", 718), Tuple.Create<System.Object, System.Int32>(Model.InputPanelCssClass
            
            #line default
            #line hidden
, 718), false)
);

WriteLiteral(">\r\n            <a");

WriteAttribute("id", Tuple.Create(" id=\"", 761), Tuple.Create("\"", 782)
            
            #line 26 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
, Tuple.Create(Tuple.Create("", 766), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 766), false)
);

WriteLiteral(" data-bind=\"attr : {\'href\':");

            
            #line 26 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
                                                          Write(Html.Raw(path));

            
            #line default
            #line hidden
WriteLiteral("}, visible:");

            
            #line 26 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
                                                                                    Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" download>\r\n");

WriteLiteral("                ");

            
            #line 27 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
           Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\r\n            </a>\r\n        </div>\r\n    </div>\r\n");

            
            #line 31 "..\..\Views\Shared\DisplayTemplates\DownloadLink.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

        }
    }
}
#pragma warning restore 1591
