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
    
    #line 2 "..\..\Areas\Sph\Views\Home\_header.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/Home/_header.cshtml")]
    public partial class _Areas_Sph_Views_Home__header_cshtml : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.SphIndexViewModel>
    {
        public _Areas_Sph_Views_Home__header_cshtml()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n\r\n<!-- BEGIN HEADER -->\r\n<div");

WriteLiteral(" class=\"page-header md-shadow-z-1-i navbar navbar-fixed-top\"");

WriteLiteral(">\r\n    <!-- BEGIN HEADER INNER -->\r\n    <div");

WriteLiteral(" class=\"page-header-inner\"");

WriteLiteral(">\r\n        <!-- BEGIN LOGO -->\r\n        <div");

WriteLiteral(" class=\"page-logo\"");

WriteLiteral(">\r\n            <a");

WriteAttribute("href", Tuple.Create(" href=\"", 321), Tuple.Create("\"", 362)
, Tuple.Create(Tuple.Create("", 328), Tuple.Create("/sph#", 328), true)
            
            #line 11 "..\..\Areas\Sph\Views\Home\_header.cshtml"
, Tuple.Create(Tuple.Create("", 333), Tuple.Create<System.Object, System.Int32>(Model.Profile?.StartModule
            
            #line default
            #line hidden
, 333), false)
);

WriteLiteral(">\r\n                <img");

WriteLiteral(" src=\"/Images/sph_32x32.png\"");

WriteLiteral(" alt=\"logo\"");

WriteLiteral(" class=\"logo-default\"");

WriteLiteral(" />\r\n            </a>\r\n            <div");

WriteLiteral(" class=\"menu-toggler sidebar-toggler hide\"");

WriteLiteral(">\r\n            </div>\r\n        </div>\r\n        <!-- END LOGO -->\r\n        <!-- BE" +
"GIN RESPONSIVE MENU TOGGLER -->\r\n        <a");

WriteLiteral(" href=\"javascript:;\"");

WriteLiteral(" class=\"menu-toggler responsive-toggler\"");

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral(" data-target=\".navbar-collapse\"");

WriteLiteral(">\r\n        </a>\r\n        <!-- END RESPONSIVE MENU TOGGLER -->\r\n        <div");

WriteLiteral(" class=\"pull-left\"");

WriteLiteral(" style=\"padding-top: 10px\"");

WriteLiteral(">\r\n            <a");

WriteAttribute("href", Tuple.Create(" href=\"", 901), Tuple.Create("\"", 942)
, Tuple.Create(Tuple.Create("", 908), Tuple.Create("/sph#", 908), true)
            
            #line 23 "..\..\Areas\Sph\Views\Home\_header.cshtml"
, Tuple.Create(Tuple.Create("", 913), Tuple.Create<System.Object, System.Int32>(Model.Profile?.StartModule
            
            #line default
            #line hidden
, 913), false)
);

WriteLiteral(" style=\"font-size:24px;color:white;\"");

WriteLiteral(">REACTIVE DEVELOPER</a>\r\n        </div>\r\n        <!-- BEGIN TOP NAVIGATION MENU -" +
"->\r\n        <div");

WriteLiteral(" class=\"top-menu\"");

WriteLiteral(">\r\n            <ul");

WriteLiteral(" class=\"nav navbar-nav pull-right\"");

WriteLiteral(">\r\n\r\n");

            
            #line 29 "..\..\Areas\Sph\Views\Home\_header.cshtml"
                
            
            #line default
            #line hidden
            
            #line 29 "..\..\Areas\Sph\Views\Home\_header.cshtml"
                 if (Model.Designation.IsMessageVisible)
                {
                    
            
            #line default
            #line hidden
            
            #line 31 "..\..\Areas\Sph\Views\Home\_header.cshtml"
               Write(Html.Partial("_messagesHeader"));

            
            #line default
            #line hidden
            
            #line 31 "..\..\Areas\Sph\Views\Home\_header.cshtml"
                                                    

            
            #line default
            #line hidden
WriteLiteral("\t\t\t\t\t<!-- END INBOX DROPDOWN -->\r\n");

            
            #line 33 "..\..\Areas\Sph\Views\Home\_header.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("\r\n                <!-- BEGIN USER LOGIN DROPDOWN -->\r\n                <!-- DOC: A" +
"pply \"dropdown-dark\" class after below \"dropdown-extended\" to change the dropdow" +
"n styte -->\r\n                <li");

WriteLiteral(" class=\"dropdown dropdown-user\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" href=\"javascript:;\"");

WriteLiteral(" class=\"dropdown-toggle\"");

WriteLiteral(" data-toggle=\"dropdown\"");

WriteLiteral(" data-hover=\"dropdown\"");

WriteLiteral(" data-close-others=\"true\"");

WriteLiteral(">\r\n                        <i");

WriteLiteral(" class=\"icon-user\"");

WriteLiteral("></i>\r\n                        <span");

WriteLiteral(" class=\"username username-hide-on-mobile\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 41 "..\..\Areas\Sph\Views\Home\_header.cshtml"
                       Write(User.Identity.Name);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </span>\r\n                        <i");

WriteLiteral(" class=\"fa fa-angle-down\"");

WriteLiteral("></i>\r\n                    </a>\r\n                    <ul");

WriteLiteral(" class=\"dropdown-menu dropdown-menu-default\"");

WriteLiteral(">\r\n                        <li>\r\n                            <a");

WriteLiteral(" href=\"#user.profile\"");

WriteLiteral(">\r\n                                <i");

WriteLiteral(" class=\"icon-user\"");

WriteLiteral("></i> My Profile\r\n                            </a>\r\n                        </li>" +
"\r\n                   \r\n                        <li>\r\n                           " +
" <a");

WriteLiteral(" href=\"javascript:;\"");

WriteLiteral(" id=\"rx-context-help\"");

WriteLiteral(">\r\n                                <i");

WriteLiteral(" class=\"icon-question\"");

WriteLiteral("></i> Help \r\n                            </a>\r\n                        </li>\r\n   " +
"                     <li");

WriteLiteral(" class=\"divider\"");

WriteLiteral(">\r\n                        </li>\r\n                    \r\n                        <" +
"li>\r\n                            <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2697), Tuple.Create("\"", 2738)
            
            #line 61 "..\..\Areas\Sph\Views\Home\_header.cshtml"
, Tuple.Create(Tuple.Create("", 2704), Tuple.Create<System.Object, System.Int32>(Url.Action("Logoff","SphAccount")
            
            #line default
            #line hidden
, 2704), false)
);

WriteLiteral(">\r\n                                <i");

WriteLiteral(" class=\"icon-key\"");

WriteLiteral(@"></i> Log Out
                            </a>
                        </li>
                    </ul>
                </li>
                <!-- END USER LOGIN DROPDOWN -->
                <!-- BEGIN QUICK SIDEBAR TOGGLER -->
                <!-- DOC: Apply ""dropdown-dark"" class after below ""dropdown-extended"" to change the dropdown styte -->
                <li");

WriteLiteral(" class=\"dropdown dropdown-quick-sidebar-toggler\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" href=\"javascript:;\"");

WriteLiteral(" class=\"dropdown-toggle\"");

WriteLiteral(">\r\n                        <i");

WriteLiteral(" class=\"icon-logout\"");

WriteLiteral("></i>\r\n                    </a>\r\n                </li>\r\n                <!-- END " +
"QUICK SIDEBAR TOGGLER -->\r\n            </ul>\r\n        </div>\r\n        <!-- END T" +
"OP NAVIGATION MENU -->\r\n    </div>\r\n    <!-- END HEADER INNER -->\r\n</div>\r\n<!-- " +
"END HEADER -->");

        }
    }
}
#pragma warning restore 1591
