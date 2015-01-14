using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.Javascripts
{
    public class Function
    {
        public string Name { get; set; }
        private readonly ObjectCollection<string> m_argumentCollection = new ObjectCollection<string>();

        public ObjectCollection<string> ArgumentCollection
        {
            get { return m_argumentCollection; }
        }
    }
}