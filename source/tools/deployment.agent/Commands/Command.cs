using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements.Commands
{
    public abstract class Command
    {
        public EntityDefinition Ed { get; }

        protected Command()
        {

        }
        protected Command(EntityDefinition ed)
        {
            Ed = ed;
        }

        public abstract CommandParameter[] GetArgumentList();
        public virtual bool UseAsync => false;

        public virtual bool ShouldContinue()
        {
            return false;
        }

        public virtual Task ExecuteAsync(EntityDefinition ed)
        {
            return Task.FromResult(0);
        }
        public virtual void Execute(EntityDefinition ed)
        {
        }

        protected T GetCommandValue<T>(string name)
        {
            var cmd = this.GetArgumentList().SingleOrDefault(x => x.Name == name);
            return null == cmd ? default(T) : cmd.GetValue<T>();
        }

        public virtual bool IsSatisfied()
        {
            var requiredParameters = this.GetArgumentList().Where(x => x.IsRequired);
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new []{":"}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Replace("/", ""))
                .ToArray();

            foreach (var prm in requiredParameters)
            {
                var switches = prm.Switches.Intersect(args);
                if (!switches.Any()) return false;
            }

            return true;
        }
    }
}