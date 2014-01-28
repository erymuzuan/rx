﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Areas.App.Views.Header
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
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/Header/Html.cshtml")]
    public partial class Html : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Html()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Areas\App\Views\Header\Html.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" id=\"header-navbar\"");

WriteLiteral(" class=\"navbar\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"navbar-inner\"");

WriteLiteral(">\r\n\r\n        <img");

WriteLiteral(" src=\"/Images/sph_32x32.png\"");

WriteLiteral(" class=\"pull-left\"");

WriteLiteral(" alt=\"Logo\"");

WriteLiteral(" />\r\n");

            
            #line 8 "..\..\Areas\App\Views\Header\Html.cshtml"
        
            
            #line default
            #line hidden
            
            #line 8 "..\..\Areas\App\Views\Header\Html.cshtml"
         if (User.Identity.IsAuthenticated)
        {

            
            #line default
            #line hidden
WriteLiteral("            <button");

WriteLiteral(" class=\"btn\"");

WriteLiteral(" id=\"drawer-menu\"");

WriteLiteral(" title=\"Open menu\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"glyphicon glyphicon-align-justify\"");

WriteLiteral("></span>\r\n            </button>\r\n");

            
            #line 13 "..\..\Areas\App\Views\Header\Html.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        <!-- Site name for smallar screens -->\r\n        <a");

WriteLiteral(" data-bind=\"attr : {href : \'#/\' + config.startModule}\"");

WriteLiteral(" class=\"logo\"");

WriteLiteral(">");

            
            #line 15 "..\..\Areas\App\Views\Header\Html.cshtml"
                                                                         Write(Bespoke.Sph.Domain.ConfigurationManager.ApplicationName);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n\r\n        <!-- Navigation starts -->\r\n        <div");

WriteLiteral(" class=\"nav-header\"");

WriteLiteral(">\r\n\r\n");

            
            #line 20 "..\..\Areas\App\Views\Header\Html.cshtml"
            
            
            #line default
            #line hidden
            
            #line 20 "..\..\Areas\App\Views\Header\Html.cshtml"
             if (User.Identity.IsAuthenticated)
            {

            
            #line default
            #line hidden
WriteLiteral("                <!-- Links -->\r\n");

WriteLiteral("                <div");

WriteLiteral(" class=\"btn-group\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" id=\"user-profile-split-button\"");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"btn btn-link dropdown-toggle\"");

WriteLiteral(" data-toggle=\"dropdown\"");

WriteLiteral(">\r\n                        <img");

WriteLiteral(" src=\"/Images/user_24_white.png\"");

WriteLiteral(" alt=\"\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 26 "..\..\Areas\App\Views\Header\Html.cshtml"
                   Write(User.Identity.Name);

            
            #line default
            #line hidden
WriteLiteral("\r\n                        <span");

WriteLiteral(" class=\"caret\"");

WriteLiteral("></span>\r\n                    </button>\r\n                    <ul");

WriteLiteral(" class=\"dropdown-menu\"");

WriteLiteral(">\r\n                        <li><a");

WriteLiteral(" href=\"#/user.profile\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-user\"");

WriteLiteral("></i>Profile</a></li>\r\n                        <li><a");

WriteLiteral(" href=\"#/user.setting\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-cogs\"");

WriteLiteral("></i>Settings</a></li>\r\n                        <li><a");

WriteAttribute("href", Tuple.Create(" href=\"", 1414), Tuple.Create("\"", 1476)
            
            #line 32 "..\..\Areas\App\Views\Header\Html.cshtml"
, Tuple.Create(Tuple.Create("", 1421), Tuple.Create<System.Object, System.Int32>(Url.Action("Logoff", "SphAccount", new {area = "Sph"})
            
            #line default
            #line hidden
, 1421), false)
);

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-power-off\"");

WriteLiteral("></i>Logout</a></li>\r\n                    </ul>\r\n                </div>\r\n");

WriteLiteral("                <!-- Notifications -->\r\n");

WriteLiteral("                <ul");

WriteLiteral(" class=\"nav navbar-nav\"");

WriteLiteral(">\r\n                    <!-- ko compose : \'viewmodels/messages\' -->\r\n             " +
"       <!-- /ko -->\r\n                    <!-- ko compose : \'viewmodels/search\' -" +
"->\r\n                    <!-- /ko -->\r\n                </ul>\r\n");

            
            #line 42 "..\..\Areas\App\Views\Header\Html.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            ");

            
            #line 43 "..\..\Areas\App\Views\Header\Html.cshtml"
             if (!User.Identity.IsAuthenticated)
            {

            
            #line default
            #line hidden
WriteLiteral("                <ul");

WriteLiteral(" class=\"nav navbar-nav\"");

WriteLiteral(">\r\n                    <li><a");

WriteLiteral(" id=\"log-in\"");

WriteAttribute("href", Tuple.Create(" href=\"", 2044), Tuple.Create("\"", 2106)
            
            #line 46 "..\..\Areas\App\Views\Header\Html.cshtml"
, Tuple.Create(Tuple.Create("", 2051), Tuple.Create<System.Object, System.Int32>(Url.Action("Login", "SphAccount", new { area = "Sph"})
            
            #line default
            #line hidden
, 2051), false)
);

WriteLiteral(" class=\"login-label\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-sign-in\"");

WriteLiteral("></i>  Log Masuk </a></li>\r\n                </ul>\r\n");

            
            #line 48 "..\..\Areas\App\Views\Header\Html.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </div>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
