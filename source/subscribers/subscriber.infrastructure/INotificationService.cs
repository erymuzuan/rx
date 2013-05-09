namespace Bespoke.Sph.SubscribersInfrastructure
{
    public interface INotificationService
    {
        void Write(string format, params object[] args);
        void WriteError(string format, params object[] args);
    }
}