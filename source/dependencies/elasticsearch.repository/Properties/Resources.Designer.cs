﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.ElasticsearchRepository.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bespoke.Sph.ElasticsearchRepository.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to  &quot;cancelledmessage&quot;:{
        ///        &quot;properties&quot;:{
        ///            &quot;messageId&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analyzed&quot;},
        ///            &quot;worker&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analyzed&quot;}
        ///        }
        ///    }.
        /// </summary>
        internal static string CancelledMessageMapping {
            get {
                return ResourceManager.GetString("CancelledMessageMapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;event&quot;:{
        ///        &quot;properties&quot;:{
        ///            &quot;MessageId&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analyzed&quot;},
        ///            &quot;RoutingKey&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analyzed&quot;},
        ///            &quot;ItemId&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analyzed&quot;},
        ///            &quot;Entity&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analyzed&quot;},
        ///            &quot;MachineName&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analyzed&quot;},
        ///            &quot;ProcessName&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analyzed&quot;},
        ///            &quot;Event&quot;: {&quot;type&quot;: &quot;string&quot;, &quot;index&quot;:&quot;not_analy [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string MessageTrackingMapping {
            get {
                return ResourceManager.GetString("MessageTrackingMapping", resourceCulture);
            }
        }
    }
}
