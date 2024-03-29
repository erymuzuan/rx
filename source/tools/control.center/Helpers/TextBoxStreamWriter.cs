﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Bespoke.Sph.ControlCenter.Helpers
{
    public class TextBoxStreamWriter : TextWriter
    {
        public static string[] ExcludeWhenContains = "/signalr_;resources/images;/images/FunctionField;/images/ConstantField;Action = 0000000;WriteFragment;WriteCompletedInline;CompletionExpected;BufferType;Expect Inline Completion;IncrementMessages;Glimpse.axd"
            .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        private readonly TextBox m_output;
        public const int MAX_LINE = 200;
        private int m_line;
        private readonly object m_lock = new object();
        public TextBoxStreamWriter(TextBox output)
        {
            m_output = output;
        }

        public override void Write(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            if (ExcludeWhenContains.Any(value.Contains)) return;
            base.Write(value);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() => m_output.AppendText(value.ToString(CultureInfo.InvariantCulture))));
        }

        public override void WriteLine(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            if (ExcludeWhenContains.Any(value.Contains)) return;
            base.WriteLine(value);
            Interlocked.Increment(ref m_line);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() => m_output.AppendText(value.ToString(CultureInfo.InvariantCulture) + "\r\n")));

            if (m_line > MAX_LINE)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.SystemIdle, new Action(() => m_output.Clear()));
                lock (m_lock)
                {
                    m_line = 0;
                }
            }
        }
        public override Encoding Encoding => Encoding.UTF8;
    }
}
