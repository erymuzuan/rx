﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Views.Shared.DisplayTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/ImageElement.cshtml")]
    public partial class _ImageElement : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ImageElement>
    {
        public _ImageElement()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
 if (string.IsNullOrWhiteSpace(Model.Enable))
{
    Model.Enable = "true";
}
            
            #line default
            #line hidden
            
            #line 5 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
   
    var path = string.Format("'/sph/image/store/' + {0}()", this.Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\r\n<label");

WriteAttribute("class", Tuple.Create(" class=\"", 211), Tuple.Create("\"", 250)
, Tuple.Create(Tuple.Create("", 219), Tuple.Create("col-md-", 219), true)
            
            #line 8 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
, Tuple.Create(Tuple.Create("", 226), Tuple.Create<System.Object, System.Int32>(Model.LabelColMd ?? 4
            
            #line default
            #line hidden
, 226), false)
);

WriteLiteral(">");

            
            #line 8 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
                                          Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n<img");

WriteAttribute("alt", Tuple.Create(" alt=\"", 278), Tuple.Create("\"", 296)
            
            #line 9 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
, Tuple.Create(Tuple.Create("", 284), Tuple.Create<System.Object, System.Int32>(Model.Label
            
            #line default
            #line hidden
, 284), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 297), Tuple.Create("\"", 319)
            
            #line 9 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
, Tuple.Create(Tuple.Create("", 305), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 305), false)
);

WriteLiteral(" width =\"");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
                                                  Write(Model.Width);

            
            #line default
            #line hidden
WriteLiteral("\" height=\"");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
                                                                        Write(Model.Height);

            
            #line default
            #line hidden
WriteLiteral("\" data-bind=\"attr : {\'src\':");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
                                                                                                                Write(Html.Raw(path));

            
            #line default
            #line hidden
WriteLiteral("}, visible:");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\ImageElement.cshtml"
                                                                                                                                          Write(Html.Raw(Model.Visible));

            
            #line default
            #line hidden
WriteLiteral("\" />");

        }
    }
}
#pragma warning restore 1591
