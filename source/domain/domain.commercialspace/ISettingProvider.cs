using System.Threading.Tasks;

namespace Bespoke.Station.Domain
{
    public interface ISettingProvider
    {
        Task<bool> Save(Setting setting);
        Task<string> GetValue(string key);
    }
}
