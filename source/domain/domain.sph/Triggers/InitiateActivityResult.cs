namespace Bespoke.Sph.Domain
{
    public class InitiateActivityResult : DomainObject
    {
        public string Message { get; set; }
        public object Result { get; set; }
        public string[] NextActivities { get; set; }
        public string ActivityId { get; set; }
        public string Correlation { get; set; }

        public override string ToString()
        {
            return this.ToJsonString();
        }
    }
}