using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class SphCompilerResult
    {
        private readonly ObjectCollection<BuildError> m_errorsCollection = new ObjectCollection<BuildError>();
        public bool Result { get; set; }
        public ObjectCollection<BuildError> Errors

        {
            get { return m_errorsCollection; }
        }

        public string Output { get; set; }
        public string[] Outputs { get; set; }
        public byte[] Buffer { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}