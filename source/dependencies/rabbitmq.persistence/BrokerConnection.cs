﻿namespace Bespoke.Station.MessagingPersistences
{
    public class BrokerConnection
    {
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}