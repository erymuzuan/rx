using System.Windows;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements
{
    public partial class ErrorWindow
    {
        private readonly LogEntry m_entry;

        public ErrorWindow(LogEntry entry)
        {
            m_entry = entry;
            InitializeComponent();
            this.Loaded += ErrorWindow_Loaded;
        }

        private void ErrorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.exceptionMessageTextBox.Text = m_entry.ToString();
        }
    }
}
