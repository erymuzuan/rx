using System.IO;
using System.Text;
using System.Web.Hosting;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.WorkflowHelpers
{
    public class ScreenActivityVirtualFile : VirtualFile
    {
        private readonly Page m_page;

        public ScreenActivityVirtualFile(string virtualPath, Page page)
            : base(virtualPath)
        {
            this.m_page = page;
        }

        public override Stream Open()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(m_page.Code));
        }
    }
}