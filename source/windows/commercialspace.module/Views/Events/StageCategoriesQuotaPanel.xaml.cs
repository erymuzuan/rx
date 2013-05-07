using System.ComponentModel.Composition;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "Categories and Stages (Route)", Order = 2)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class StageCategoriesQuotaPanel
    {
        public StageCategoriesQuotaPanel()
        {
            InitializeComponent();
        }

        [Import]
        public RideViewModel ViewModel
        {
            get { return this.DataContext as RideViewModel; }
            set { this.DataContext = value; }
        }


        private void AddEventCategoryClicked(object sender, RoutedEventArgs e)
        {
            var cat = new RideCategory();
            var dialog = new CategoryDialog { DataContext = cat };
            dialog.ShowDialog();
            if (dialog.DialogResult ?? false)
            {
                this.ViewModel.Ride.RideCategoryCollection.Add(cat);
            }
        }

        private void DeleteEventCategoryClicked(object sender, RoutedEventArgs e)
        {
            var selectedCategory = categoriesDataGrid.SelectedItem as RideCategory;
            this.ViewModel.Ride.RideCategoryCollection.Remove(selectedCategory);
        }

        private void AddStageCategoryClicked(object sender, RoutedEventArgs e)
        {
            var st = new Stage();
            var stageWindow = new StageDialog { DataContext = new StageViewModel(st) };
            stageWindow.ShowDialog();
            if (stageWindow.DialogResult ?? false)
            {

                if (!(stageWindow.DialogResult ?? false)) return;
                if (string.IsNullOrWhiteSpace(st.Title))
                {
                    MessageBox.Show("Please provide a title for this stage");
                    //cs.Cancel = true;
                    return;
                }

                if (st.HasValidationErrors)
                {
                    var sb = new StringBuilder();
                    foreach (var err in st.ValidationErrors)
                    {
                        sb.AppendFormat("\r\n{0}  --\r\n {1}\r\n",
                                        string.Join(",", err.MemberNames),
                                        err.ErrorMessage);
                    }
                    MessageBox.Show(sb.ToString(), "Stage Validation Errors",
                                    MessageBoxButton.OK);
                    // cs.Cancel = true;
                    return;
                }
                this.ViewModel.Ride.StageCollection.Add(st);
            }

        }


        private void EditCategoryButtonClick(object sender, RoutedEventArgs e)
        {

            var cat = ((Button)sender).Tag as RideCategory;
            if (null == cat) return;
            var eventWindow = new CategoryDialog { DataContext = cat };
            eventWindow.Show();
        }
        private void RemoveCategoryButtonClick(object sender, RoutedEventArgs e)
        {
            this.ViewModel.RemoveCategoryCommand.Execute(((Button)sender).Tag);
        }

        private void EditStageClick(object sender, RoutedEventArgs e)
        {

            var cat = ((Button)sender).Tag as Stage;
            if (null == cat) return;
            var eventWindow = new StageDialog { DataContext = new StageViewModel(cat) };
            eventWindow.Show();
        }
        private void RemoveStageClick(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Ride.StageCollection.Remove((Stage)((Button)sender).Tag);
        }


        private void AddQuotaClicked(object sender, RoutedEventArgs e)
        {
            var q = new Quota();
            var quotaWindow = new QuotaDialog { DataContext = q };
            quotaWindow.Closed += (s, ea) =>
                                      {
                                          if (quotaWindow.DialogResult ?? false)
                                          {
                                              this.ViewModel.Ride.QuotaCollection.Add(q);
                                          }
                                      };
            quotaWindow.Show();
        }

        private void EditQuotaClicked(object sender, RoutedEventArgs e)
        {
            var q = ((Button)sender).Tag as Quota;
            if (null == q) return;
            var quotaWindow = new QuotaDialog() { DataContext = q };
            quotaWindow.Show();
        }

        private void DeleteQuotaClicked(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Ride.QuotaCollection.Remove((Quota)((Button)sender).Tag);
        }
    }
}
