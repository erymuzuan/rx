using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.SqlServer.Extensions
{
    public static class DomainObjectExtension
    {
        public static AttachedProperty AddAttachedProperty<T>(this DomainObject attachedTo, string name, T value)
        {
            return new AttachedProperty(name, value, attachedTo, "SqlServer2016");
        }
    }
}
