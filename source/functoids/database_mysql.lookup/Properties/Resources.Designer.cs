﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Domain.Functoids.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bespoke.Sph.Domain.Functoids.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to 
        ///&lt;section class=&quot;view-model-modal&quot; id=&quot;config-functoid-dialog&quot;&gt;
        ///    &lt;div class=&quot;modal-dialog&quot;&gt;
        ///        &lt;div class=&quot;modal-content&quot;&gt;
        ///            &lt;div class=&quot;modal-header&quot;&gt;
        ///                &lt;button type=&quot;button&quot; class=&quot;close&quot; data-dismiss=&quot;modal&quot;
        ///                        data-bind=&quot;click : cancelClick&quot;&gt;
        ///                    &amp;times;
        ///                &lt;/button&gt;
        ///                &lt;h3&gt;Config Value Functoid&lt;/h3&gt;
        ///            &lt;/div&gt;
        ///            &lt;div class=&quot;modal-body&quot; data-bind=&quot;with:functoid&quot;&gt;
        ///
        ///                [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ConfigView {
            get {
                return ResourceManager.GetString("ConfigView", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to define([&quot;services/datacontext&quot;, &quot;services/logger&quot;, &quot;plugins/dialog&quot;],
        ///    function (context, logger, dialog) {
        ///        var functoid = ko.observable(),
        ///            okClick = function (data, ev) {
        ///                dialog.close(this, &quot;OK&quot;);
        ///
        ///            },
        ///            cancelClick = function () {
        ///                dialog.close(this, &quot;Cancel&quot;);
        ///            };
        ///        var vm = {
        ///            functoid: functoid,
        ///            okClick: okClick,
        ///            cancelClick: cancelClick
        ///        };
        ///        return [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ConfigViewModel {
            get {
                return ResourceManager.GetString("ConfigViewModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///&lt;section class=&quot;view-model-modal&quot; id=&quot;sql-server--lookup-functoid-dialog&quot;&gt;
        ///    &lt;div class=&quot;modal-dialog&quot;&gt;
        ///        &lt;div class=&quot;modal-content&quot;&gt;
        ///
        ///            &lt;div class=&quot;modal-header&quot;&gt;
        ///                &lt;button type=&quot;button&quot; class=&quot;close&quot; data-dismiss=&quot;modal&quot;
        ///                        data-bind=&quot;click : cancelClick&quot;&gt;
        ///                    &amp;times;
        ///                &lt;/button&gt;
        ///                &lt;h3&gt;SQL Lookup Functoid&lt;/h3&gt;
        ///            &lt;/div&gt;
        ///            &lt;div class=&quot;modal-body&quot; data-bind=&quot;with:functoid&quot;&gt;
        ///
        ///    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string tableview {
            get {
                return ResourceManager.GetString("tableview", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to define([&quot;services/datacontext&quot;, &quot;services/logger&quot;, &quot;plugins/dialog&quot;],
        ///    function (context, logger, dialog) {
        ///        var functoid = ko.observable(),
        ///            okClick = function (data, ev) {
        ///                dialog.close(this, &quot;OK&quot;);
        ///
        ///            },
        ///            cancelClick = function () {
        ///                dialog.close(this, &quot;Cancel&quot;);
        ///            };
        ///        var vm = {
        ///            functoid: functoid,
        ///            okClick: okClick,
        ///            cancelClick: cancelClick
        ///        };
        ///        return [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string tablevm {
            get {
                return ResourceManager.GetString("tablevm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///&lt;section class=&quot;view-model-modal&quot; id=&quot;sql-server--lookup-functoid-dialog&quot;&gt;
        ///    &lt;div class=&quot;modal-dialog&quot;&gt;
        ///        &lt;div class=&quot;modal-content&quot;&gt;
        ///
        ///            &lt;div class=&quot;modal-header&quot;&gt;
        ///                &lt;button type=&quot;button&quot; class=&quot;close&quot; data-dismiss=&quot;modal&quot;
        ///                        data-bind=&quot;click : cancelClick&quot;&gt;
        ///                    &amp;times;
        ///                &lt;/button&gt;
        ///                &lt;h3&gt;Key Value Extractor Functoid&lt;/h3&gt;
        ///            &lt;/div&gt;
        ///            &lt;div class=&quot;modal-body&quot; data-bind=&quot;with:functoid [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ValueExtractorView {
            get {
                return ResourceManager.GetString("ValueExtractorView", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to define([&quot;services/datacontext&quot;, &quot;services/logger&quot;, &quot;plugins/dialog&quot;],
        ///    function (context, logger, dialog) {
        ///        var functoid = ko.observable(),
        ///            okClick = function (data, ev) {
        ///                dialog.close(this, &quot;OK&quot;);
        ///
        ///            },
        ///            cancelClick = function () {
        ///                dialog.close(this, &quot;Cancel&quot;);
        ///            };
        ///        var vm = {
        ///            functoid: functoid,
        ///            okClick: okClick,
        ///            cancelClick: cancelClick
        ///        };
        ///        return [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ValueExtractorViewModel {
            get {
                return ResourceManager.GetString("ValueExtractorViewModel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///&lt;section class=&quot;view-model-modal&quot; id=&quot;sql-server--lookup-functoid-dialog&quot;&gt;
        ///    &lt;div class=&quot;modal-dialog&quot;&gt;
        ///        &lt;div class=&quot;modal-content&quot;&gt;
        ///
        ///            &lt;div class=&quot;modal-header&quot;&gt;
        ///                &lt;button type=&quot;button&quot; class=&quot;close&quot; data-dismiss=&quot;modal&quot;
        ///                        data-bind=&quot;click : cancelClick&quot;&gt;
        ///                    &amp;times;
        ///                &lt;/button&gt;
        ///                &lt;h3&gt;SQL Lookup Functoid&lt;/h3&gt;
        ///            &lt;/div&gt;
        ///            &lt;div class=&quot;modal-body&quot; data-bind=&quot;with:functoid&quot;&gt;
        ///
        ///    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string view {
            get {
                return ResourceManager.GetString("view", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to define([&quot;services/datacontext&quot;, &quot;services/logger&quot;, &quot;plugins/dialog&quot;],
        ///    function (context, logger, dialog) {
        ///        var functoid = ko.observable(),
        ///            okClick = function (data, ev) {
        ///                dialog.close(this, &quot;OK&quot;);
        ///
        ///            },
        ///            cancelClick = function () {
        ///                dialog.close(this, &quot;Cancel&quot;);
        ///            };
        ///        var vm = {
        ///            functoid: functoid,
        ///            okClick: okClick,
        ///            cancelClick: cancelClick
        ///        };
        ///        return [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string vm {
            get {
                return ResourceManager.GetString("vm", resourceCulture);
            }
        }
    }
}
