using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class RoleSettingViewModel
    {
        public ObjectCollection<string> SearchableEntityOptions { get; } = new ObjectCollection<string>();
        public ObjectCollection<JsRoute> Routes { get; } = new ObjectCollection<JsRoute>();
        public ObjectCollection<string> Roles { get; } = new ObjectCollection<string>();
    }
}