using System;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;
using Telerik.Windows.Controls.GridView;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class AddRiderWindow
    {
        public AddRiderWindow()
        {
            InitializeComponent();
        }
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as RiderRegistrationViewModel;
            if (null == vm) throw new InvalidOperationException("The datacontext is not the expected type");
            if (vm.Registration.HasValidationErrors) return;
            if (vm.Registration.ValidationErrors.Count > 0) return;

            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RemoveEmergencyContactClicked(object sender, RoutedEventArgs e)
        {
            var contact = ((Button)sender).Tag as EmergencyContact;
            var vm = this.DataContext as RiderRegistrationViewModel;
            if(null == vm || null == contact)return;
            vm.RemoveEmergencyContactCommand.Execute(contact);
        }

    }

    
}




