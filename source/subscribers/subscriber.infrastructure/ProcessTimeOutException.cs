using System;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class ProcessTimeOutException<T> : Exception where T : Entity
    {
        public ProcessTimeOutException(Subscriber<T> subscriber, ulong deliveryTag) : base(string.Format("Time out exception {0} : {1}", subscriber.GetType().FullName, deliveryTag))
        {

        }
    }
}