using System.IO;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements.Commands
{
    public class EntityDefinitionCommand : Command
    {
        public override CommandParameter[] GetArgumentList()
        {
            return new[]
            {
                new CommandParameter("entity",false, "e", "entity-id", "entity-name"),
                new CommandParameter(1, true),
            };
        }

        protected EntityDefinition GetEntityDefinition()
        {
            var id = this.GetCommandValue<string>("entity");
            var file = this.GetCommandValue<string>(1);
            if (!string.IsNullOrWhiteSpace(id))
                file = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\{id}.json";
            if (!string.IsNullOrWhiteSpace(id) && !File.Exists(file))// try with ED Name
                file = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\{GetEntityDefinitionId(id)}.json";

            EntityDefinition ed = null;
            if (File.Exists(file))
                ed = file.DeserializeFromJsonFile<EntityDefinition>();

            return ed;
        }
        private static string GetEntityDefinitionId(string name)
        {
            var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\", "*.json");
            foreach (var file in files)
            {
                var ed = file.DeserializeFromJsonFile<EntityDefinition>();
                if (ed.Name == name) return ed.Id;
            }

            return null;
        }
    }
}