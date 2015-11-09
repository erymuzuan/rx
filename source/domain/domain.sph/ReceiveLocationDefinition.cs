using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public abstract class ReceiveLocationDefinition : Entity
    {
        public string Name { get; set; }
        public virtual string CodeNamespace => $"{ConfigurationManager.ApplicationName}.ReceiveLocations.{this.Name}";
        // call the adapter
        // pipe line processing, handles encryption, compression etc
        // dissambler - what the data is too big, split it, use stream
        protected abstract Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces);
    }
}
