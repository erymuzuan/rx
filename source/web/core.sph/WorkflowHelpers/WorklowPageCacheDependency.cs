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

        public override string GetUniqueID()
        {
            return m_virtualPath;
        }

        void ListenerChanged(object sender, EntityChangedEventArgs<Page> e)
        {
            this.ResetDependency();

        }

        private void ResetDependency()
        {
            if (!this.HasChanged)
            {
                this.SetUtcLastModified(DateTime.Now);
                this.NotifyDependencyChanged(this, EventArgs.Empty);
            }

        }
    }
}