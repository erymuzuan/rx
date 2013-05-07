﻿using System;
using System.Threading;

namespace Bespoke.Station.Domain
{
    public interface IEntityChangedListener<T>  where T : Entity
    {
        void Run();
        void Stop();
        event EventHandler<T> Changed;
        void Run(SynchronizationContext synchronizationContext);
    }
}