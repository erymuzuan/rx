using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Bespoke.Sph.Windows.Helpers;
using Bespoke.Sph.Windows.Infrastructure;
using Bespoke.Sph.Windows.Properties;
using Bespoke.Sph.Windows.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.RibbonView;
using System.Linq;

namespace Bespoke.Sph.Windows
{
    [Export("MainWindow", typeof(Window))]
    [Export(typeof(IView))]
    public partial class DashboardWindow : IPartImportsSatisfiedNotification, IView
    {
        readonly Storyboard m_startOverlay;
        readonly Storyboard m_stopOVerlay;
        public DashboardWindow()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();

            InitializeComponent();

            if (this.IsInDesignMode())
                return;
            m_startOverlay = this.Resources["Startoverlay"] as Storyboard;
            m_stopOVerlay = (Storyboard)this.Resources["Stopoverlay"];
            m_stopOVerlay.Completed += (s, e) => { overlay.Visibility = Visibility.Collapsed; };
            versionLabel.Text = " " + Assembly.GetExecutingAssembly().GetName().Version;
            this.Closing += DashboardWindowClosing;

        }

        private bool m_hasOverlay;
        protected override void OnDeactivated(EventArgs e)
        {
            if (this.IsInDesignMode()) return;
            if (this.OwnedWindows.Count > 0 || Application.Current.Windows.Count > 1)
            {
                Debug.WriteLine("Deactivated.....");
                overlay.Visibility = Visibility.Visible;
                m_startOverlay.Begin(this);
                m_hasOverlay = true;
            }


            base.OnDeactivated(e);
        }

        protected override void OnActivated(EventArgs e)
        {
            if (m_hasOverlay)
            {
                m_stopOVerlay.Begin(this);
            }
            m_hasOverlay = false;
            base.OnActivated(e);
        }

        void DashboardWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var model = new MainWindowClosingModel();
            Messenger.Default.Send(model);
            if (model.Cancel)
            {
                e.Cancel = true;
                if (model.MesssageCollection.Any(s => !string.IsNullOrWhiteSpace(s)))
                    this.Alert(string.Join("\r\n", model.MesssageCollection), AlertImage.Information);

            }
        }

        [Import]
        public MainWindowViewModel ViewModel
        {
            get { return this.DataContext as MainWindowViewModel; }
            set { this.DataContext = value; }
        }

        [ImportMany("RibbonItem", typeof(object), AllowRecomposition = true)]
        public Lazy<object, IViewMetadata>[] RibbonItems { get; set; }


        public void OnImportsSatisfied()
        {
            Messenger.Default.Register<ChangeViewArgs>(this, ChangeView);
            Messenger.Default.Register<StatusBarMessage>(this, StatusBarMessageChanged);
            foreach (var @group in this.ViewModel.ViewGroupCollection.OrderBy(g => g.Caption))
            {
                var resource = new ResourceManager("Bespoke.Station.Windows.Properties.Resources", typeof(Resources).Assembly);
                var caption = resource.GetString(@group.Caption + "Tab");
                var tab = new RadRibbonTab { Header = caption };
                foreach (var sub in @group.ItemCollection)
                {
                    var subPanel = new RadRibbonGroup { Header = sub.Caption };
                    tab.Items.Add(subPanel);
                    var @group1 = @group;
                    var sub1 = sub;
                    var ribbonItems =
                        this.RibbonItems
                        .Where(d => d.Metadata.Group == @group1.Caption && d.Metadata.SubGroup == sub1.Caption);
                    foreach (var rbi in ribbonItems)
                    {
                        subPanel.Items.Add(rbi.Value);
                    }

                    foreach (var item in sub.ItemCollection.OrderBy(c => c.Order))
                    {
                        if (item.IsHidden) continue;
                        //#36B8FF
                        var uri = "pack://application:,,,/Images/order_big.gif";
                        if (!string.IsNullOrWhiteSpace(item.Image) && !item.Image.StartsWith("pack://application:,,,"))
                        {
                            uri = "pack://application:,,,/dashboard.shell;component/" + item.Image;
                        }
                        var image = new BitmapImage(new Uri(uri));
                        var button = new RadRibbonButton
                                         {
                                             Size = ButtonSize.Large,
                                             Text = item.Caption,
                                             IsAutoSize = false,
                                             Command = item.ShowViewCommand,
                                             CommandParameter = item.CommandParameter,
                                             Width = 60,
                                             HorizontalAlignment = HorizontalAlignment.Center,
                                             FontSize = 12,
                                             LargeImage = image
                                         };

                        if (null != item.CommandVmCommand)
                            button.SetBinding(ButtonBase.CommandProperty, new Binding("CommandVmCommand"));
                        if (null != item.CommandParameter)
                            button.SetBinding(ButtonBase.CommandParameterProperty, new Binding("CommandParameter"));
                        button.SetBinding(RadRibbonButton.TextProperty, new Binding("Caption"));
                        button.SetBinding(RadRibbonButton.LargeImageProperty, new Binding("Image"));
                        button.SetBinding(ToolTipProperty, new Binding("Tooltip"));
                        ToolTipService.SetToolTip(button, item.Tooltip);

                        button.DataContext = item;
                        if (null != item.CommandVmCommand)
                        {
                            button.Command = item.CommandVmCommand;
                            button.CommandParameter = item.CommandParameter;
                        }
                        subPanel.Items.Add(button);
                    }
                }
                ribbon.Items.Add(tab);
            }
        }

        private void StatusBarMessageChanged(StatusBarMessage obj)
        {
            ((IView)this).Post(message => this.statusPanel.DataContext = message, obj);
        }

        private void ChangeView(ChangeViewArgs arg)
        {
            foreach (var c in contentPanel.Children)
            {
                var uc = c as UserControl;
                if (null != uc)
                    uc.Visibility = Visibility.Collapsed;
            }
            var panel = this.ViewModel.SelectedView;
            if (!contentPanel.Children.Contains(panel))
                contentPanel.Children.Add(panel);

            panel.Visibility = Visibility.Visible;
            this.contentBusyIndicator.DataContext = this.statusBarText.DataContext = panel.DataContext;
            contextTextBox.Text = string.Format("{0}:{1}", arg.ViewName, panel.DataContext.GetType().Name);
        }

        public DispatcherObject View
        {
            get { return this; }
            set { }
        }

        public bool Confirm(string message)
        {
            var result = false;
            var wait = new AutoResetEvent(false);
            var parameter = new DialogParameters
                                {
                                    Content = message,
                                    ModalBackground = new SolidColorBrush(Colors.Red),
                                    Closed = (s, e) =>
                                    {
                                        result = e.DialogResult ?? false;
                                        wait.Set();
                                    },
                                    CancelButtonContent = "Batal",
                                    OkButtonContent = "OK",
                                    Header = "Station MS",
                                    Owner = this
                                };
            RadWindow.Confirm(parameter);
            wait.WaitOne();
            return result;
        }

        public bool Alert(string message, AlertImage image)
        {
            var result = false;
            var wait = new AutoResetEvent(false);
            var parameter = new DialogParameters
            {
                Content = message,
                ModalBackground = new SolidColorBrush(Colors.Red),
                Closed = (s, e) =>
                {
                    result = e.DialogResult ?? false;
                    wait.Set();
                },
                CancelButtonContent = "Batal",
                OkButtonContent = "OK",
                Header = "Station MS",
                Owner = this
            };

            RadWindow.Alert(parameter);
            wait.WaitOne();
            return result;
        }
    }
}
