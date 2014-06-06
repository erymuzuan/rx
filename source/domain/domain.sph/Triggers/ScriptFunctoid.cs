using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class ScriptFunctoid : Functoid
    {
        public override T Convert<T,TArg>(TArg arg)
        {
            var script = ObjectBuilder.GetObject<IScriptEngine>();
            return script.Evaluate<T, TArg>(this.Expression, arg);
        }

        public override Task<string> ConvertAsync(object source)
        {
            var engine = ObjectBuilder.GetObject<IScriptEngine>();
            var val = engine.Evaluate<object, object>(this.Expression, source);
            return Task.FromResult(string.Format("{0}",val));
        }
    }
}