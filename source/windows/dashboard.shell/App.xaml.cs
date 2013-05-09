using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Bespoke.Sph.Windows
{
    public partial class App
    {
        static App()
        {

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));


        }


        private CompositionContainer m_container;

        [Import("MainWindow")]
        public new Window MainWindow
        {
            get { return base.MainWindow; }
            set { base.MainWindow = value; }
        }



        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.DispatcherUnhandledException += AppDispatcherUnhandledException;
            this.LoadDependencies();
            if (Compose())
            {
                MainWindow.Show();
            }
            else
            {
                Shutdown();
            }

        }

        void AppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var message = e.Exception.ToString();
            var window = new ErrorWindow(message);
            window.ShowDialog();
            e.Handled = true;
            this.Shutdown(-1);
        }

        private void LoadDependencies()
        {
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            if (m_container != null)
            {
                m_container.Dispose();
            }
        }

        private bool Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            catalog.Catalogs.Add(new DirectoryCatalog("."));

            m_container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddPart(this);
            batch.AddExportedValue(m_container);

            try
            {
                m_container.Compose(batch);
            }
            catch (CompositionException compositionException)
            {
                var window = new ErrorWindow(compositionException.ToString());
                window.ShowDialog();
                Shutdown(1);
            }
            return true;
        }
    }
}
