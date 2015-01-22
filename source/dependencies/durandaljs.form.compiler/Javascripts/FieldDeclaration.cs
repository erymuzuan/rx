namespace Bespoke.Sph.FormCompilers.DurandalJs.Javascripts
{
    internal class FieldDeclaration
    {
        public string Name { get; set; }
        public string Intializer { get; set; }

        public override string ToString()
        {
            return string.Format("      {0} = {1}", this.Name, this.Intializer);
        }
    }
}