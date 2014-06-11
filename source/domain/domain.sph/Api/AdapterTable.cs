namespace Bespoke.Sph.Domain.Api
{
    public class AdapterTable
    {
        public string Name { get; set; }
        public string[] Parents { get; set; }
        public string[] Children { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}