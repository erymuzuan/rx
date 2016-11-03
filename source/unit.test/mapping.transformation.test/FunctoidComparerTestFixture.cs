using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mapping.transformation.test
{
    [TestClass]
    public class FunctoidComparerTestFixture
    {
        [TestMethod]
        public void InconsistentResult()
        {
            var json = File.ReadAllText($@"{ConfigurationManager.Home}\..\source\unit.test\mapping.transformation.test\mmcsb.json");
            var mapping = json.DeserializeFromJson<TransformDefinition>();
            mapping.FunctoidCollection.Select((x, i) => x.Index = i).ToList().ForEach(x => { });
            mapping.FunctoidCollection.ForEach(x => x.TransformDefinition = mapping);
            mapping.MapCollection.ForEach(x => x.TransformDefinition = mapping);

            var code = new StringBuilder();
            // functoids statement
            var sorted = new List<Functoid>(mapping.FunctoidCollection.OrderBy(x =>x));
            var functoidStatements = from f in sorted
                                     let statement = f.GenerateStatementCode()
                                     where !string.IsNullOrWhiteSpace(statement)
                                     && (!statement.Contains("Collection.") || (f.GetType() == typeof(LoopingFunctoid)))
                                     select $@"
    //{f.Name}:{f.GetType().Name}:{f.WebId}
    {statement}";
            code.AppendLine(string.Concat(functoidStatements.ToArray()));
            code.AppendLine();

        }

        [TestMethod]
        public void Sort()
        {

            var formatting = new FormattingFunctoid {WebId = "formatting", Name = "Formatting"};
            formatting.Initialize();
            formatting["value"].Functoid = "sql01";

            var sql01 = new SqlServerLookup { WebId = "sql01", SqlText = "SELECT MAX(Id) FROM dbo.Patient" };
            sql01.Initialize();
            sql01["connection"].Functoid = "conn";

            var sql02 = new SqlServerLookup { WebId = "sql02", SqlText = "SELECT MAX(Id) FROM dbo.Patient" };
            sql02.Initialize();
            sql02["connection"].Functoid = "conn";

            var config = new ConfigurationSettingFunctoid { WebId = "conn", Section = "ConnectionString", Key = "His" };

            var mapping = new TransformDefinition();
            mapping.AddFunctoids(formatting, sql01, sql02, config);
            mapping.FunctoidCollection.Select((x, i) => x.Index = i).ToList().ForEach(x => { });
            mapping.FunctoidCollection.ForEach(x => x.TransformDefinition = mapping);
            mapping.MapCollection.ForEach(x => x.TransformDefinition = mapping);
            
            // functoids statement
            var sorted = new List<Functoid>(mapping.FunctoidCollection)
                .OrderBy(x => x).ToList();
            //sorted.OrderBy(x => x.DependsOn());


            Assert.IsTrue(sorted.IndexOf(formatting) > sorted.IndexOf(sql01), "Formatting should come after sql01");
            Assert.IsTrue(sorted.IndexOf(sql01) > sorted.IndexOf(config), "sql01 should come after config");
            Assert.IsTrue(sorted.IndexOf(sql02) > sorted.IndexOf(config), "sql02 should come after config");

        }
    }
}