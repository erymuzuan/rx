using System;
using System.Web.Caching;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.WorkflowHelpers
{
    public class WorklowPageCacheDependency : CacheDependency
    {
        private readonly string m_virtualPath;

        public WorklowPageCacheDependency(string virtualPath, IEntityChangedListener<Page> listener)
        {

            var vp = virtualPath;
            if (!virtualPath.StartsWith("~"))
                vp = "~" + virtualPath;

            m_virtualPath = vp;
            listener.Changed += ListenerChanged;
            this.SetUtcLastModified(DateTime.Today);
           
        }

        void ListenerChanged(object sender, EntityChangedEventArgs<Page> e)
        {
            Console.WriteLine("{0} - {1}", m_virtualPath, e.Item.VirtualPath);
            this.SetUtcLastModified(DateTime.Now);
            this.NotifyDependencyChanged(this, EventArgs.Empty);

        }
    }
}