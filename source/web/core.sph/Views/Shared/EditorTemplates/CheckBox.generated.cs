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
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/CheckBox.cshtml")]
    public partial class _CheckBox : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.CheckBox>
    {
        public _CheckBox()
        {
        }
        public override void Execute()
        {
            
            #line 2 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
 if (string.IsNullOrWhiteSpace(Model.Enable))
{
    Model.Enable = "true";
}

            
            #line default
            #line hidden
            
            #line 6 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
 if (Model.IsCompact)
{

            
            #line default
            #line hidden
WriteLiteral("    <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 8 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                          Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n        <input");

WriteLiteral(" data-bind=\"");

            
            #line 9 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                      Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 284), Tuple.Create("\"", 300)
            
            #line 9 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
  , Tuple.Create(Tuple.Create("", 289), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 289), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 301), Tuple.Create("\"", 323)
            
            #line 9 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                      , Tuple.Create(Tuple.Create("", 309), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 309), false)
);

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create(" name=\"", 340), Tuple.Create("\"", 358)
            
            #line 9 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                                            , Tuple.Create(Tuple.Create("", 347), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 347), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("        ");

            
            #line 10 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
   Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\r\n    </label>\r\n");

            
            #line 12 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
}
else
{


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"control-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 16 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                             Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n            <input");

WriteLiteral(" data-bind=\"");

            
            #line 18 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                          Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 596), Tuple.Create("\"", 612)
            
            #line 18 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
      , Tuple.Create(Tuple.Create("", 601), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 601), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 613), Tuple.Create("\"", 635)
            
            #line 18 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                          , Tuple.Create(Tuple.Create("", 621), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 621), false)
);

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create(" name=\"", 652), Tuple.Create("\"", 670)
            
            #line 18 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                                                , Tuple.Create(Tuple.Create("", 659), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 659), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("            ");

            
            #line 19 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
       Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\r\n        </label>\r\n    </div>\r\n");

            
            #line 22 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"

}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

        }
    }
}
#pragma warning restore 1591
