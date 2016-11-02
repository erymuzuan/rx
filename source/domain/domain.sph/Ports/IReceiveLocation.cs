namespace Bespoke.Sph.Domain
{
    public interface IReceiveLocation
    {
        bool Start();
        bool Stop();
        void Pause();
        void Resume();
    }
}