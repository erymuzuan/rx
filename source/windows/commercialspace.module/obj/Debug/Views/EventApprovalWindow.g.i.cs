﻿#pragma checksum "..\..\..\Views\EventApprovalWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D5172DFD6C96B407E3889331464C33FC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Animation;
using Telerik.Windows.Controls.Carousel;
using Telerik.Windows.Controls.Data.PropertyGrid;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Legend;
using Telerik.Windows.Controls.Primitives;
using Telerik.Windows.Controls.TransitionEffects;
using Telerik.Windows.Controls.TreeListView;
using Telerik.Windows.Controls.TreeView;
using Telerik.Windows.Data;
using Telerik.Windows.DragDrop;
using Telerik.Windows.DragDrop.Behaviors;
using Telerik.Windows.Input.Touch;
using Telerik.Windows.Shapes;


namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views {
    
    
    /// <summary>
    /// EventApprovalWindow
    /// </summary>
    public partial class EventApprovalWindow : Telerik.Windows.Controls.RadWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 70 "..\..\..\Views\EventApprovalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadButton CancelButton;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\Views\EventApprovalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadButton DisapprovedButton;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\Views\EventApprovalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadButton ApprovedButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/commercialspace.module;component/views/eventapprovalwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\EventApprovalWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.CancelButton = ((Telerik.Windows.Controls.RadButton)(target));
            
            #line 70 "..\..\..\Views\EventApprovalWindow.xaml"
            this.CancelButton.Click += new System.Windows.RoutedEventHandler(this.CancelButtonClick);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DisapprovedButton = ((Telerik.Windows.Controls.RadButton)(target));
            
            #line 71 "..\..\..\Views\EventApprovalWindow.xaml"
            this.DisapprovedButton.Click += new System.Windows.RoutedEventHandler(this.NoButtonClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.ApprovedButton = ((Telerik.Windows.Controls.RadButton)(target));
            
            #line 72 "..\..\..\Views\EventApprovalWindow.xaml"
            this.ApprovedButton.Click += new System.Windows.RoutedEventHandler(this.OkButtonClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

