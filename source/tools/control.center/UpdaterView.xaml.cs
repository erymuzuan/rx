using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{
    public partial class UpdaterView
    {
        public UpdaterView()
        {
            InitializeComponent();
            this.Loaded += UpdaterViewLoaded;
        }

        void UpdaterViewLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = new UpdaterViewModel {View = this};
            this.DataContext = vm;
            vm.LogCollection.CollectionChanged += LogCollection_CollectionChanged;

        }

        private void LogCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var items = e.NewItems.Cast<LogEntry>();
                foreach (var t in items)
                {
                    if (t.Message.Trim() == ".")
                    {
                        logTextBox.AppendText(t.Message);
                    }
                    else
                    {
                        var now = DateTime.Now.ToShortTimeString();
                        var message = $"[{now}][{t.Severity}]  {t.Message}";
                        logTextBox.AppendText("\r\n" + message);

                        Delegate caret = new Action(() =>
                        {
                            //outputTextBox.Focus();
                            logTextBox.CaretIndex = logTextBox.Text.Length;
                            logTextBox.ScrollToEnd();

                        });
                        this.Dispatcher.BeginInvoke(caret, DispatcherPriority.ApplicationIdle);
                    }
                }

            }

        }
    }
}
