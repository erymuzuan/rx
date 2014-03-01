using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;
using Bespoke.Station.Windows.RabbitMqDeadLetter.ViewModels;
using ICSharpCode.AvalonEdit.Search;
using Telerik.Windows.Controls;
using System.Linq;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter
{
    [Export("MainWindow", typeof(Window))]
    public partial class MainWindow : IPartImportsSatisfiedNotification
    {
        [Import]
        public MainViewModel MainViewModel { get; set; }
        [Import]
        public ConnectionViewModel ConnectionViewModel { get; set; }

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();
            this.Closed += MainWindowClosed;
            this.Loaded += MainWindowLoaded;
        }

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var consolas = Fonts.SystemFontFamilies.AsQueryable()
                .SingleOrDefault(a => a.Source == "Consolas");
            if (null != consolas)
                textEditor.FontFamily = consolas;

            SearchPanel.Install(textEditor);
            //textEditor.TextArea.DefaultInputHandler.NestedInputHandlers.Add(new SearchInputHandler(textEditor.TextArea));
        }

        void MainWindowClosed(object sender, System.EventArgs e)
        {
            if (this.ConnectionViewModel.IsConnected)
                this.ConnectionViewModel.Disconnect();
        }

        public void OnImportsSatisfied()
        {
            this.MainViewModel.View = this;
            this.ConnectionViewModel.View = this;
            this.DataContext = this.MainViewModel;
            this.connectionPanel.DataContext = this.ConnectionViewModel;
            this.MainViewModel.PropertyChanged += MainViewModelPropertyChanged;

        }

        private bool m_updatingMessageFlag;
        void MainViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(m_updatingMessageFlag)return;
            if (e.PropertyName == "Message")// && !string.IsNullOrWhiteSpace(this.MainViewModel.Message))
            {
                this.textEditor.Text = this.MainViewModel.Message;
            }
        }

        private void RequeuButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                m_updatingMessageFlag = true;
                this.MainViewModel.Message = textEditor.Text;
                this.MainViewModel.RequeueCommand.Execute(null);
            }
            finally
            {
                m_updatingMessageFlag = false;
            }
        }
    }
}
