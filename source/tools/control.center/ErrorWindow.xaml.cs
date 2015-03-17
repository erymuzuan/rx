namespace Bespoke.Station.Windows
{
    public partial class ErrorWindow
    {
        public ErrorWindow(string message)
        {
            InitializeComponent();
            exceptionMessageTextBox.Text = message;
            this.Topmost = true;

        }
      
    }
}
