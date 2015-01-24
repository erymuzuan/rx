using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter
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
            get { return MainWindow; }
            set { MainWindow = value; }
        }



        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
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
