using System;
using System.Linq;
using System.Threading.Tasks;


namespace Bespoke.CommercialSpace.Domain
{
    public sealed class PersistenceSession : IDisposable
    {
        private SphDataContext m_context;

        private readonly ObjectCollection<Entity> m_attachedCollection = new ObjectCollection<Entity>();
        private readonly ObjectCollection<Entity> m_deletedCollection = new ObjectCollection<Entity>();

        internal ObjectCollection<Entity> AttachedCollection
        {
            get { return m_attachedCollection; }
        }
        internal ObjectCollection<Entity> DeletedCollection
        {
            get { return m_deletedCollection; }
        }

        internal PersistenceSession(SphDataContext context)
        {
            m_context = context;
        }

        public void Delete(params Entity[] entities)
        {
            if (null == m_context)
                throw new ObjectDisposedException("This session has been completed or terminated");
            m_deletedCollection.AddRange(entities);
        }


        public void Attach(params Entity[] entities)
        {
            if (null == m_context)
                throw new ObjectDisposedException("This session has been completed or terminated");

            foreach (var t in entities.Where(e => string.IsNullOrWhiteSpace(e.WebId)))
            {
                t.WebId = Guid.NewGuid().ToString();
            }
            m_attachedCollection.AddRange(entities);
        }



        public void Dispose()
        {
            m_context = null;
            m_attachedCollection.Clear();
            m_deletedCollection.Clear();
        }

        public async Task<SubmitOperation> SubmitChanges()
        {
            var so = await m_context.SubmitChangesAsync(this);
            m_attachedCollection.Clear();
            m_deletedCollection.Clear();
            m_context = null;
            return so;
        }
    }
}
