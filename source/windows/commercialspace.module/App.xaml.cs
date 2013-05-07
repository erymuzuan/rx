using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Bespoke.Cycling.Windows.RideOrganizerModule
{
    public partial class App
    {
        static App()
        {

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));


        }
        
    }
}
