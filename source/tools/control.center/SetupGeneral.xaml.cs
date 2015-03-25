using System.Windows;

namespace Bespoke.Sph.ControlCenter
{

    public partial class SetupGeneral
    {
        public SetupGeneral()
        {
            InitializeComponent();
            this.Loaded += SetupGeneral_Loaded;
        }

        private void SetupGeneral_Loaded(object sender, RoutedEventArgs e)
        {
            this.Post(() =>
            {
                appNameTextbox.Focus();

            });
        }
    }
}
