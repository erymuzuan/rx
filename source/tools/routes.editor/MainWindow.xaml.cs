using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using Telerik.Windows.Controls;

namespace routes.editor
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            StyleManager.ApplicationTheme = new Expression_DarkTheme();
            InitializeComponent();
            this.Loaded += MainWindowLoaded;
        }

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var vm = new MainViewModel();
            this.DataContext = vm;
            vm.Load();
            var css = ConfigurationManager.AppSettings["css-path"] ??
                      @"\project\work\sph\source\web\web.sph\Content\font-awesome.css";
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
            const string pattern = "^\\.(?<class>fa.*?)(:before)? {";
            var matches = Regex.Matches(File.ReadAllText(css), pattern,options);
            var cssClasses = from Match m in matches 
                             select m.Groups["class"].Value;

            this.iconsComboBox.ItemsSource = cssClasses.ToArray();
        }


        private void ViewIconsButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start("http://fortawesome.github.io/Font-Awesome/icons/");
        }
    }
}
