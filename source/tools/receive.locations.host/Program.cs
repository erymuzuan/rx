using System;
using Topshelf;

namespace receive.locations.host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(config =>
            {
                config.Service<IReceiveLocation>(svc =>
                {
                    svc.ConstructUsing(() =>
                    {
                        dynamic loc = new ConverterService();
                        return (IReceiveLocation)loc;
                    });

                    svc.WhenStarted(x => x.Start());
                    svc.WhenStopped(x => x.Stop());
                    svc.WhenPaused(x => x.Pause());
                    svc.WhenShutdown(x => x.Stop());
                    svc.WhenCustomCommandReceived((x, g, i) =>
                    {
                        Console.WriteLine(g);
                        Console.WriteLine(i);
                    });
                });
                config.SetServiceName("RxReceiveLocationHost");
                config.SetDisplayName("Rx Receive location host service");
                config.SetDescription("Rx Developer receive location default host");

                config.StartAutomatically();
            });
        }
    }
}
