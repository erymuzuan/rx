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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/EntityFormRenderer/_AuditTrailContent.cshtml")]
    public partial class _Areas_Sph_Views_EntityFormRenderer__AuditTrailContent_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.FormRendererViewModel>
    {
        public _Areas_Sph_Views_EntityFormRenderer__AuditTrailContent_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Areas\Sph\Views\EntityFormRenderer\_AuditTrailContent.cshtml"
 if (Model.Form.IsAuditTrailAvailable)
{

            
            #line default
            #line hidden
WriteLiteral("    <h3");

WriteLiteral(" data-i18n=\"Audit Trail\"");

WriteLiteral(">Audit Trail</h3>\r\n");

WriteLiteral("        <!--ko compose: {model:\'viewmodels/_audittrail.list\', activationData:{id:" +
"$root.entity().Id(), entity : \'");

            
            #line 6 "..\..\Areas\Sph\Views\EntityFormRenderer\_AuditTrailContent.cshtml"
                                                                                                            Write(Model.EntityDefinition.Name);

            
            #line default
            #line hidden
WriteLiteral("\'}} -->\r\n");

WriteLiteral("        <!--/ko-->\r\n");

            
            #line 8 "..\..\Areas\Sph\Views\EntityFormRenderer\_AuditTrailContent.cshtml"
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591