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
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Workflow/EditorTemplates/EmailFormElement.cshtml")]
    public partial class _Areas_Sph_Views_Workflow_EditorTemplates_EmailFormElement_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.EmailFormElement>
    {
        public _Areas_Sph_Views_Workflow_EditorTemplates_EmailFormElement_cshtml_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 80), Tuple.Create("\"", 102)
            
            #line 3 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 86), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 86), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 103), Tuple.Create("\"", 145)
, Tuple.Create(Tuple.Create("", 111), Tuple.Create("control-label", 111), true)
            
            #line 3 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create(" ", 124), Tuple.Create<System.Object, System.Int32>(Model.LabelCssClass
            
            #line default
            #line hidden
, 125), false)
);

WriteLiteral(">");

            
            #line 3 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\EmailFormElement.cshtml"
                                                                        Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n    <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n        <input");

WriteAttribute("class", Tuple.Create(" class=\"", 211), Tuple.Create("\"", 267)
            
            #line 5 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 219), Tuple.Create<System.Object, System.Int32>(Model.CssClass + " form-control "+ Model.Size
            
            #line default
            #line hidden
, 219), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 268), Tuple.Create("\"", 290)
            
            #line 5 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 276), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 276), false)
);

WriteLiteral(" \r\n            data-bind=\"");

            
            #line 6 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\EmailFormElement.cshtml"
                   Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 367), Tuple.Create("\"", 388)
            
            #line 6 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\EmailFormElement.cshtml"
, Tuple.Create(Tuple.Create("", 372), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 372), false)
);

WriteLiteral(" type=\"email\"");

WriteAttribute("name", Tuple.Create(" name=\"", 402), Tuple.Create("\"", 420)
            
            #line 6 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\EmailFormElement.cshtml"
                                    , Tuple.Create(Tuple.Create("", 409), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 409), false)
);

WriteLiteral(" />\r\n    </div>\r\n</div>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
