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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/EditorTemplates/CheckBox.cshtml")]
    public partial class _Areas_Sph_Views_App_EditorTemplates_CheckBox_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.CheckBox>
    {
        public _Areas_Sph_Views_App_EditorTemplates_CheckBox_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Areas\Sph\Views\App\EditorTemplates\CheckBox.cshtml"
  
    

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 5 "..\..\Areas\Sph\Views\App\EditorTemplates\CheckBox.cshtml"
                                         Write(Html.Raw(Model.Visible));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"controls\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n            <input");

WriteLiteral(" data-bind=\"");

            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\CheckBox.cshtml"
                          Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 265), Tuple.Create("\"", 281)
            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\CheckBox.cshtml"
      , Tuple.Create(Tuple.Create("", 270), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 270), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 282), Tuple.Create("\"", 304)
            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\CheckBox.cshtml"
                          , Tuple.Create(Tuple.Create("", 290), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 290), false)
);

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create(" name=\"", 321), Tuple.Create("\"", 339)
            
            #line 8 "..\..\Areas\Sph\Views\App\EditorTemplates\CheckBox.cshtml"
                                                                , Tuple.Create(Tuple.Create("", 328), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 328), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("            ");

            
            #line 9 "..\..\Areas\Sph\Views\App\EditorTemplates\CheckBox.cshtml"
       Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\r\n        </label>\r\n    </div>\r\n</div>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
