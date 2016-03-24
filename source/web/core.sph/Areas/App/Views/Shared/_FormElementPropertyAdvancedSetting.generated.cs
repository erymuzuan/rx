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
    
    #line 2 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
    using System.Linq;
    
    #line default
    #line hidden
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 1 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Shared/_FormElementPropertyAdvancedSetting.cshtml")]
    public partial class _Areas_App_Views_Shared__FormElementPropertyAdvancedSetting_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public _Areas_App_Views_Shared__FormElementPropertyAdvancedSetting_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 6 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
  
    Type[] types = new Type[]
    {
                            typeof(TextBox),
                            typeof(ComboBox),
                            typeof(Button),
                            typeof(DownloadLink),
                            typeof(ImageElement),
                            typeof(ChildEntityListView),
                            typeof(EntityLookupElement),
                            typeof(FileUploadElement),
                            typeof(TabControl),
                            typeof(ListView)
                        };

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 21 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
 foreach (var type in types)
{
    var fe = Model.FormElements.Single(x => x.GetType() == type);
    var feName = "toolbox-advanced-settings-" + type.Name;
    var typeName = Strings.GetShortAssemblyQualifiedName(type);


            
            #line default
            #line hidden
WriteLiteral("                    <!--ko if: ko.unwrap($type) === \"");

            
            #line 27 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
                                                Write(typeName);

            
            #line default
            #line hidden
WriteLiteral("\" -->\r\n");

WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n\r\n\r\n        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1029), Tuple.Create("\"", 1046)
, Tuple.Create(Tuple.Create("", 1036), Tuple.Create("#", 1036), true)
            
            #line 31 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
, Tuple.Create(Tuple.Create("", 1037), Tuple.Create<System.Object, System.Int32>(feName
            
            #line default
            #line hidden
, 1037), false)
);

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral(">\r\n            <i");

WriteLiteral(" class=\"fa fa-chevron-down\"");

WriteLiteral("></i>\r\n            Advanced settings\r\n        </a>\r\n    </div>\r\n");

WriteLiteral("    <div");

WriteAttribute("id", Tuple.Create(" id=\"", 1186), Tuple.Create("\"", 1198)
            
            #line 36 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
, Tuple.Create(Tuple.Create("", 1191), Tuple.Create<System.Object, System.Int32>(feName
            
            #line default
            #line hidden
, 1191), false)
);

WriteLiteral(" class=\"collapse\"");

WriteLiteral(">\r\n");

            
            #line 37 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
        
            
            #line default
            #line hidden
            
            #line 37 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
          

            FormElement fe1 = fe;
            
            
            #line default
            #line hidden
            
            #line 40 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
       Write(Html.EditorFor(m => fe1));

            
            #line default
            #line hidden
            
            #line 40 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
                                     
        
            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n");

WriteLiteral("                    <!-- /ko -->\r\n");

            
            #line 44 "..\..\Areas\App\Views\Shared\_FormElementPropertyAdvancedSetting.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
