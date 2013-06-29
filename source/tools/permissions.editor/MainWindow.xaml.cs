using System.Globalization;
using System.Linq;

namespace permissions.editor
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindowLoaded;
        }

        void MainWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var list = from i in Enumerable.Range(1, 10)
                select new Permission
                {
                    Name = i.ToString(CultureInfo.InvariantCulture).PadLeft(6,'0'),
                    Description = i.ToString(CultureInfo.InvariantCulture)
                };
            this.DataContext = list;
        }
    }
}
