using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Station.MessagingPersistences
{
    public class MessagingPersistence : IPersistence
    {
        private readonly BrokerConnection m_brokerConnection = new BrokerConnection();
        private readonly string m_exchange;

        public BrokerConnection BrokerConnection
        {
            get { return m_brokerConnection; }
        }

        public MessagingPersistence(BrokerConnection connection, string exchange)
        {
            m_brokerConnection = connection;
            m_exchange = exchange;
        }

        public Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session)
        {
            var tcs = new TaskCompletionSource<SubmitOperation>();
            var broker = new MessagingBroker(m_brokerConnection, m_exchange);
            Console.WriteLine(m_exchange);
            var addedItems = addedOrUpdatedItems.ToList();
            broker.SubmitChanges(addedItems, deletedItems)
                .ContinueWith(_ =>
                    {
                        var so = _.Result;
                        tcs.SetResult(so);

                        broker.Dispose();
                    });

            return tcs.Task;

        }

        public async Task<SubmitOperation> SubmitChanges(Entity item)
        {
            using (var broker = new MessagingBroker(m_brokerConnection, m_exchange))
            {
                return await broker.SubmitChanges(new[] { item }, new Entity[] { });
            }
        }
    }
}