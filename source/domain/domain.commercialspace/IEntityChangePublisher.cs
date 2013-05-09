﻿using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bespoke.CommercialSpace.Domain
{
    public interface IEntityChangePublisher
    {
        Task PublishAdded(IEnumerable<Entity> attachedCollection);
        Task PublishChanges(IEnumerable<Entity> attachedCollection);
        Task PublishDeleted(IEnumerable<Entity> deletedCollection);
    }
}