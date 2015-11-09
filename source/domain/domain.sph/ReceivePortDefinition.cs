using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [EntityType(typeof(ReceivePortDefinition))]
    public partial class ReceivePortDefinition : Entity
    {
        public virtual string CodeNamespace => $"{ConfigurationManager.ApplicationName}.ReceivePort.{this.Name}";
        // call the adapter
        // pipe line processing, handles encryption, compression etc
        // dissambler - what the data is too big, split it, use stream
        protected virtual Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options,
            params string[] namespaces)
        {
            throw new Exception("Not implemented");
        }


    }
}