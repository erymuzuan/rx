﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.Integrations.Adapters.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bespoke.Sph.Integrations.Adapters.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to ///&lt;reference path=&quot;../../web/core.sph/Scripts/jstree.min.js&quot;/&gt;
        //////&lt;reference path=&quot;../../web/core.sph/Scripts/require.js&quot;/&gt;
        //////&lt;reference path=&quot;../../web/core.sph/Scripts/underscore.js&quot;/&gt;
        //////&lt;reference path=&quot;../../web/core.sph/SphApp/objectbuilders.js&quot;/&gt;
        //////&lt;reference path=&quot;../../web/core.sph/SphApp/schemas/form.designer.g.js&quot;/&gt;
        ///
        ///define([&quot;knockout&quot;], function (ko) {
        ///    ko.bindingHandlers.sprocRequestSchemaTree = {
        ///        init: function (element, valueAccessor) {
        ///            var system = require( [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string _ko_adapter_sqlserver {
            get {
                return ResourceManager.GetString("_ko_adapter_sqlserver", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There is already file {0} with the {1} content.
        /// </summary>
        internal static string DuplicateContentSource {
            get {
                return ResourceManager.GetString("DuplicateContentSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0,-15}	.
        /// </summary>
        internal static string Format15Tab {
            get {
                return ResourceManager.GetString("Format15Tab", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///	
        ///SELECT 
        ///        c.column_id as &apos;Id&apos;
        ///        ,o.name as &apos;Table&apos;
        ///		    ,s.name as &apos;Schema&apos;
        ///        ,c.name as &apos;Column&apos;
        ///        ,t.name as &apos;Type&apos; 		
        ///        ,c.max_length as &apos;Length&apos;
        ///        ,c.is_nullable as &apos;IsNullable&apos;    
        ///	    ,c.is_identity as &apos;IsIdentity&apos;
        ///        ,c.is_computed as &apos;IsComputed&apos;
        ///    FROM 
        ///        sys.objects o INNER JOIN sys.all_columns c
        ///        ON c.object_id = o.object_id
        ///        INNER JOIN sys.types t 
        ///        ON c.system_type_id = t.system_type_id
        ///        INNER JOI [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SelectColumnsSql {
            get {
                return ResourceManager.GetString("SelectColumnsSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT  
        ///    B.COLUMN_NAME
        ///FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS A, 
        ///    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B
        ///WHERE CONSTRAINT_TYPE = &apos;PRIMARY KEY&apos; AND A.CONSTRAINT_NAME = B.CONSTRAINT_NAME
        ///    AND A.TABLE_NAME = @Table
        ///    AND A.CONSTRAINT_SCHEMA = @Schema.
        /// </summary>
        internal static string SelectTablePrimaryKeysSql {
            get {
                return ResourceManager.GetString("SelectTablePrimaryKeysSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT o.name, s.name as &apos;schema&apos; FROM sys.all_objects o INNER JOIN sys.schemas s
        ///ON o.schema_id = s.schema_id
        ///WHERE o.[type] = &apos;U&apos;
        ///AND s.name NOT IN (&apos;sys&apos;, &apos;INFORMATION_SCHEMA&apos;)
        ///ORDER BY o.[schema_id].
        /// </summary>
        internal static string SelectTablesSql {
            get {
                return ResourceManager.GetString("SelectTablesSql", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;link href=&quot;/Content/external/prism.css&quot; rel=&quot;stylesheet&quot; /&gt;
        ///
        ///&lt;h1&gt;Stored Procedure Operation Details&lt;/h1&gt;
        ///&lt;div class=&quot;row&quot;&gt;
        ///    &lt;div class=&quot;col-sm-12 col-md-8 col-lg-9&quot;&gt;
        ///
        ///        &lt;div class=&quot;row&quot; id=&quot;members-panel&quot;&gt;
        ///            &lt;h3&gt;Request Schema Designer&lt;/h3&gt;
        ///            &lt;div class=&quot;col-sm-8&quot;&gt;
        ///                &lt;div data-bind=&quot;sprocRequestSchemaTree : {entity: requestSchema, selected: member, searchbox: &apos;#search-box-tree&apos; }&quot;&gt;
        ///                &lt;/div&gt;
        ///            &lt;/div&gt;
        ///
        ///            &lt;div class=&quot;col [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SprocHtml {
            get {
                return ResourceManager.GetString("SprocHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /// &lt;reference path=&quot;../Scripts/jquery-2.2.0.intellisense.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/knockout-3.4.0.debug.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/knockout.mapping-latest.debug.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/require.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/underscore.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/respond.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/moment.js&quot; /&gt;
        ////// &lt;reference path=&quot;../services/datacontext.js&quot; /&gt;
        ////// &lt;reference path=&quot;../schemas/sph.domain.g.js&quot; /&gt;
        ////// &lt;reference  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SprocJs {
            get {
                return ResourceManager.GetString("SprocJs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;h1&gt;Microsoft SQL Server&lt;/h1&gt;
        ///&lt;div id=&quot;error-list&quot; class=&quot;row&quot; data-bind=&quot;visible:errors().length&quot;&gt;
        ///    &lt;!-- ko foreach : errors --&gt;
        ///    &lt;div class=&quot;col-md-8 col-md-offset-2 alert alert-dismissable alert-danger&quot;&gt;
        ///        &lt;button type=&quot;button&quot; class=&quot;close&quot; data-dismiss=&quot;alert&quot; aria-hidden=&quot;true&quot;&gt;&amp;times;&lt;/button&gt;
        ///        &lt;i class=&quot;fa fa-exclamation&quot;&gt;&lt;/i&gt;
        ///        &lt;span data-bind=&quot;text:Message&quot;&gt;&lt;/span&gt;
        ///
        ///    &lt;/div&gt;
        ///    &lt;div class=&quot;col-md-2&quot;&gt;&lt;/div&gt;
        ///    &lt;!-- /ko--&gt;
        ///&lt;/div&gt;
        ///
        ///&lt;div class=&quot;row&quot;&gt;
        ///    &lt;div [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SqlServerAdapterHtml {
            get {
                return ResourceManager.GetString("SqlServerAdapterHtml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /// &lt;reference path=&quot;../Scripts/jquery-2.2.0.intellisense.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/knockout-3.4.0.debug.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/knockout.mapping-latest.debug.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/require.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/underscore.js&quot; /&gt;
        ////// &lt;reference path=&quot;../Scripts/moment.js&quot; /&gt;
        ////// &lt;reference path=&quot;../services/datacontext.js&quot; /&gt;
        ////// &lt;reference path=&quot;../schemas/sph.domain.g.js&quot; /&gt;
        ////// &lt;reference path=&quot;../../../web/core.sph/sphapp/schemas/adapt [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SqlServerAdapterJs {
            get {
                return ResourceManager.GetString("SqlServerAdapterJs", resourceCulture);
            }
        }
    }
}
