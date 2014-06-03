using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class StringConcateFunctoid : Functoid
    {
        private readonly ObjectCollection<Field> m_argumentCollection = new ObjectCollection<Field>();

        public ObjectCollection<Field> ArgumentCollection
        {
            get { return m_argumentCollection; }
        }

        public override Task<string> ConvertAsync(object source)
        {
            var context = new RuleContext((Entity)source);
            var args = this.ArgumentCollection.Select(a => a.GetValue(context));
            return Task.FromResult(string.Concat(args.ToArrayString()));
        }
    }
}