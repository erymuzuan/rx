namespace Bespoke.Sph.Web.ViewModels
{
    public class RoleModel
    {
        public RoleModel()
        {
            
        }
        public RoleModel(string role)
        {
            this.Name = role;
            this.Description = role;
            this.Role = role;
            this.IsActive = true;
        }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}