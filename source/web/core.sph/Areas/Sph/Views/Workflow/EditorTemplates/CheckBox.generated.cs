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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Workflow/EditorTemplates/CheckBox.cshtml")]
    public partial class _Areas_Sph_Views_Workflow_EditorTemplates_CheckBox_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.CheckBox>
    {
        public _Areas_Sph_Views_Workflow_EditorTemplates_CheckBox_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\CheckBox.cshtml"
  
    

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n            <input");

WriteLiteral(" data-bind=\"");

            
            #line 8 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\CheckBox.cshtml"
                          Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 220), Tuple.Create("\"", 236)
            
            #line 8 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\CheckBox.cshtml"
      , Tuple.Create(Tuple.Create("", 225), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 225), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 237), Tuple.Create("\"", 259)
            
            #line 8 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\CheckBox.cshtml"
                          , Tuple.Create(Tuple.Create("", 245), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 245), false)
);

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create(" name=\"", 276), Tuple.Create("\"", 294)
            
            #line 8 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\CheckBox.cshtml"
                                                                , Tuple.Create(Tuple.Create("", 283), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 283), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("            ");

            
            #line 9 "..\..\Areas\Sph\Views\Workflow\EditorTemplates\CheckBox.cshtml"
       Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\r\n        </label>\r\n    </div>\r\n</div>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
