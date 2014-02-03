using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public sealed class Isolated<T> : IDisposable where T : MarshalByRefObject
    {
        private AppDomain m_domain;
        private readonly T m_value;

        public Isolated()
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;
            setup.ApplicationBase = ConfigurationManager.SubscriberPath;

            m_domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(),
                null, setup);

            var type = typeof(T);
            m_value = (T)m_domain.CreateInstanceFromAndUnwrap(type.Assembly.Location, type.FullName);
        }

        public T Value
        {
            get
            {
                return m_value;
            }
        }

        public void Dispose()
        {
            if (m_domain != null)
            {
                AppDomain.Unload(m_domain);

                m_domain = null;
            }
        }
    }
}