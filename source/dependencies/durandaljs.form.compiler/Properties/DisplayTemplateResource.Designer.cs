﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.FormCompilers.DurandalJs.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class DisplayTemplateResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DisplayTemplateResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bespoke.Sph.FormCompilers.DurandalJs.Properties.DisplayTemplateResource", typeof(DisplayTemplateResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.Button
        ///.
        /// </summary>
        internal static string display_template_Button {
            get {
                return ResourceManager.GetString("display_template_Button", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.CheckBox
        ///@{
        ///    
        ///}
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label class=&quot;control-label&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;span data-bind=&quot;text: @Model.Path&quot; id=&quot;ID&quot;  /&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;
        ///
        ///.
        /// </summary>
        internal static string display_template_CheckBox {
            get {
                return ResourceManager.GetString("display_template_CheckBox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @using System.Web.Mvc.Html
        ///@using Bespoke.Sph.Domain
        ///@model Bespoke.Sph.Domain.ChildEntityListView
        ///
        ///@if (string.IsNullOrWhiteSpace(Model.Enable))
        ///{
        ///    Model.Enable = &quot;true&quot;;
        ///}
        ///
        ///&lt;div data-bind=&quot;visible:@Model.Visible&quot;&gt;
        ///    &lt;a class=&quot;btn btn-default pull-right&quot;&gt;@Model.Label&lt;/a&gt;
        ///    &lt;table class=&quot;table table-condensed table-striped&quot;&gt;
        ///        &lt;thead&gt;
        ///            &lt;tr&gt;
        ///                @foreach (var col in Model.ViewColumnCollection)
        ///                {
        ///                    &lt;th&gt;@col.Header&lt;/th&gt;
        ///     [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string display_template_ChildEntityListView {
            get {
                return ResourceManager.GetString("display_template_ChildEntityListView", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.ComboBox
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label class=&quot;control-label&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;span data-bind=&quot;@Model.GetKnockoutDisplayBindingExpression()&quot;&gt;&lt;/span&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;
        ///.
        /// </summary>
        internal static string display_template_ComboBox {
            get {
                return ResourceManager.GetString("display_template_ComboBox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.CurrencyElement
        ///&lt;div class=&quot;form-group&quot;&gt;
        ///
        ///    &lt;label class=&quot;col-md-4&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;span class=&quot;col-md-8&quot; data-bind=&quot;money: @Model.Path&quot;&gt;&lt;/span&gt;
        ///
        ///&lt;/div&gt;.
        /// </summary>
        internal static string display_template_CurrencyElement {
            get {
                return ResourceManager.GetString("display_template_CurrencyElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.DatePicker
        ///
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label for=&quot;@Model.ElementId&quot; class=&quot;control-label&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;input class=&quot;@(Model.CssClass + &quot; &quot;+ Model.Size)&quot; 
        ///            data-bind=&quot;@(Html.Raw(Model.GetKnockoutBindingExpression()))&quot; id=&quot;@Model.ElementId&quot; type=&quot;text&quot; 
        ///            name=&quot;@Model.Path&quot; /&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;.
        /// </summary>
        internal static string display_template_DatePicker {
            get {
                return ResourceManager.GetString("display_template_DatePicker", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.DatePicker
        ///
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label for=&quot;@Model.ElementId&quot; class=&quot;control-label&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;input class=&quot;@(Model.CssClass + &quot; &quot;+ Model.Size)&quot; 
        ///            data-bind=&quot;@(Html.Raw(Model.GetKnockoutBindingExpression()))&quot; id=&quot;@Model.ElementId&quot; type=&quot;text&quot; 
        ///            name=&quot;@Model.Path&quot; /&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;.
        /// </summary>
        internal static string display_template_DatePickerTime {
            get {
                return ResourceManager.GetString("display_template_DatePickerTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.DownloadLink
        ///@if (string.IsNullOrWhiteSpace(Model.Enable))
        ///{
        ///    Model.Enable = &quot;true&quot;;
        ///}
        ///@{
        ///    var path = string.Format(&quot;&apos;/sph/binarystore/get/&apos; + {0}()&quot;, Model.Path);
        ///    if (Model.IsTransformTemplate)
        ///    {
        ///        path = string.Format(&quot;&apos;/sph/documenttemplate/transform?entity={0}&amp;templateId={1}&amp;id=&apos; + {2}()&quot;, Model.Entity, Model.TemplateId, Model.Path);
        ///    }
        ///}
        ///
        ///@if (Model.IsCompact)
        ///{
        ///    &lt;a data-bind=&quot;attr : {&apos;href&apos;:@Html.Raw(path)}, visible:@Model.Visible&quot; dow [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string display_template_DownloadLink {
            get {
                return ResourceManager.GetString("display_template_DownloadLink", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.EmailFormElement
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label for=&quot;@Model.ElementId&quot; class=&quot;control-label&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;input class=&quot;@(Model.CssClass + &quot; &quot;+ Model.Size)&quot; title=&quot;@Model.Tooltip&quot; 
        ///            data-bind=&quot;@(Html.Raw(Model.GetKnockoutBindingExpression()))&quot; id=&quot;@Model.ElementId&quot; type=&quot;email&quot; name=&quot;@Model.Path&quot; /&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;
        ///
        ///.
        /// </summary>
        internal static string display_template_EmailFormElement {
            get {
                return ResourceManager.GetString("display_template_EmailFormElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.FileUploadElement
        ///.
        /// </summary>
        internal static string display_template_FileUploadElement {
            get {
                return ResourceManager.GetString("display_template_FileUploadElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.FormElement
        ///
        ///&lt;div class=&quot;alert alert-error&quot;&gt;
        ///    &lt;strong&gt;There&apos;s no EditorTemplate for @Model.GetType().Name&lt;/strong&gt;
        ///&lt;/div&gt;
        ///.
        /// </summary>
        internal static string display_template_FormElement {
            get {
                return ResourceManager.GetString("display_template_FormElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.HtmlElement
        ///@Html.Raw(Model.Text)
        ///
        ///.
        /// </summary>
        internal static string display_template_HtmlElement {
            get {
                return ResourceManager.GetString("display_template_HtmlElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.ImageElement
        ///@if (string.IsNullOrWhiteSpace(Model.Enable))
        ///{
        ///    Model.Enable = &quot;true&quot;;
        ///}@{
        ///    var path = string.Format(&quot;&apos;/sph/image/store/&apos; + {0}()&quot;, this.Model.Path);
        /// }
        ///&lt;div class=&quot;form-group&quot;&gt;
        ///    &lt;label class=&quot;col-md-@(Model.LabelColMd ?? 4)&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;img alt=&quot;@Model.Label&quot; title=&quot;@Model.Tooltip&quot; width=&quot;@Model.Width&quot; height=&quot;@Model.Height&quot; data-bind=&quot;attr : {&apos;src&apos;:@Html.Raw(path)}, visible:@Html.Raw(Model.Visible)&quot; /&gt;
        ///&lt;/div&gt;.
        /// </summary>
        internal static string display_template_ImageElement {
            get {
                return ResourceManager.GetString("display_template_ImageElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @using System.Web.Mvc.Html
        ///@model Bespoke.Sph.Domain.ListView
        ///
        ///
        ///&lt;div data-bind=&quot;visible:@Model.Visible&quot;&gt;
        ///    &lt;span&gt; @Model.Label&lt;/span&gt;
        ///    &lt;table class=&quot;table table-condensed table-striped&quot;&gt;
        ///        &lt;thead&gt;
        ///            &lt;tr&gt;
        ///                @foreach (var col in Model.ListViewColumnCollection)
        ///                {
        ///                    &lt;th&gt;@col.Label&lt;/th&gt;
        ///                }
        ///            &lt;/tr&gt;
        ///        &lt;/thead&gt;
        ///        &lt;tbody data-bind=&quot;foreach :@Model.Path&quot;&gt;
        ///            &lt;tr&gt;
        ///                @foreach [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string display_template_ListView {
            get {
                return ResourceManager.GetString("display_template_ListView", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.MapElement
        ///           
        ///&lt;!-- ko if : staticMap --&gt;
        ///&lt;img data-bind=&quot;attr : {src:staticMap}&quot; src=&quot;/Images/no-image.png&quot; alt=&quot;map&quot; /&gt;
        ///&lt;!-- /ko  --&gt;
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label for=&quot;@Model.ElementId&quot; class=&quot;control-label&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;button title=&quot;@Model.Tooltip&quot; id=&quot;@Model.ElementId&quot; class=&quot;@Model.CssClass&quot; data-bind=&quot;click: $root.showMapCommand, visible:@Model.Visible&quot;&gt;
        ///            &lt;i class=&quot;@Model.Icon&quot;&gt;&lt;/i&gt;
        ///           [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string display_template_MapElement {
            get {
                return ResourceManager.GetString("display_template_MapElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.NumberTextBox
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label for=&quot;@Model.ElementId&quot; class=&quot;control-label&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;span  class=&quot;@(Model.CssClass + &quot; &quot;+ Model.Size)&quot; 
        ///            title=&quot;@Model.Tooltip&quot; data-bind=&quot;text: @Model.Path&quot;
        ///             id=&quot;@Model.ElementId&quot; /&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;  .
        /// </summary>
        internal static string display_template_NumberTextBox {
            get {
                return ResourceManager.GetString("display_template_NumberTextBox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.SectionFormElement
        ///&lt;h2 data-bind=&quot;@(Html.Raw(Model.GetKnockoutBindingExpression()))&quot;&gt;@Model.Label&lt;/h2&gt;
        ///
        ///.
        /// </summary>
        internal static string display_template_SectionFormElement {
            get {
                return ResourceManager.GetString("display_template_SectionFormElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.TextAreaElement
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label class=&quot;control-label &quot;&gt;@Model.Label &lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;span data-bind=&quot;@Model.GetKnockoutDisplayBindingExpression()&quot;/&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;.
        /// </summary>
        internal static string display_template_TextAreaElement {
            get {
                return ResourceManager.GetString("display_template_TextAreaElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.TextBox
        ///&lt;div class=&quot;form-group&quot;&gt;
        ///    &lt;label class=&quot;control-label&quot;&gt;@Model.Label &lt;/label&gt;
        ///    &lt;div&gt;
        ///        &lt;span data-bind=&quot;@Model.GetKnockoutDisplayBindingExpression()&quot; &gt;&lt;/span&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;
        ///.
        /// </summary>
        internal static string display_template_TextBox {
            get {
                return ResourceManager.GetString("display_template_TextBox", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @model Bespoke.Sph.Domain.WebsiteFormElement
        ///&lt;div class=&quot;control-group&quot;&gt;
        ///    &lt;label for=&quot;@Model.ElementId&quot; class=&quot;control-label&quot;&gt;@Model.Label&lt;/label&gt;
        ///    &lt;div class=&quot;controls&quot;&gt;
        ///        &lt;input class=&quot;@(Model.CssClass + &quot; &quot;+ Model.Size)&quot; 
        ///               title=&quot;@Model.Tooltip&quot; 
        ///            data-bind=&quot;@(Html.Raw(Model.GetKnockoutBindingExpression()))&quot; id=&quot;@Model.ElementId&quot; type=&quot;url&quot; name=&quot;@Model.Path&quot; /&gt;
        ///    &lt;/div&gt;
        ///&lt;/div&gt;
        ///
        ///
        ///.
        /// </summary>
        internal static string display_template_WebsiteFormElement {
            get {
                return ResourceManager.GetString("display_template_WebsiteFormElement", resourceCulture);
            }
        }
    }
}
