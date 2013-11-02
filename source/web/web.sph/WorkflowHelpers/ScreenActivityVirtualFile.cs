using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace Bespoke.Sph.Web.WorkflowHelpers
{
    public class ScreenActivityVirtualFile : VirtualFile
    {
        private readonly string m_virtualPath;
        private readonly byte[] m_data;
        private static readonly Dictionary<string, object> ReadList = new Dictionary<string, object>();

        public ScreenActivityVirtualFile(string virtualPath, byte[] data)
            : base(virtualPath)
        {
            m_virtualPath = virtualPath;
            this.m_data = data;
            /*
            Console.WriteLine("--------------- XXXXXXXXXX ------------");
            Console.WriteLine("Finding page for :" + virtualPath);
             * */
        }

        public override Stream Open()
        {
            /*
            if (ReadList.ContainsKey(m_virtualPath)) return new MemoryStream(new byte[] { });
            ReadList.Add(m_virtualPath, true);
            */
            return new MemoryStream(m_data);
        }
    }
}