using System.Reflection;
[assembly: AssemblyTitle("rabbitmq.changepublisher")]
[assembly: AssemblyDescription("Messaging Broker implementation with RabbitMq")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
//
//