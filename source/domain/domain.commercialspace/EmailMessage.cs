namespace Bespoke.SphCommercialSpaces.Domain
{
    public class EmailMessage
    {
        public string Body { get; set; }
        public string[] To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
    }
}