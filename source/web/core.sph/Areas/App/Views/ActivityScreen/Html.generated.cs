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

namespace Bespoke.Sph.Web.Areas.App.Views.ActivityScreen
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
    
    #line 1 "..\..\Areas\App\Views\ActivityScreen\Html.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/ActivityScreen/Html.cshtml")]
    public partial class Html : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public Html()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Areas\App\Views\ActivityScreen\Html.cshtml"
  
    Layout = null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n\r\n<div");

WriteLiteral(" class=\"view-model-modal\"");

WriteLiteral(" id=\"activity-screen-modal\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"modal-dialog\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"modal-content\"");

WriteLiteral(">\r\n            \r\n            <div");

WriteLiteral(" class=\"modal-header\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" type=\"button\"");

WriteLiteral(" class=\"close\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(" data-bind=\"click : cancelClick\"");

WriteLiteral(">&times;</button>\r\n                <h3>Screen Activity</h3>\r\n            </div>\r\n" +
"            <div");

WriteLiteral(" class=\"modal-body\"");

WriteLiteral(">\r\n                <ul");

WriteLiteral(" class=\"nav nav-tabs\"");

WriteLiteral(">\r\n                    <li");

WriteLiteral(" class=\"active\"");

WriteLiteral("><a");

WriteLiteral(" href=\"#form-general\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">General Info</a></li>\r\n                    <li><a");

WriteLiteral(" href=\"#invite-message\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Invite</a></li>\r\n                </ul>\r\n                <div");

WriteLiteral(" class=\"tab-content\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" id=\"form-general\"");

WriteLiteral(" class=\"tab-pane active\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                            <form");

WriteLiteral(" class=\"form-horizontal col-lg-12\"");

WriteLiteral(" id=\"activity-screen-modal-form\"");

WriteLiteral(" data-bind=\"with: activity\"");

WriteLiteral(">\r\n                                <div");

WriteLiteral(" class=\"row\"");

WriteLiteral(">\r\n                                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                        <label");

WriteLiteral(" for=\"screen-vpath\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Name</label>\r\n                                        <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                                            <input");

WriteLiteral(" pattern=\"^[A-Za-z_][A-Za-z0-9_ ]*$\"");

WriteLiteral(" required");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: Name\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"screen-vpath\"");

WriteLiteral(" placeholder=\"Name\"");

WriteLiteral(">\r\n                                        </div>\r\n                              " +
"      </div>\r\n                                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                        <label");

WriteLiteral(" for=\"screen-next\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Next</label>\r\n                                        <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                                            <select");

WriteLiteral(" id=\"screen-next\"");

WriteLiteral(@"
                                                    data-bind=""value:NextActivityWebId,
                                                    uniqueName:true,
                                                    options: $root.wd().ActivityCollection,
                                                    optionsText:'Name',
                                                    optionsValue:'WebId',
                                                    optionsCaption:'Please select next activity'""");

WriteLiteral("\r\n                                                    class=\"form-control\"");

WriteLiteral("></select>\r\n                                        </div>\r\n                     " +
"               </div>\r\n                                </div>\r\n                 " +
"               <h3>Screen Confirmation Options</h3>\r\n                           " +
"     <div");

WriteLiteral(" id=\"form-confirmation-options\"");

WriteLiteral(" class=\"row\"");

WriteLiteral(" data-bind=\"with : ConfirmationOptions\"");

WriteLiteral(">\r\n                                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                        <label");

WriteLiteral(" for=\"confirmation-type\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Type</label>\r\n                                        <div");

WriteLiteral(" class=\"col-lg-6\"");

WriteLiteral(">\r\n                                            <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"confirmation-type\"");

WriteLiteral(" data-bind=\"value:Type\"");

WriteLiteral(">\r\n                                                <option");

WriteLiteral(" value=\"Message\"");

WriteLiteral(">Show Message</option>\r\n                                                <option");

WriteLiteral(" value=\"Website\"");

WriteLiteral(">Redirect To Website</option>\r\n                                            </sele" +
"ct>\r\n                                        </div>\r\n                           " +
"         </div>\r\n                                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                        <label");

WriteLiteral(" for=\"Value\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Value</label>\r\n                                        <div");

WriteLiteral(" class=\"col-lg-9\"");

WriteLiteral(">\r\n                                            <textarea");

WriteLiteral(" data-bind=\"value:Value\"");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" id=\"Value\"");

WriteLiteral(" placeholder=\"Message / Url....\"");

WriteLiteral(@"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>

                    <div");

WriteLiteral(" id=\"invite-message\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(" data-bind=\"with : activity\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 70 "..\..\Areas\App\Views\ActivityScreen\Html.cshtml"
                   Write(Html.Partial("_ScreenPerformer"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        <form");

WriteLiteral(" class=\"form-horizontal\"");

WriteLiteral(" role=\"form\"");

WriteLiteral(">\r\n                            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"scr-ims\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Subject</label>\r\n                                <div");

WriteLiteral(" class=\"col-lg-9\"");

WriteLiteral(">\r\n                                    <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: InvitationMessageSubject\"");

WriteLiteral("\r\n                                           required");

WriteLiteral("\r\n                                           placeholder=\"Invitation message subj" +
"ect\"");

WriteLiteral("\r\n                                           class=\"form-control razor-field\"");

WriteLiteral(" id=\"scr-ims\"");

WriteLiteral(">\r\n                                </div>\r\n                            </div>\r\n  " +
"                          <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"scr-imb\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Body</label>\r\n                                <div");

WriteLiteral(" class=\"col-lg-9\"");

WriteLiteral(">\r\n                                    <textarea");

WriteLiteral(" data-bind=\"value: InvitationMessageBody\"");

WriteLiteral("\r\n                                              required");

WriteLiteral("\r\n                                              placeholder=\"Invitation message b" +
"ody\"");

WriteLiteral("\r\n                                              class=\"form-control razor-field\"");

WriteLiteral(" id=\"scr-imb\"");

WriteLiteral("></textarea>\r\n                                </div>\r\n                           " +
" </div>\r\n                            <h4>Cancellation message</h4>\r\n            " +
"                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"scr-cms\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Subject</label>\r\n                                <div");

WriteLiteral(" class=\"col-lg-9\"");

WriteLiteral(">\r\n                                    <input");

WriteLiteral(" type=\"text\"");

WriteLiteral(" data-bind=\"value: CancelMessageSubject\"");

WriteLiteral("\r\n                                           required");

WriteLiteral("\r\n                                           placeholder=\"Invitation message subj" +
"ect\"");

WriteLiteral("\r\n                                           class=\"form-control razor-field\"");

WriteLiteral(" id=\"scr-cms\"");

WriteLiteral(">\r\n                                </div>\r\n                            </div>\r\n  " +
"                          <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                                <label");

WriteLiteral(" for=\"scr-cmb\"");

WriteLiteral(" class=\"col-lg-2 control-label\"");

WriteLiteral(">Body</label>\r\n                                <div");

WriteLiteral(" class=\"col-lg-9\"");

WriteLiteral(">\r\n                                    <textarea");

WriteLiteral(" data-bind=\"value: CancelMessageBody\"");

WriteLiteral("\r\n                                              required");

WriteLiteral("\r\n                                              placeholder=\"Invitation message b" +
"ody\"");

WriteLiteral("\r\n                                              class=\"form-control razor-field\"");

WriteLiteral(" id=\"scr-cmb\"");

WriteLiteral("></textarea>\r\n                                </div>\r\n                           " +
" </div>\r\n                        </form>\r\n                    </div>\r\n          " +
"      </div>\r\n             </div>\r\n            <div");

WriteLiteral(" class=\"modal-footer\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" form=\"activity-screen-modal-form\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" value=\"OK\"");

WriteLiteral(" data-bind=\"click: okClick\"");

WriteLiteral(" />\r\n                <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" data-dismiss=\"modal\"");

WriteLiteral(" data-bind=\"click : cancelClick\"");

WriteLiteral(">Cancel</a>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
