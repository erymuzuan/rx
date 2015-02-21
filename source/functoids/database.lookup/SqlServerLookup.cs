using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "SqlServerLookup", FontAwesomeIcon = "database", Category = FunctoidCategory.DATABASE)]
    public class SqlServerLookup : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "connection", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "schema", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "table", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "column", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "predicate", Type = typeof(string) });
            return base.Initialize();

        }

        public override string GetEditorView()
        {
            return "";
        }

        
    }
}
