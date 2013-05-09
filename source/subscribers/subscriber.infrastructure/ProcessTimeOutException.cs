using System;
using Bespoke.Sph.Subscribers.Infrastructure;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class ProcessTimeOutException : Exception
    {
        public ProcessTimeOutException(Subscriber subscriber, ulong deliveryTag) : base(string.Format("Time out exception {0} : {1}", subscriber.GetType().FullName, deliveryTag))
        {

        }
    }
}