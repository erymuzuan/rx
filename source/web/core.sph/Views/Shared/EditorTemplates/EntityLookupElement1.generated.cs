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
    
    #line 1 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/EntityLookupElement.cshtml")]
    public partial class _Views_Shared_EditorTemplates_EntityLookupElement_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.EntityLookupElement>
    {
        public _Views_Shared_EditorTemplates_EntityLookupElement_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
  
    if (string.IsNullOrWhiteSpace(Model.Enable))
    {
        Model.Enable = "true";
    }
    
    var columns = string.Join(",", Model.LookupColumnCollection.Select(c => $"'{c}'"));
    Console.WriteLine(columns);

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n");

            
            #line 14 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
 if (Model.IsCompact)
{


            
            #line default
            #line hidden
WriteLiteral("    <a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral("\r\n       data-bind=\"tooltip :\'");

            
            #line 18 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                       Write(Model.Tooltip);

            
            #line default
            #line hidden
WriteLiteral("\',lookup : {entity: \'");

            
            #line 18 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                          Write(Model.Entity);

            
            #line default
            #line hidden
WriteLiteral("\', member : \'");

            
            #line 18 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                                                    Write(Model.ValueMemberPath);

            
            #line default
            #line hidden
WriteLiteral("\', value : ");

            
            #line 18 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                                                                                     Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral(", columns : [");

            
            #line 18 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                                                                                                                                                 Write(Html.Raw(columns));

            
            #line default
            #line hidden
WriteLiteral("]}\"");

WriteAttribute("id", Tuple.Create("\r\n       id=\"", 569), Tuple.Create("\"", 598)
            
            #line 19 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
, Tuple.Create(Tuple.Create("", 582), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 582), false)
);

WriteLiteral(">\r\n        <!-- ko text :");

            
            #line 20 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                 Write(Model.DisplayMemberPath);

            
            #line default
            #line hidden
WriteLiteral(" -->\r\n        <!-- /ko -->\r\n        <i");

WriteLiteral(" class=\"fa fa-search\"");

WriteLiteral("></i>\r\n    </a>\r\n");

            
            #line 24 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
}
else
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" data-bind=\"visible:");

            
            #line 27 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                       Write(Model.Visible);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" data-i18n=\"");

            
            #line 28 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                     Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteAttribute("for", Tuple.Create(" for=\"", 840), Tuple.Create("\"", 862)
            
            #line 28 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
, Tuple.Create(Tuple.Create("", 846), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 846), false)
);

WriteAttribute("class", Tuple.Create(" class=\"", 863), Tuple.Create("\"", 891)
            
            #line 28 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
, Tuple.Create(Tuple.Create("", 871), Tuple.Create<System.Object, System.Int32>(Model.LabelCssClass
            
            #line default
            #line hidden
, 871), false)
);

WriteLiteral(">");

            
            #line 28 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                                                       Write(Model.Label);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n\r\n        <script");

WriteLiteral(" type=\"text/json\"");

WriteAttribute("id", Tuple.Create(" id=\"", 949), Tuple.Create("\"", 984)
            
            #line 30 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
, Tuple.Create(Tuple.Create("", 954), Tuple.Create<System.Object, System.Int32>($"filter-{Model.ElementId}"
            
            #line default
            #line hidden
, 954), false)
);

WriteLiteral(">\r\n            \r\n        </script>\r\n\r\n\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1037), Tuple.Create("\"", 1070)
            
            #line 35 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
, Tuple.Create(Tuple.Create("", 1045), Tuple.Create<System.Object, System.Int32>(Model.InputPanelCssClass
            
            #line default
            #line hidden
, 1045), false)
);

WriteLiteral(">\r\n            <a");

WriteLiteral(" class=\"btn btn-link\"");

WriteLiteral(" \r\n               data-bind=\"tooltip :\'");

            
            #line 37 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                               Write(Model.Tooltip);

            
            #line default
            #line hidden
WriteLiteral("\',lookup : {entity: \'");

            
            #line 37 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                                  Write(Model.Entity);

            
            #line default
            #line hidden
WriteLiteral("\', member : \'");

            
            #line 37 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                                                            Write(Model.ValueMemberPath);

            
            #line default
            #line hidden
WriteLiteral("\', value : ");

            
            #line 37 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                                                                                             Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral(", columns : [");

            
            #line 37 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                                                                                                                                                         Write(Html.Raw(columns));

            
            #line default
            #line hidden
WriteLiteral("]}\"");

WriteAttribute("id", Tuple.Create("\r\n               id=\"", 1323), Tuple.Create("\"", 1360)
            
            #line 38 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
, Tuple.Create(Tuple.Create("", 1344), Tuple.Create<System.Object, System.Int32>(Model.ElementId
            
            #line default
            #line hidden
, 1344), false)
);

WriteLiteral(">\r\n");

            
            #line 39 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                
            
            #line default
            #line hidden
            
            #line 39 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                 if (!string.IsNullOrWhiteSpace(Model.DisplayTemplate))
                {
                    
            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
               Write(Html.Raw(Model.DisplayTemplate));

            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                                    ;
                }
                else
                {

            
            #line default
            #line hidden
WriteLiteral("                     <!-- ko text: ");

            
            #line 45 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                              Write(Model.DisplayMemberPath);

            
            #line default
            #line hidden
WriteLiteral(" -->\r\n");

WriteLiteral("                     <!-- /ko -->\r\n");

            
            #line 47 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                <i");

WriteLiteral(" class=\"fa fa-search\"");

WriteLiteral("></i>\r\n            </a>\r\n        </div>\r\n");

            
            #line 51 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
        
            
            #line default
            #line hidden
            
            #line 51 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
         if (!string.IsNullOrWhiteSpace(Model.HelpText))
        {

            
            #line default
            #line hidden
WriteLiteral("            <span");

WriteLiteral(" class=\"help-block\"");

WriteLiteral(">");

            
            #line 53 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
                                Write(Model.HelpText);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

            
            #line 54 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 56 "..\..\Views\Shared\EditorTemplates\EntityLookupElement.cshtml"

}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
