namespace Bespoke.Sph.Domain.Messaging
{
    public enum MessageReceiveStatus
    {
        Accepted,
        Rejected,
        Dropped,
        Delayed,
        Requeued
    }
}