﻿#pragma checksum "..\..\..\Views\PaymentRecordDialog.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "79BC4082F30DAB7A3E6C6D77F7BEE8FB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
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
    /// PaymentRecordDialog
    /// </summary>
    public partial class PaymentRecordDialog : Telerik.Windows.Controls.RadWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 39 "..\..\..\Views\PaymentRecordDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Views\PaymentRecordDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.VisualStateGroup PaymentStateGroup;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\Views\PaymentRecordDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.VisualState Invoice;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\Views\PaymentRecordDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.VisualState Payment;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\Views\PaymentRecordDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Telerik.Windows.Controls.RadGridView InvoicePaymentList;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\Views\PaymentRecordDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid InvoiceInfoPanel;
        
        #line default
        #line hidden
        
        
        #line 136 "..\..\..\Views\PaymentRecordDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid PaymentPanel;
        
        #line default
        #line hidden
        
        
        #line 174 "..\..\..\Views\PaymentRecordDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel DialogButtonsPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/commercialspace.module;component/views/paymentrecorddialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\PaymentRecordDialog.xaml"
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
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.PaymentStateGroup = ((System.Windows.VisualStateGroup)(target));
            return;
            case 3:
            this.Invoice = ((System.Windows.VisualState)(target));
            return;
            case 4:
            this.Payment = ((System.Windows.VisualState)(target));
            return;
            case 5:
            this.InvoicePaymentList = ((Telerik.Windows.Controls.RadGridView)(target));
            return;
            case 6:
            this.InvoiceInfoPanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.PaymentPanel = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.DialogButtonsPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 9:
            
            #line 175 "..\..\..\Views\PaymentRecordDialog.xaml"
            ((Telerik.Windows.Controls.RadButton)(target)).Click += new System.Windows.RoutedEventHandler(this.OkButtonClicked);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 176 "..\..\..\Views\PaymentRecordDialog.xaml"
            ((Telerik.Windows.Controls.RadButton)(target)).Click += new System.Windows.RoutedEventHandler(this.CancelButtonClicked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

