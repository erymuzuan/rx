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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bespoke.Sph.FormCompilers.DurandalJs.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to @using Bespoke.Sph.Domain
        ///@inherits Bespoke.Sph.FormCompilers.DurandalJs.Template&lt;Bespoke.Sph.FormCompilers.DurandalJs.FormRendererViewModel&gt;
        ///@{
        ///
        ///    Layout = null;
        ///    var form = (EntityForm)Model.Form;
        ///    var entity = (EntityDefinition)Model.Project;
        ///    var formId = @Model.Form.Route + &quot;-form&quot;;
        ///    var caption = string.IsNullOrWhiteSpace(form.Caption) ? Model.Form.Name : form.Caption;
        ///
        ///}&lt;h1&gt;@caption&lt;/h1&gt;
        ///&lt;div id=&quot;error-list&quot; class=&quot;row&quot; data-bind=&quot;visible:errors().length&quot;&gt;
        ///    &lt;!-- ko foreac [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Html2ColsWithAuditTrail {
            get {
                return ResourceManager.GetString("Html2ColsWithAuditTrail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to @using Bespoke.Sph.Domain
        ///@inherits Bespoke.Sph.FormCompilers.DurandalJs.Template&lt;Bespoke.Sph.FormCompilers.DurandalJs.FormRendererViewModel&gt;
        ///@{
        ///
        ///    Layout = null;
        ///    var form = (ScreenActivityForm)Model.Form;
        ///    var wd = (WorkflowDefinition)Model.Project;
        ///    var formId = @Model.Form.Route + &quot;-form&quot;;
        ///    var caption = form.Name;
        ///
        ///}&lt;h1&gt;@caption&lt;/h1&gt;
        ///&lt;div id=&quot;error-list&quot; class=&quot;row&quot; data-bind=&quot;visible:errors().length&quot;&gt;
        ///    &lt;!-- ko foreach : errors --&gt;
        ///    &lt;div class=&quot;col-sm-8 col-sm-offset-2  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ScreenActivityFormTemplate {
            get {
                return ResourceManager.GetString("ScreenActivityFormTemplate", resourceCulture);
            }
        }
    }
}