using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Cycling.Windows.RideOrganizerModule.Contants;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export("ViewContract", typeof(UserControl))]
    [ViewMetadata(Group = ViewGroup.Home, Image = "/Images/customer.png", Name = ViewNames.EVENT_DETAIL_VIEW, Caption = "Event details", Order = 1)]
    public partial class EventDetailsView : IPartImportsSatisfiedNotification
    {
        [Import]
        public RideViewModel ViewModel { get { return this.DataContext as RideViewModel; } set { this.DataContext = value; } }


        [ImportMany("RideViewContract", typeof(UserControl), AllowRecomposition = true)]
        public Lazy<UserControl, IViewMetadata>[] Views { get; set; }

        public EventDetailsView()
        {
            InitializeComponent();
        }


        public void OnImportsSatisfied()
        {
            var list = this.Views
                .OrderBy(n => n.Metadata.Order)
                .Select(n => n.Metadata)
                .ToList();
            navPanel.ItemsSource = list;

            var view = this.Views.OrderBy(v => v.Metadata.Order).First();
            frame.Children.Add(view.Value);

        }

        private void NavLinkClick(object sender, RoutedEventArgs e)
        {
            var ni = ((Button)sender).Tag as IViewMetadata;
            if (null == ni) return;
            var view = this.Views.Single(l => l.Metadata.Caption == ni.Caption).Value;
            if (!frame.Children.Contains(view))
                frame.Children.Add(view);

            SwapView(view);
        }

        private void SwapView(UserControl view)
        {
            foreach (UserControl c in frame.Children)
            {
                if (c.Visibility == Visibility.Visible)
                    c.Visibility = Visibility.Collapsed;
            }
            view.Visibility = Visibility.Visible;
        }
    }

}
