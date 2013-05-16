using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.CommercialSpace.Domain;

namespace Bespoke.Sph.Commerspace.Web.Dependencies
{
    public class DummyChangeListener<T> : IEntityChangedListener<T> where T : Entity
    {
        public void Run()
        {
            Task.Delay(TimeSpan.FromDays(1));
        }

        public void Stop()
        {
            Task.Delay(TimeSpan.FromDays(1));
        }

        public event EventHandler<T> Changed;

        public void Run(System.Threading.SynchronizationContext synchronizationContext)
        {
            Task.Delay(TimeSpan.FromDays(1));
        }
    }
}
