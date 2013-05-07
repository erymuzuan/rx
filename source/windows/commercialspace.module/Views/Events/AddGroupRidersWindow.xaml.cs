using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class AddGroupRidersWindow 
    {
        public AddGroupRidersWindow()
        {
            InitializeComponent();
            this.Loaded += AddGroupRidersWindowLoaded;
        }

        void AddGroupRidersWindowLoaded(object sender, RoutedEventArgs e)
        {
            var categories = (ObjectCollection<string>)this.Resources["CategoryCollection"];
            categories.AddRange(this.Categories);
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(GroupName))
            {
                MessageBox.Show("Please enter the group name", "Group Name Missing", MessageBoxButton.OK);
                return;
            }

            if (RegistrationCollection.Count <= 0)
            {
                MessageBox.Show("No rider information is given", "Group Participant Missing", MessageBoxButton.OK);
                return;
            }

            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        public static readonly DependencyProperty GroupNameProperty =
            DependencyProperty.Register("GroupName", typeof(string), typeof(AddGroupRidersWindow), new PropertyMetadata(string.Empty));

        
        public ObjectCollection<Registration> RegistrationCollection
        {
            get { return (ObjectCollection<Registration>)GetValue(RegistrationCollectionProperty); }
            set { SetValue(RegistrationCollectionProperty, value); }
        }

        public static readonly DependencyProperty RegistrationCollectionProperty =
            DependencyProperty.Register("RegistrationCollectionProperty", typeof(ObjectCollection<Registration>), typeof(AddGroupRidersWindow), new PropertyMetadata(new ObjectCollection<Registration>()));


        public ObjectCollection<string> Categories
        {
            get { return (ObjectCollection<string>)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }

        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(ObjectCollection<string>), typeof(AddGroupRidersWindow), new PropertyMetadata(new ObjectCollection<string>()));

        
        private void AddNewRiderClicked(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as RidersViewModel;
            if (null == vm) return;
            var item = new Registration
                                                {
                                                    RideId = vm.Ride.RideId, 
                                                    Group=this.GroupName, 
                                                    State=string.Empty, 
                                                    Country=string.Empty,
                                                    Status = "New",
                                                    PaymentStatus = "No",
                                                    InvoiceNo = string.Empty
                                                };
            
            this.RegistrationCollection.Add(item);
        }

        private void RemoveRiderClicked(object sender, RoutedEventArgs e)
        {
            var rider = ((Button) sender).Tag as Registration;
            this.RegistrationCollection.Remove(rider);
        }
    }

    public class CategoryCollection : ObjectCollection<string> { }
}

