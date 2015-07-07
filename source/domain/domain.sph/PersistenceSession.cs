using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Bespoke.Sph.Domain
{
    public sealed class PersistenceSession : IDisposable
    {
        private SphDataContext m_context;

        internal ObjectCollection<Entity> AttachedCollection { get; } = new ObjectCollection<Entity>();

        internal ObjectCollection<Entity> DeletedCollection { get; } = new ObjectCollection<Entity>();

        internal PersistenceSession(SphDataContext context)
        {
            m_context = context;
        }

        public void Delete(params Entity[] entities)
        {
            if (null == m_context)
                throw new ObjectDisposedException("This session has been completed or terminated");
            DeletedCollection.AddRange(entities);
        }


        public void Attach(params Entity[] entities)
        {
            if (null == m_context)
                throw new ObjectDisposedException("This session has been completed or terminated");

            var userName = ObjectBuilder.GetObject<IDirectoryService>().CurrentUserName;
            entities.Where(e => string.IsNullOrWhiteSpace(e.WebId)).ToList().ForEach(t => t.WebId = Strings.GenerateId());

            foreach (var t in entities.Where(e => string.IsNullOrWhiteSpace(e.CreatedBy) || e.CreatedDate == DateTime.MinValue))
            {
                t.CreatedBy = userName;
                t.ChangedBy = userName;
                t.CreatedDate = DateTime.Now;
                t.ChangedDate = DateTime.Now;
            }
            foreach (var t in entities)
            {
                t.ChangedBy = userName;
                t.ChangedDate = DateTime.Now;
            }
            AttachedCollection.AddRange(entities);
        }



        public void Dispose()
        {
            m_context = null;
            AttachedCollection.Clear();
            DeletedCollection.Clear();
        }

        public async Task<SubmitOperation> SubmitChanges(string operation = "", Dictionary<string, object> headers = null)
        {
            var so = await m_context.SubmitChangesAsync(operation, this, headers);
            AttachedCollection.Clear();
            DeletedCollection.Clear();
            m_context = null;
            return so;
        }
    }
}
