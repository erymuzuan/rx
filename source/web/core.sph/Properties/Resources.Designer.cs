﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Web.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bespoke.Sph.Web.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to &lt;h1&gt;Integration Adapters &lt;/h1&gt;
        ///&lt;div class=&quot;row&quot;&gt;
        ///
        ///    &lt;div&gt;
        ///        &lt;div class=&quot;btn-group&quot;&gt;
        ///            &lt;a class=&quot;btn btn-link dropdown-toggle&quot; data-toggle=&quot;drop-down&quot;&gt;
        ///                Add Adapter&amp;nbsp;&lt;span class=&quot;caret&quot;&gt;&lt;/span&gt;
        ///            &lt;/a&gt;
        ///            &lt;ul class=&quot;dropdown-menu&quot; data-bind=&quot;foreach :adapterOptions&quot;&gt;
        ///                &lt;li&gt;
        ///                    &lt;a style=&quot;text-align: left&quot; class=&quot;btn btn-link&quot; data-bind=&quot;attr : {&apos;href&apos;:&apos;#&apos; + designer.Route}&quot;&gt;
        ///                        &lt;i class=&quot;fa fa [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AdapterDefinitionListHtml {
            get {
                return ResourceManager.GetString("AdapterDefinitionListHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /// &lt;reference path=&quot;../../Scripts/jquery-2.1.1.intellisense.js&quot; /&gt;
        ////// &lt;reference path=&quot;../../Scripts/knockout-3.1.0.debug.js&quot; /&gt;
        ////// &lt;reference path=&quot;../../Scripts/knockout.mapping-latest.debug.js&quot; /&gt;
        ////// &lt;reference path=&quot;../../Scripts/require.js&quot; /&gt;
        ////// &lt;reference path=&quot;../../Scripts/underscore.js&quot; /&gt;
        ////// &lt;reference path=&quot;../../Scripts/moment.js&quot; /&gt;
        ////// &lt;reference path=&quot;../services/datacontext.js&quot; /&gt;
        ////// &lt;reference path=&quot;../schemas/sph.domain.g.js&quot; /&gt;
        ///
        ///
        ///define([&apos;services/datacontext&apos;, &apos;services [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AdapterDefinitionListJs {
            get {
                return ResourceManager.GetString("AdapterDefinitionListJs", resourceCulture);
            }
        }
    }
}
