using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class ProcessTimeOutException<T> : Exception where T : Entity
    {
        public ProcessTimeOutException(Subscriber<T> subscriber, ulong deliveryTag) : base(
            $"Time out exception {subscriber.GetType().FullName} : {deliveryTag}")
        {

        }
    }
}