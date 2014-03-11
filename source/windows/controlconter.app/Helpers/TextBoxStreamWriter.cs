using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Bespoke.Sph.ControlCenter.Helpers
{
    public class TextBoxStreamWriter : TextWriter
    {
        readonly TextBox m_output;

        public TextBoxStreamWriter(TextBox output)
        {
            m_output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            //m_output.AppendText(value.ToString(CultureInfo.InvariantCulture));
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => m_output.AppendText(value.ToString(CultureInfo.InvariantCulture))));
            
        }

        public override void WriteLine(char value)
        {
            base.WriteLine(value);
            //m_output.AppendText(value.ToString(CultureInfo.InvariantCulture));
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new ThreadStart(() => m_output.AppendText(value.ToString(CultureInfo.InvariantCulture))));

        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
