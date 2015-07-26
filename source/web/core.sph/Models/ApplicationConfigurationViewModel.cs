using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Models
{
    public class ApplicationConfigurationViewModel
    {
        public string StateOptions { get; set; }
        public string ApplicationName => ConfigurationManager.ApplicationName;
        public string ApplicationFullName => ConfigurationManager.ApplicationFullName;
        public string DepartmentOptions { get; set; }
        public ObjectCollection<JsRoute> Routes { get; } = new ObjectCollection<JsRoute>();
        public string StartModule { get; set; }
        public Designation Designation { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}