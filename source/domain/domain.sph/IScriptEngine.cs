namespace Bespoke.Sph.Domain
{
    public interface IScriptEngine
    {
       // object Evaluate(string script, Entity item);
        T Evaluate<T, T1>(string script, T1 arg1);
        T Evaluate<T, T1,T2>(string script, T1 arg1, T2 arg2);
        T Evaluate<T, T1,T2,T3>(string script, T1 arg1, T2 arg2,T3 arg3);
    }
}