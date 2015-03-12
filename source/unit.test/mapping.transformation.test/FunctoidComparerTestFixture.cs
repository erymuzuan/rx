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
            var json = File.ReadAllText(@"C:\project\work\sph\source\unit.test\mapping.transformation.test\mmcsb.json");
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
    }
}