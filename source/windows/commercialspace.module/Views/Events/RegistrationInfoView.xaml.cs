using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "Registration Information", Order = 3)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class RegistrationInfoView 
    {

        public RegistrationInfoView()
        {
            InitializeComponent();
            openingDate.DisplayDateStart = new DateTime(2011,1,1);
            closingDate.DisplayDateStart = new DateTime(2011,1,1);
            
        }

    }
}
