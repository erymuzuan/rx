using System.Threading.Tasks;


namespace Bespoke.CommercialSpace.Domain
{
    public interface ISettingProvider
    {
        Task<bool> Save(Setting setting);
        Task<string> GetValue(string key);
    }
}
