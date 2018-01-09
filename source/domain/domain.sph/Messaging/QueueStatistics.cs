namespace Bespoke.Sph.Domain.Messaging
{
    public class QueueStatistics
    {
        public long LongCount { get; set; }
        public int Count { get; set; }
        public int Processing { get; set; }
        public double DeliveryRate { get; set; }
        public double PublishedRate { get; set; }
        public double MemoryUsed { get; set; }
        public double StorageUsed { get; set; }
    }
}