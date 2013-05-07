using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Cycling.Windows.RideOrganizerModule.Contants;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export("ViewContract", typeof(UserControl))]
    [ViewMetadata(Group = ViewGroup.Home, Image = "/Images/customer.png", Name = ViewNames.EVENT_REGISTRATION_LIST_VIEW, Caption = "Registered riders", Order = 5)]
    public partial class RegistrationPivotView //: IPartImportsSatisfiedNotification
    {
        public RegistrationPivotView()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this)) return;
        }

        [Import]
        public RegistrationListViewModel ViewModel
        {
            get { return this.DataContext as RegistrationListViewModel; }
            set { this.DataContext = value; }
        }
        /*
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            EventId = PresentationHelper.ParseIdFromUri(e.Uri);
            if (null == this.ViewModel) return;
            this.DataContext = this.ViewModel;
            this.ViewModel.LoadEventInformation(this.EventId); 
            
            PivotViewerControl.CustomActions.Add(new CustomAction("Details", new Uri("http://www.angrychains.com/images/icon-arrow-white-right.png", UriKind.RelativeOrAbsolute), "Details", "details"));
            PivotViewerControl.CustomActions.Add(new CustomAction("Delete", new Uri("http://www.angrychains.com/images/icon-delete.png", UriKind.RelativeOrAbsolute), "Delete registration", "delete"));

            //
            //dummy call for cxml
            var cxml = new Uri(Application.Current.Host.Source, "../angrychains.cxml");
            var uri = string.Format("{0}?id={1}", cxml, EventId);
            PivotViewerControl.LoadCollection(uri, null);
        }
        
        public void OnImportsSatisfied()
        {
            if (EventId > 0 && null != this.ViewModel)
            {
                this.DataContext = ViewModel;
                this.ViewModel.LoadEventInformation(this.EventId);
            }
        }

        public static readonly DependencyProperty EventIdProperty =
            DependencyProperty.Register("EventId", typeof(int), typeof(RegistrationPivotView), new PropertyMetadata(0));

        public int EventId
        {
            get { return (int)GetValue(EventIdProperty); }
            set { SetValue(EventIdProperty, value); }
        }
        
        private void CustomActionClicked(object sender, ItemActionEventArgs e)
        {
            var item = PivotViewerControl.GetItem(e.ItemId);
            var registrationId = int.Parse(item.Facets["RegistrationId"][0]);
            if (e.CustomActionId == "details")
            {
                ViewModel.ViewRegistrationDetails(registrationId);
            }
            if (e.CustomActionId == "delete")
            {
                if (MessageBox.Show(string.Format("Are you sure you want to delete '{0}' data ?", item.Name), 
                    "Delete Registration", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    ViewModel.DeleteRegistration(registrationId);
                }
            }
        }

        private void PivotLinkClicked(object sender, LinkEventArgs e)
        {
            var uri = e.Link;
            var parts = uri.ToString().Split(new[] { '?', '&' });
            var action = parts[1].Split(new[] {'='})[1];
            var id = int.Parse(parts[2].Split(new[] {'='})[1]);
            //MessageBox.Show(string.Format("{0}:{1}", action, id));
            switch(action)
            {
                case "view":
                    ViewModel.ViewPayment(id);
                    break;
                case "pay":
                    ViewModel.MakePayment(id);
                    break;
            }
        }*/
    }
}
