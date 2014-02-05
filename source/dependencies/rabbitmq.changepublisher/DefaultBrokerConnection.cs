namespace Bespoke.Sph.RabbitMqPublisher
{
    public class DefaultBrokerConnection: IBrokerConnection
    {
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
    }
}