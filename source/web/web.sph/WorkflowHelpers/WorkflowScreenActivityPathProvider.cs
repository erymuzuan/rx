using System.Collections.Generic;
using System.Web.Hosting;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.WorkflowHelpers
{
    public class WorkflowScreenActivityPathProvider : VirtualPathProvider
    {
        private readonly Dictionary<string, Page> m_screens = new Dictionary<string, Page>(); 

        public override bool FileExists(string virtualPath)
        {
            var page = FindPage(virtualPath);
            if (page == null)
            {
                return base.FileExists(virtualPath);
            }
            return true;
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            var page = FindPage(virtualPath);
            if (page == null)
            {
                return base.GetFile(virtualPath);
            }
            return new ScreenActivityVirtualFile(virtualPath, page);
        }

        private Page FindPage(string virtualPath)
        {
            if (m_screens.ContainsKey(virtualPath)) return m_screens[virtualPath];
            
            var context = new SphDataContext();
            var page = context.LoadOne<Page>(p => p.VirtualPath == virtualPath);
            if (null != page)
            {
                m_screens.Add(virtualPath,page);
                return page;

            }
            return null;

        }
    }
}