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
    
    #line 1 "..\..\Views\Shared\EditorTemplates\ChildView.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/ChildView.cshtml")]
    public partial class _Views_Shared_EditorTemplates_ChildView_cshtml_ : System.Web.Mvc.WebViewPage<Bespoke.Sph.Domain.ChildView>
    {
        public _Views_Shared_EditorTemplates_ChildView_cshtml_()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Shared\EditorTemplates\ChildView.cshtml"
  
    var defaultPath = Model.Path == "." || string.IsNullOrWhiteSpace(Model.Path);

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 6 "..\..\Views\Shared\EditorTemplates\ChildView.cshtml"
 if (!defaultPath)
{

            
            #line default
            #line hidden
WriteLiteral("    <!-- ko with : ");

            
            #line 8 "..\..\Views\Shared\EditorTemplates\ChildView.cshtml"
              Write(Model.Path.ConvertJavascriptObjectToFunction());

            
            #line default
            #line hidden
WriteLiteral(" -->\r\n");

            
            #line 9 "..\..\Views\Shared\EditorTemplates\ChildView.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("    <!-- ko compose : : {view : \'");

            
            #line 10 "..\..\Views\Shared\EditorTemplates\ChildView.cshtml"
                             Write(Model.PartialView);

            
            #line default
            #line hidden
WriteLiteral(".html\'}-->\r\n    <!-- /ko-->\r\n\r\n");

            
            #line 13 "..\..\Views\Shared\EditorTemplates\ChildView.cshtml"
 if (!defaultPath)
{

            
            #line default
            #line hidden
WriteLiteral("    <!-- /ko -->\r\n");

            
            #line 16 "..\..\Views\Shared\EditorTemplates\ChildView.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
