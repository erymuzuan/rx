namespace Bespoke.Sph.Domain
{
    public partial class Designation : Entity
    {
        public string Title { get; set; }
        public Owner Owner { get; set; }
        public int Option { get; set; }
    }
}
