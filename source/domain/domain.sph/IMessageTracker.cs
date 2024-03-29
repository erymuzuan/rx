﻿using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IMessageTracker
    {
        Task RegisterAcceptanceAsync(MessageTrackingEvent @event);
        Task RegisterSendingToWorkerAsync(MessageTrackingEvent eventData);
        Task RegisterStartProcessingAsync(MessageTrackingEvent eventData);
        Task RegisterCompletedAsync(MessageTrackingEvent eventData);
        Task RegisterDlqedAsync(MessageTrackingEvent eventData);
        Task RegisterRetriedAsync(MessageTrackingEvent eventData);
        Task RegisterDelayedAsync(MessageTrackingEvent eventData);
        Task<MessageTrackingStatus> GetProcessStatusAsync(MessageSlaEvent @event);
        Task RegisterCancelledAsync(MessageTrackingEvent @event);
        Task RegisterCancelRequestedAsync(MessageTrackingEvent @event);
    }

    [Flags]
    public enum MessageTrackingStatus
    {
        NotStarted = 0,
        Started = 1,
        Completed = 2,
        Error = 4,
        Terminated = 8
    }
}