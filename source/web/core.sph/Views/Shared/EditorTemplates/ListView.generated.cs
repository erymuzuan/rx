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
    
    #line 1 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/ListView.cshtml")]
    public partial class _Views_Shared_EditorTemplates_ListView_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ListView>
    {
        public _Views_Shared_EditorTemplates_ListView_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
 if (string.IsNullOrWhiteSpace(Model.Enable))
{
    Model.Enable = "true";
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" data-bind=\"visible:");

            
            #line 10 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                   Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n    <button");

WriteLiteral(" data-i18n=\"");

            
            #line 11 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                  Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" class=\"btn btn-default pull-right\"");

WriteLiteral(" data-bind=\"click : addChildItem(");

            
            #line 11 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                                                                                   Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral(", ");

            
            #line 11 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                                                                                                                                    Write(Model.ChildItemType);

            
            #line default
            #line hidden
WriteLiteral(")\"");

WriteLiteral(">");

            
            #line 11 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                                                                                                                                                           Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n    <table");

WriteLiteral(" class=\"table table-condensed table-striped\"");

WriteLiteral(">\r\n        <thead>\r\n            <tr>\r\n");

            
            #line 15 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                 foreach (var col in Model.ListViewColumnCollection)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <th");

WriteLiteral(" data-i18n=\"");

            
            #line 17 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                              Write(col.Label);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">");

            
            #line 17 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                          Write(col.Label);

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n");

            
            #line 18 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                <th></th>\r\n            </tr>\r\n        </thead>\r\n        <tbody");

WriteLiteral(" data-bind=\"foreach :");

            
            #line 22 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                              Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n            <tr>\r\n");

            
            #line 24 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                
            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                 foreach (var col in Model.ListViewColumnCollection)
                {
                    var col1 = col;
                    col.Input.Path = col.Path;
                    if (string.IsNullOrWhiteSpace(col.Input.Visible))
                    {
                        col.Input.Visible = "true";
                    }
                    if (string.IsNullOrWhiteSpace(col.Input.Enable))
                    {
                        col.Input.Enable = "true";
                    }
                    col.Input.IsCompact = true;
                    col.Input.ElementId = string.Empty;
                    col.Input.IsUniqueName = true;


            
            #line default
            #line hidden
WriteLiteral("                    <td>\r\n");

WriteLiteral("                        ");

            
            #line 41 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                    Write(col1.Input.UseDisplayTemplate ? Html.DisplayFor(f => col1.Input) : Html.EditorFor(f => col1.Input));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        \r\n                    </td>\r\n");

            
            #line 44 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                <td>\r\n                    <a");

WriteLiteral(" title=\"remove\"");

WriteLiteral(" href=\"#\"");

WriteLiteral(" data-bind=\"click : $parent.removeChildItem.call($parent,$parent.");

            
            #line 46 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                                                                                          Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral(", $data)\"");

WriteLiteral(">\r\n                        <span");

WriteLiteral(" class=\"glyphicon glyphicon-remove\"");

WriteLiteral("></span>\r\n                    </a>\r\n                </td>\r\n            </tr>\r\n   " +
"     </tbody>\r\n    </table>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
