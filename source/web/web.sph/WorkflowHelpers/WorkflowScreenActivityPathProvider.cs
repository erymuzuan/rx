using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
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

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {

            return IsPathVirtual(virtualPath) ? null : base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);

        }

        private bool IsPathVirtual(string virtualPath)
        {
            if (string.IsNullOrWhiteSpace(virtualPath)) return false;

            return virtualPath.ToLowerInvariant().Contains("workflow_");
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            var vp = virtualPath;
            if (!virtualPath.StartsWith("~"))
                vp = "~" + virtualPath;

            var page = FindPage(vp);
            if (page == null)
            {
                return base.GetFile(virtualPath);
            }
            return new ScreenActivityVirtualFile(vp, page);
        }

        private Page FindPage(string virtualPath)
        {
            var vp = virtualPath;
            if (!virtualPath.StartsWith("~"))
                vp = "~" + virtualPath;

            if (m_screens.ContainsKey(virtualPath)) return m_screens[vp];
            
            var context = new SphDataContext();
            var page = context.LoadOne<Page>(p => p.VirtualPath == vp);
            if (null != page)
            {
                m_screens.Add(virtualPath,page);
                return page;

            }
            return null;

        }
    }
}