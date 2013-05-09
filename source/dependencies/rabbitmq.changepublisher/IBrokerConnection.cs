namespace Bespoke.Sph.RabbitMqPublisher
{
    public interface IBrokerConnection
    {
        string Host { get;  }
        string Username { get;  }
        string Password { get; }
        int Port { get; set; }
        string VirtualHost { get;}
    }
}
