namespace scheduler.data.import
{
    public class Queue
    {
        public string Name { get; set; }
        public int MessagesCount { get; set; }
        public double Rate { get; set; }
        public int Unacked { get; set; }

        public Queue(string name, int count, double rate)
        {
            this.Name = name;
            this.MessagesCount = count;
            this.Rate = rate;
        }

        public Queue(string name, int count, double rate, int unacked)
        {
            this.Name = name;
            this.MessagesCount = count;
            this.Rate = rate;
            this.Unacked = unacked;
        }
    }
}