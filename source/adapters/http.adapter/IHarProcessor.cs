using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    public interface IHarProcessor
    {
        bool CanProcess(HttpOperationDefinition op, JToken jt);
        void Process(HttpOperationDefinition op, JToken jt);
    }
}
