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

namespace Bespoke.Sph.Web.Views.Shared.EditorTemplates
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
    public partial class ListView : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ListView>
    {
        public ListView()
        {
        }
        public override void Execute()
        {



WriteLiteral("\n");


            
            #line 5 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
 if (string.IsNullOrWhiteSpace(Model.Enable))
{
    Model.Enable = "true";
}

            
            #line default
            #line hidden
WriteLiteral("\n<div data-bind=\"visible:");


            
            #line 10 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                   Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\">\n    <button class=\"btn btn-default pull-right\" data-bind=\"click : addChildItem" +
"(");


            
            #line 11 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                                                          Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral(", ");


            
            #line 11 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                                                                                                           Write(Model.ChildItemType);

            
            #line default
            #line hidden
WriteLiteral(")\">");


            
            #line 11 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                                                                                                                                  Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</button>\n    <table class=\"table table-condensed table-striped\">\n        <thead>" +
"\n            <tr>\n");


            
            #line 15 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                 foreach (var col in Model.ListViewColumnCollection)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <th>");


            
            #line 17 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                   Write(col.Label);

            
            #line default
            #line hidden
WriteLiteral("</th>\n");


            
            #line 18 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                <th></th>\n            </tr>\n        </thead>\n        <tbody data-" +
"bind=\"foreach :");


            
            #line 22 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                              Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral("\">\n            <tr>\n");


            
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
WriteLiteral("                    <td>\n                        ");


            
            #line 41 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                   Write(Html.EditorFor(f => col1.Input));

            
            #line default
            #line hidden
WriteLiteral("\n                    </td>\n");


            
            #line 43 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                <td>\n                    <a title=\"remove\" href=\"#\" data-bind=\"cl" +
"ick : $parent.removeChildItem.call($parent,$parent.");


            
            #line 45 "..\..\Views\Shared\EditorTemplates\ListView.cshtml"
                                                                                                          Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral(", $data)\">\n                        <span class=\"glyphicon glyphicon-remove\"></spa" +
"n>\n                    </a>\n                </td>\n            </tr>\n        </tb" +
"ody>\n    </table>\n</div>\n");


        }
    }
}
#pragma warning restore 1591
