namespace Bespoke.Sph.Domain
{
    public partial class ScriptFunctoid : Functoid
    {
        public override T Convert<T,TArg>(TArg arg)
        {
            var script = ObjectBuilder.GetObject<IScriptEngine>();
            return script.Evaluate<T, TArg>(this.Expression, arg);
        }
    }
}