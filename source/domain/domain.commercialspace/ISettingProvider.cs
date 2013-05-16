using System.Threading.Tasks;


namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface ISettingProvider
    {
        Task<bool> Save(Setting setting);
        Task<string> GetValue(string key);
    }
}
