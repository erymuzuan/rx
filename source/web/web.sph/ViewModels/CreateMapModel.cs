namespace Bespoke.Sph.Web.ViewModels
{
    public class CreateMapModel
    {
        public string EncodedPath { get; set; }
        public string Type { get; set; }
        public string Tag { get; set; }

        public override string ToString()
        {
            return this.EncodedPath;
        }
    }
}