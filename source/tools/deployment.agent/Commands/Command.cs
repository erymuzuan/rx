using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.Mangements.Commands
{
    public abstract class Command
    {
        public EntityDefinition Ed { get; }
        public ILogger Console = ObjectBuilder.GetObject<ILogger>();

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

        public virtual Task ExecuteAsync()
        {
            return Task.FromResult(0);
        }
        public virtual void Execute()
        {
        }

        protected virtual void WriteVerbose(string message)
        {
            Console.WriteVerbose(message);
        }

        protected virtual void WriteInfo(string message)
        {
            Console.WriteInfo(message);
        }
        protected virtual void WriteWarnig(string message)
        {
            Console.WriteWarning(message);
        }
        protected virtual void WriteError(string message)
        {
            Console.WriteError(message);
        }

        protected T GetCommandValue<T>(int position)
        {
            if (position <= 0)
                throw new ArgumentException(@"Positional argument should be 1 or more", nameof(position));
            var cmd = this.GetArgumentList().SingleOrDefault(x => x.Position == position);
            return null == cmd ? default(T) : cmd.GetValue<T>();
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
                .Select(x => x.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Replace("/", ""))
                .ToArray();

            foreach (var prm in requiredParameters.Where(x => null != x.Switches))
            {
                var switches = prm.Switches.Intersect(args);
                if (!switches.Any()) return false;
            }

            return true;
        }
    }
}