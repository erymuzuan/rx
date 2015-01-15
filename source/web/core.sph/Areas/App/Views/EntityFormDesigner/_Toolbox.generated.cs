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

namespace Bespoke.Sph.Web.Areas.App.Views.EntityFormDesigner
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
    
    #line 1 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
    using Bespoke.Sph.Domain;
    
    #line default
    #line hidden
    using Bespoke.Sph.Web;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Areas/App/Views/EntityFormDesigner/_Toolbox.cshtml")]
    public partial class Toolbox : System.Web.Mvc.WebViewPage<Bespoke.Sph.Web.ViewModels.TemplateFormViewModel>
    {
        public Toolbox()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" id=\"form-designer-toolbox\"");

WriteLiteral(" data-path=\"Areas\\App\\Views\\EntityFormDesigner\\_Toolbox.cshtml\"");

WriteLiteral(">\r\n    <ul");

WriteLiteral(" class=\"nav nav-tabs\"");

WriteLiteral(" data-bind2=\"filter : {path:\'>li\'}\"");

WriteLiteral(">\r\n        <li");

WriteLiteral(" class=\"active\"");

WriteLiteral(">\r\n            <a");

WriteLiteral(" href=\"#form-general2\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">General</a>\r\n        </li>\r\n        <li>\r\n            <a");

WriteLiteral(" href=\"#add-field\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Add a field</a>\r\n        </li>\r\n        <li>\r\n            <a");

WriteLiteral(" href=\"#fields-settings\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Properties</a>\r\n        </li>\r\n        <li>\r\n            <a");

WriteLiteral(" href=\"#fields-validation\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Validations</a>\r\n        </li>\r\n\r\n        <li>\r\n            <a");

WriteLiteral(" href=\"#business-rules\"");

WriteLiteral(" data-toggle=\"tab\"");

WriteLiteral(">Business Rules</a>\r\n        </li>\r\n\r\n    </ul>\r\n    <div");

WriteLiteral(" class=\"tab-content\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" id=\"form-general2\"");

WriteLiteral(" class=\"tab-pane active\"");

WriteLiteral(" data-bind=\"with : form\"");

WriteLiteral(">\r\n            <form");

WriteLiteral(" role=\"form\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label>Name</label>\r\n                    <input required");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Name\"");

WriteLiteral(" id=\"form-design-name\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"FormDesign.Name\"");

WriteLiteral(" />\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label>Route</label>\r\n                    <input required");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Route\"");

WriteLiteral(" id=\"form-design-Route\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"FormDesign.Route\"");

WriteLiteral(" />\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" for=\"form-design-caption\"");

WriteLiteral(">Caption</label>\r\n                    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Caption\"");

WriteLiteral(" id=\"form-design-caption\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"FormDesign.Caption\"");

WriteLiteral(" />\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" for=\"form-design-layout\"");

WriteLiteral(">Layout</label>\r\n                    <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"options :$root.layoutOptions,\r\n                            optionsCap" +
"tion :\'[Default Layout]\',\r\n                            value: Layout\"");

WriteLiteral(" id=\"form-design-layout\"");

WriteLiteral(" name=\"FormDesign.Layout\"");

WriteLiteral("></select>\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" for=\"form-design-operation\"");

WriteLiteral(">Operation</label>\r\n                    <select");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"options :$root.operationsOption,\r\n                            options" +
"Caption :\'[Default - No Operation]\',\r\n                            value: Operati" +
"on\"");

WriteLiteral(" id=\"form-design-operation\"");

WriteLiteral(" name=\"FormDesign.Operation\"");

WriteLiteral("></select>\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" for=\"form-design-icon-class\"");

WriteLiteral(">Icon class</label>\r\n                    <br />\r\n                    <i");

WriteLiteral(" data-bind=\"iconPicker: IconClass, attr:{\'class\':IconClass() + \' fa-2x\' }\"");

WriteLiteral(" id=\"form-design-icon-class\"");

WriteLiteral("></i>\r\n\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label>Note</label>\r\n                    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Note\"");

WriteLiteral(" id=\"form-design-Description\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"FormDesign.Description\"");

WriteLiteral(" />\r\n                </div>\r\n          \r\n                <div");

WriteLiteral(" class=\"form-group checkbox checkbox-no-padding-left\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsDefault\"");

WriteLiteral(" id=\"form-design-isdefault\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"FormDesign.IsDefault\"");

WriteLiteral(" />\r\n                        Is default form\r\n                    </label>\r\n     " +
"           </div>\r\n                <div");

WriteLiteral(" class=\"form-group checkbox checkbox-no-padding-left\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsAllowedNewItem\"");

WriteLiteral(" id=\"form-design-is-allow-new\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"FormDesign.IsAllowedNewItem\"");

WriteLiteral(" />\r\n                        Allow new item\r\n                    </label>\r\n      " +
"          </div>\r\n                <div");

WriteLiteral(" class=\"form-group checkbox checkbox-no-padding-left\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsRemoveAvailable\"");

WriteLiteral(" id=\"form-design-is-remove-item\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"FormDesign.IsRemoveAvailable\"");

WriteLiteral(" />\r\n                        Allow remove item\r\n                    </label>\r\n   " +
"             </div>\r\n                <div");

WriteLiteral(" class=\"form-group checkbox checkbox-no-padding-left\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsEmailAvailable\"");

WriteLiteral(" id=\"form-design-isemail\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"FormDesign.IsEmailAvailable\"");

WriteLiteral(" />\r\n                        Allow Email\r\n                    </label>\r\n         " +
"       </div>\r\n\r\n                <div");

WriteLiteral(" class=\"form-group checkbox checkbox-no-padding-left\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsPrintAvailable\"");

WriteLiteral(" id=\"form-design-isprint\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"FormDesign.IsPrintAvailable\"");

WriteLiteral(" />\r\n                        Allow Print\r\n                    </label>\r\n         " +
"       </div>\r\n                <div");

WriteLiteral(" class=\"form-group checkbox checkbox-no-padding-left\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsWatchAvailable\"");

WriteLiteral(" id=\"form-design-iswatch\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"FormDesign.IsWatchAvailable\"");

WriteLiteral(" />\r\n                        Allow Watch\r\n                    </label>\r\n         " +
"       </div>\r\n                <div");

WriteLiteral(" class=\"form-group checkbox checkbox-no-padding-left\"");

WriteLiteral(">\r\n                    <label>\r\n                        <input");

WriteLiteral(" data-bind=\"checked: IsAuditTrailAvailable\"");

WriteLiteral(" id=\"form-design-IsAuditTrailAvailable\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"FormDesign.IsAuditTrailAvailable\"");

WriteLiteral(" />\r\n                        Audit trail\r\n                    </label>\r\n         " +
"       </div>\r\n\r\n                <!-- ko with : FormDesign -->\r\n\r\n\r\n            " +
"    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" href=\"#label-cols\"");

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-chevron-down\"");

WriteLiteral("></i>Label columns span</a>\r\n                </div>\r\n                <div");

WriteLiteral(" id=\"label-cols\"");

WriteLiteral(" class=\"form-group collapse\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label>Label Col Large</label>\r\n                      " +
"  <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: LabelColLg\"");

WriteLiteral(" id=\"form-design-LabelColLg\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" name=\"FormDesign.LabelColLg\"");

WriteLiteral(" />\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label>Label Col Medium</label>\r\n                     " +
"   <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: LabelColMd\"");

WriteLiteral(" id=\"form-design-LabelColMd\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" name=\"FormDesign.LabelColMd\"");

WriteLiteral(" />\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label>Label Col Small</label>\r\n                      " +
"  <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: LabelColSm\"");

WriteLiteral(" id=\"form-design-LabelColSm\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" name=\"FormDesign.LabelColSm\"");

WriteLiteral(" />\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label>Label Col x-small</label>\r\n                    " +
"    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: LabelColXs\"");

WriteLiteral(" id=\"form-design-LabelColXs\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" name=\"FormDesign.LabelColXs\"");

WriteLiteral(" />\r\n                    </div>\r\n                </div>\r\n\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" href=\"#input-cols\"");

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-chevron-down\"");

WriteLiteral("></i>Input columns span</a>\r\n                </div>\r\n                <div");

WriteLiteral(" id=\"input-cols\"");

WriteLiteral(" class=\"collapse form-group\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label>Input Col Large</label>\r\n                      " +
"  <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: InputColLg\"");

WriteLiteral(" id=\"form-design-InputColLg\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" name=\"FormDesign.InputColLg\"");

WriteLiteral(" />\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label>Input Col Medium</label>\r\n                     " +
"   <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: InputColMd\"");

WriteLiteral(" id=\"form-design-InputColMd\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" name=\"FormDesign.InputColMd\"");

WriteLiteral(" />\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label>Input Col Small</label>\r\n                      " +
"  <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: InputColSm\"");

WriteLiteral(" id=\"form-design-InputColSm\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" name=\"FormDesign.InputColSm\"");

WriteLiteral(" />\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label>Input Col x-small</label>\r\n                    " +
"    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: InputColXs\"");

WriteLiteral(" id=\"form-design-InputColXs\"");

WriteLiteral(" type=\"number\"");

WriteLiteral(" name=\"FormDesign.InputColXs\"");

WriteLiteral(" />\r\n                    </div>\r\n\r\n                </div>\r\n\r\n\r\n                <!" +
"-- /ko -->\r\n\r\n            </form>\r\n        </div>\r\n        <div");

WriteLiteral(" id=\"add-field\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(">\r\n            <ul");

WriteLiteral(" class=\"nav\"");

WriteLiteral(" data-bind=\"foreach: formElements\"");

WriteLiteral(">\r\n                <li>\r\n                    <a");

WriteLiteral(" class=\"btn btn-default\"");

WriteLiteral(" href=\"#add-field\"");

WriteLiteral(" data-bind=\"tooltip : designer.Description\"");

WriteLiteral(">\r\n                        <i");

WriteLiteral(" data-bind=\"attr: { \'class\':\'fa fa-\' + designer.FontAwesomeIcon }\"");

WriteLiteral(" class=\"pull-left\"");

WriteLiteral("></i>\r\n                        <!-- ko text: designer.Name -->\r\n                 " +
"       <!-- /ko-->\r\n                    </a>\r\n                </li>\r\n           " +
" </ul>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" id=\"fields-settings\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(" data-bind=\"with: selectedFormElement\"");

WriteLiteral(">\r\n            <form");

WriteLiteral(" role=\"form\"");

WriteLiteral(">\r\n\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-bind=\"visible:IsPathIsRequired\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" for=\"form-element-path\"");

WriteLiteral(">Path</label>\r\n                    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" pattern=\"^[A-Za-z_][A-Za-z0-9_.]*$\"");

WriteLiteral(" data-bind=\"value: Path, entityTypeaheadPath : $root.entity().Id()\"");

WriteLiteral(" id=\"form-element-path\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Path\"");

WriteLiteral(" />\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" for=\"form-element-label\"");

WriteLiteral(">Label</label>\r\n                    <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Label, valueUpdate: \'keyup\'\"");

WriteLiteral(" id=\"form-element-label\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Label\"");

WriteLiteral(" />\r\n                </div>\r\n                <br />\r\n                <div");

WriteLiteral(" class=\"form-group checkbox checkbox-no-padding-left\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" for=\"use-display-template\"");

WriteLiteral(" class=\"sr-only\"");

WriteLiteral(">Use display template</label>\r\n                    <label");

WriteLiteral(" for=\"use-display-template\"");

WriteLiteral(">\r\n                        <input");

WriteLiteral(" data-bind=\"checked: UseDisplayTemplate\"");

WriteLiteral(" id=\"use-display-template\"");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" name=\"UseDisplayTemplate\"");

WriteLiteral(" />\r\n                        Use display template\r\n                    </label>\r\n" +
"                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" href=\"#toolbox-more-settings\"");

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-chevron-down\"");

WriteLiteral("></i>More settings</a>\r\n                </div>\r\n                <div");

WriteLiteral(" id=\"toolbox-more-settings\"");

WriteLiteral(" class=\"collapse\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label");

WriteLiteral(" for=\"form-element-tooltip\"");

WriteLiteral(">Tooltip</label>\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Tooltip\"");

WriteLiteral(" id=\"form-element-tooltip\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Tooltip\"");

WriteLiteral(" />\r\n                    </div>\r\n\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label");

WriteLiteral(" for=\"form-element-help-text\"");

WriteLiteral(">Help</label>\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: HelpText, valueUpdate: \'keyup\'\"");

WriteLiteral(" id=\"form-element-help-text\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"HelpText\"");

WriteLiteral(" />\r\n                    </div>\r\n\r\n\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label");

WriteLiteral(" for=\"fe-css-class\"");

WriteLiteral(">Css class</label>\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: CssClass\"");

WriteLiteral(" id=\"fe-css-class\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"CssClass\"");

WriteLiteral(" />\r\n                    </div>\r\n\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label");

WriteLiteral(" for=\"form-element-visible\"");

WriteLiteral(">Visible</label>\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Visible\"");

WriteLiteral(" id=\"form-element-visible\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Visible\"");

WriteLiteral(" />\r\n                    </div>\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label");

WriteLiteral(" for=\"form-element-enable\"");

WriteLiteral(">Enable</label>\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: Enable\"");

WriteLiteral(" id=\"form-element-enable\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"Enable\"");

WriteLiteral(" />\r\n                    </div>\r\n\r\n\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                        <label");

WriteLiteral(" for=\"form-element-elementId\"");

WriteLiteral(">Id</label>\r\n                        <input");

WriteLiteral(" class=\"form-control\"");

WriteLiteral(" data-bind=\"value: ElementId\"");

WriteLiteral(" id=\"form-element-elementId\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"ElementId\"");

WriteLiteral(" />\r\n                    </div>\r\n\r\n                </div>\r\n\r\n                <div" +
"");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" href=\"#toolbox-advanced-settings\"");

WriteLiteral(" data-toggle=\"collapse\"");

WriteLiteral(">\r\n                        <i");

WriteLiteral(" class=\"fa fa-chevron-down\"");

WriteLiteral("></i>\r\n                        Advanced settings\r\n                    </a>\r\n     " +
"           </div>\r\n                <div");

WriteLiteral(" id=\"toolbox-advanced-settings\"");

WriteLiteral(" class=\"collapse\"");

WriteLiteral(">\r\n");

            
            #line 235 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 235 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
                      
                        var types = new[]
                        {
                            typeof(TextBox),
                            typeof(ComboBox),
                            typeof(Button),
                            typeof(DownloadLink),
                            typeof(ImageElement),
                            typeof(ChildEntityListView),
                            typeof(EntityLookupElement),
                            typeof(FileUploadElement),
                            typeof(ListView)
                        };
                    
            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 249 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 249 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
                     foreach (var fe in Model.FormElements.Where(t => types.Contains(t.GetType())))
                    {
                        FormElement fe1 = fe;
                        
            
            #line default
            #line hidden
            
            #line 252 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
                   Write(Html.EditorFor(m => fe1));

            
            #line default
            #line hidden
            
            #line 252 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
                                                 
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n\r\n                <a");

WriteLiteral(" data-bind=\"click: $root.removeFormElement\"");

WriteLiteral(" class=\"btn btn-warning\"");

WriteLiteral(" title=\"remove this element\"");

WriteLiteral(">Remove</a>\r\n                <!-- END OF EDITOR TOOLBOX -->\r\n            </form>\r" +
"\n\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" id=\"fields-validation\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(" data-bind=\"with : selectedFormElement\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 263 "..\..\Areas\App\Views\EntityFormDesigner\_Toolbox.cshtml"
       Write(Html.Partial("_ValidationSetting"));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" id=\"business-rules\"");

WriteLiteral(" class=\"tab-pane\"");

WriteLiteral(">\r\n            <span>Apply these business rules to the form</span>\r\n            <" +
"ul");

WriteLiteral(" data-bind=\"foreach :entity().BusinessRuleCollection\"");

WriteLiteral(" class=\"nav\"");

WriteLiteral(">\r\n                <li>\r\n                    <label>\r\n                        <in" +
"put");

WriteLiteral(" type=\"checkbox\"");

WriteLiteral(" data-bind=\"checked : $root.form().Rules, value: Name\"");

WriteLiteral(" />\r\n                        <!-- ko text : Name -->\r\n                        <!-" +
"- /ko -->\r\n\r\n                    </label>\r\n                </li>\r\n            </" +
"ul>\r\n        </div>\r\n\r\n    </div>\r\n</div>\r\n");

        }
    }
}
#pragma warning restore 1591
