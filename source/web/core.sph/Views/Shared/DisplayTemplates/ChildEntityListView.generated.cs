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
    
    #line 1 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/ChildEntityListView.cshtml")]
    public partial class _Views_Shared_DisplayTemplates_ChildEntityListView_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ChildEntityListView>
    {
        public _Views_Shared_DisplayTemplates_ChildEntityListView_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
 if (string.IsNullOrWhiteSpace(Model.Enable))
{
    Model.Enable = "true";
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" data-bind=\"visible:");

            
            #line 10 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                   Write(Html.Raw(Model.Visible));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n    <a");

WriteLiteral(" class=\"btn btn-default pull-right\"");

WriteLiteral(">");

            
            #line 11 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                                     Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n    <table");

WriteLiteral(" class=\"table table-condensed table-striped\"");

WriteLiteral(">\r\n        <thead>\r\n            <tr>\r\n");

            
            #line 15 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                 foreach (var col in Model.ViewColumnCollection)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <th>");

            
            #line 17 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                   Write(col.Header);

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n");

            
            #line 18 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </tr>\r\n        </thead>\r\n        <tbody");

WriteLiteral(" data-bind=\"foreach :");

            
            #line 21 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                              Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n            <tr>\r\n");

            
            #line 23 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                
            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                 foreach (var col in Model.ViewColumnCollection)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <td");

WriteLiteral(" data-bind=\"text:");

            
            #line 25 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                                   Write(col.Path);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral("></td>\r\n");

            
            #line 26 "..\..\Views\Shared\DisplayTemplates\ChildEntityListView.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </tr>\r\n        </tbody>\r\n    </table>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
