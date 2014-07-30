using System;
using System.Text;
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


        public override string GeneratePreCode(FunctoidMap map)
        {
            var block = this.Expression;
            if (!block.EndsWith(";")) return string.Empty;
            
            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLine();
            code.AppendLinf("               Func<{{SOURCE_TYPE}}, {1}> {0} = d =>", this.Name, map.DestinationType.FullName);
            code.AppendLine("                                           {");
            code.AppendLine("                                               " + this.Expression);
            code.AppendLine("                                           };");
            return code.ToString();
        }

        public override string GenerateCode()
        {
            if(string.IsNullOrWhiteSpace(this.Name))throw new InvalidOperationException("Name cannot be empty");
            var block = this.Expression;
            if (!block.EndsWith(";")) return this.Expression;

            return string.Format("{0}(item)", this.Name);
        }

        public string Name{ get; set; }
    }
}