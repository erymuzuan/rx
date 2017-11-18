using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Mangements;

namespace deployment.gui
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
        public Window MainWindow2
        {
            get => MainWindow;
            set => MainWindow = value;
        }



        protected override void OnStartup(StartupEventArgs e)
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            base.OnStartup(e);
            this.LoadDependencies();
            if (Compose() && null != MainWindow)
            {
                MainWindow.Show();
            }
            else
            {
                Shutdown();
            }

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var entry = new LogEntry(e.ExceptionObject as Exception);
            this.MainWindow.Post(m =>
            {
                var window = new ErrorWindow(m);
                window.ShowDialog();
                this.Shutdown(-1);
            }, entry);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var entry = new LogEntry(e.Exception);
            this.MainWindow.Post(m =>
            {
                var window = new ErrorWindow(m);
                window.ShowDialog();
                this.Shutdown(-1);
            }, entry);
        }

        private void LoadDependencies()
        {
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            m_container?.Dispose();
        }

        private bool Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            // TODO : there's conflic with other assemblies
            // catalog.Catalogs.Add(new DirectoryCatalog("."));

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
                Debug.WriteLine(compositionException);
                Debugger.Break();
                Shutdown(1);
            }
            return true;
        }
    }
}
