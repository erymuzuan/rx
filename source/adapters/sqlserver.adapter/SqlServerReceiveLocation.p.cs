namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class SqlServerReceiveLocation
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public bool Trusted { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public Polling Polling { get; set; } = new Polling();
    }
}