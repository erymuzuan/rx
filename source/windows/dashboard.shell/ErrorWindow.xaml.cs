namespace Bespoke.Station.Windows
{
    public partial class ErrorWindow
    {
        public ErrorWindow(string message)
        {
            InitializeComponent();
            exceptionMessageTextBox.Text = message;
            this.IsTopmost = true;

        }
        protected override bool ShouldFocusOnActivate()
        {
            return true;
        }
        protected override bool ShouldActivateOnShow()
        {
            return true;
        }
    }
}
