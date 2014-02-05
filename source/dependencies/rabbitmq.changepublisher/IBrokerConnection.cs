namespace Bespoke.Sph.RabbitMqPublisher
{
    public interface IBrokerConnection
    {
        string Host { get;  }
        string UserName { get;  }
        string Password { get; }
        int Port { get; set; }
        string VirtualHost { get;}
    }
}
