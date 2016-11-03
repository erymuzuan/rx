using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mapping.transformation.test
{
    [TestClass]
    public class FunctoidDependenciesTestFixture
    {
        [TestMethod]
        public void CompareFormatAndParseDouble()
        {
            var map = new TransformDefinition();
            var source = new ConstantFunctoid
            {
                WebId = "source",
                Value = "SOURCE",
                Type = typeof(string),
                TransformDefinition = map
            };

            var format1 = new FormattingFunctoid
            {
                WebId = "format1",
                TransformDefinition = map
            };
            format1.Initialize();
            format1["value"].Functoid = source.WebId;

            var parseDouble = new ParseDoubleFunctoid
            {
                Name = "parseDouble",
                WebId = "parseDouble",
                TransformDefinition = map
            };
            parseDouble.Initialize();
            parseDouble["value"].Functoid = format1.WebId;
            
            var list = new List<Functoid> { source, parseDouble, format1 };
            map.FunctoidCollection.AddRange(list);

            var sorted = list.OrderBy(x => x);
            var code = string.Join(",", sorted.Select(x => x.WebId));
            Assert.AreEqual("source,format1,parseDouble", code);

        }
    }
}
