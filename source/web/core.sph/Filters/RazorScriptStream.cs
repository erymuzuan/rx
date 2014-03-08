using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Web.Filters
{
    public class RazorScriptStream : Stream
    // This filter changes all characters passed through it to uppercase.
    {
        private readonly Stream m_sink;

        public RazorScriptStream(Stream sink)
        {
            m_sink = sink;
        }

        // The following members of Stream must be overriden.
        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position { get; set; }

        public override long Seek(long offset, SeekOrigin direction)
        {
            return m_sink.Seek(offset, direction);
        }

        public override void SetLength(long length)
        {
            m_sink.SetLength(length);
        }

        public override void Close()
        {
            m_sink.Close();
        }

        public override void Flush()
        {
            m_sink.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return m_sink.Read(buffer, offset, count);
        }

        readonly StringBuilder m_output = new StringBuilder();
        // The Write method actually does the filtering.
        public override void Write(byte[] buffer, int offset, int count)
        {
            var szBuffer = Encoding.UTF8.GetString(buffer, offset, count);
            m_output.Append(szBuffer);
            var scriptBuffer = ExtractScriptFromHtml(m_output.ToString());
            
            var data = Encoding.UTF8.GetBytes(scriptBuffer);
            m_sink.Write(data, 0, data.Length);

        }


        private static string ExtractScriptFromHtml(string html)
        {
            const RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            var matches = Regex.Matches(html,
                @"<script type=\""text/javascript\"" data-script=\""true\"">(?<script>.*?)</script>", option);
            if (matches.Count == 1)
                return matches[0].Groups["script"].Value;
            if (matches.Count == 0)
                return string.Empty;

            var scripts = new StringBuilder();
            foreach (Match m in matches)
            {
                scripts.AppendLine(m.Groups["script"].Value);
                scripts.AppendLine();
            }

            return scripts.ToString();
        }
    }
}
