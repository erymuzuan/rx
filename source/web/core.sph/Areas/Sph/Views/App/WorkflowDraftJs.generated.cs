﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Areas.Sph.Views.App
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/Sph/Views/App/WorkflowDraftJs.cshtml")]
    public partial class WorkflowDraftJs : System.Web.Mvc.WebViewPage<dynamic>
    {
        public WorkflowDraftJs()
        {
        }
        public override void Execute()
        {
            
            #line 1 "..\..\Areas\Sph\Views\App\WorkflowDraftJs.cshtml"
  
    Layout = null;
    var userName = @User.Identity.Name;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(" data-script=\"true\"");

WriteLiteral(">\r\n    define([\'services/datacontext\'], function (context) {\r\n        var isBusy " +
"= ko.observable(false),\r\n            user = ko.observable(),\r\n            activa" +
"te = function () {\r\n                user(\'");

            
            #line 11 "..\..\Areas\Sph\Views\App\WorkflowDraftJs.cshtml"
                 Write(Html.Raw(userName));

            
            #line default
            #line hidden
WriteLiteral(@"');
                var query = String.format(""CreatedBy eq '{0}' "", user());
                var tcs = new $.Deferred();
                context.loadAsync(""Workflow"", query)
                    .then(function (lo) {
                        isBusy(false);
                        vm.workflow(lo.itemCollection);
                        tcs.resolve(true);
                    });
                return tcs.promise();
            };

        var vm = {
            activate: activate,
            workflow: ko.observableArray(),
            user: user

        };

        return vm;
    });
</script>

");

        }
    }
}
#pragma warning restore 1591