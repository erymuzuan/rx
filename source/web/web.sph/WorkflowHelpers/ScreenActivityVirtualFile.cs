using System.IO;
using System.Web.Hosting;

namespace Bespoke.Sph.Web.WorkflowHelpers
{
    public class ScreenActivityVirtualFile : VirtualFile
    {
        private readonly byte[] m_data;

        public ScreenActivityVirtualFile(string virtualPath, byte[] data)
            : base(virtualPath)
        {
            this.m_data = data;
        }

        public override Stream Open()
        {
            return new MemoryStream(m_data);
        }
    }
}