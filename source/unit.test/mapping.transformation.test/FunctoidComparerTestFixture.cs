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
            var sorted = new List<Functoid>(mapping.FunctoidCollection);
            sorted.Sort(new FunctoidDependencyComparer());
            var functoidStatements = from f in sorted
                                     let statement = f.GenerateStatementCode()
                                     where !string.IsNullOrWhiteSpace(statement)
                                     && (!statement.Contains("Collection.") || (f.GetType() == typeof(LoopingFunctoid)))
                                     select string.Format("\r\n{4}//{0}:{1}:{2}\r\n{4}{3}", f.Name, f.GetType().Name, f.WebId, statement, " ");
            code.AppendLine(string.Concat(functoidStatements.ToArray()));
            code.AppendLine();
            StringAssert.Contains(code.ToString(), "hrmis_pdrmk");
        }

        [TestMethod]
        public void Sort()
        {
            var sql = new SqlServerLookup { WebId = "A", SqlText = "SELECT MAX(Id) FROM dbo.Patient" };
            sql.Initialize();
            sql["connection"].Functoid = "B";

            var config = new ConfigurationSettingFunctoid { WebId = "B", Section = "ConnectionString", Key = "His"};

            var mapping = new TransformDefinition();
            mapping.AddFunctoids(sql, config);
            mapping.FunctoidCollection.Select((x, i) => x.Index = i).ToList().ForEach(x => { });
            mapping.FunctoidCollection.ForEach(x => x.TransformDefinition = mapping);
            mapping.MapCollection.ForEach(x => x.TransformDefinition = mapping);
           
            Assert.AreEqual(sql, mapping.FunctoidCollection[0]);
            Assert.AreEqual(config, mapping.FunctoidCollection[1]);

            // functoids statement
            var sorted = new List<Functoid>(mapping.FunctoidCollection);
            sorted.Sort(new FunctoidDependencyComparer());

            Assert.AreEqual(config, sorted[0]);
            Assert.AreEqual(sql, sorted[1]);

        }
    }
}