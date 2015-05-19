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
    
    #line 1 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/CheckBox.cshtml")]
    public partial class _Views_Shared_EditorTemplates_CheckBox_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.CheckBox>
    {
        public _Views_Shared_EditorTemplates_CheckBox_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
  
    var originalPath = Model.Path.ToEmptyString().Replace("().", ".");
    if (string.IsNullOrWhiteSpace(Model.Enable))
    {
        Model.Enable = "true";
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 10 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
 if (Model.IsCompact)
{

            
            #line default
            #line hidden
WriteLiteral("    <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 12 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                          Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n        <input");

WriteLiteral(" data-i18n=\"");

            
            #line 13 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                     Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" data-bind=\"");

            
            #line 13 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                               Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 430), Tuple.Create("\"", 446)
            
            #line 13 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                           , Tuple.Create(Tuple.Create("", 435), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 435), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 447), Tuple.Create("\"", 469)
            
            #line 13 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                               , Tuple.Create(Tuple.Create("", 455), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 455), false)
);

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create(" name=\"", 486), Tuple.Create("\"", 506)
            
            #line 13 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                                                                     , Tuple.Create(Tuple.Create("", 493), Tuple.Create<System.Object, System.Int32>(originalPath
            
            #line default
            #line hidden
, 493), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("        ");

            
            #line 14 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
   Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\r\n    </label>\r\n");

            
            #line 16 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
}
else
{


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible:");

            
            #line 20 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                          Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 639), Tuple.Create("\"", 712)
, Tuple.Create(Tuple.Create("", 647), Tuple.Create("col-md-offset-", 647), true)
            
            #line 21 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
, Tuple.Create(Tuple.Create("", 661), Tuple.Create<System.Object, System.Int32>(@Model.LabelColMd ?? 4
            
            #line default
            #line hidden
, 661), false)
            
            #line 21 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
, Tuple.Create(Tuple.Create(" ", 686), Tuple.Create<System.Object, System.Int32>(Model.InputPanelCssClass
            
            #line default
            #line hidden
, 687), false)
);

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                <label");

WriteLiteral(" class=\"checkbox\"");

WriteLiteral(">\r\n                    <input");

WriteLiteral(" data-bind=\"");

            
            #line 24 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                  Write(Html.Raw(Model.GetKnockoutBindingExpression()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("id", Tuple.Create(" id=\"", 882), Tuple.Create("\"", 898)
            
            #line 24 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
              , Tuple.Create(Tuple.Create("", 887), Tuple.Create<System.Object, System.Int32>(Model.Path
            
            #line default
            #line hidden
, 887), false)
);

WriteAttribute("title", Tuple.Create(" title=\"", 899), Tuple.Create("\"", 921)
            
            #line 24 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                  , Tuple.Create(Tuple.Create("", 907), Tuple.Create<System.Object, System.Int32>(Model.Tooltip
            
            #line default
            #line hidden
, 907), false)
);

WriteLiteral(" type=\"checkbox\"");

WriteAttribute("name", Tuple.Create("\r\n                           name=\"", 938), Tuple.Create("\"", 986)
            
            #line 25 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
, Tuple.Create(Tuple.Create("", 973), Tuple.Create<System.Object, System.Int32>(originalPath
            
            #line default
            #line hidden
, 973), false)
);

WriteLiteral("/>\r\n                    <span");

WriteLiteral(" data-i18n=\"");

            
            #line 26 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">");

            
            #line 26 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
                                              Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                    \r\n                </label>\r\n            </div>\r\n    " +
"    </div>\r\n    </div>\r\n");

            
            #line 32 "..\..\Views\Shared\EditorTemplates\CheckBox.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

        }
    }
}
#pragma warning restore 1591
