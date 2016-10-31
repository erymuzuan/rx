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
            var source = new ConstantFunctoid
            {
                WebId = "source",
                Value = "SOURCE",
                Type = typeof(string)
            };

            var format1 = new FormattingFunctoid
            {
                WebId = "format1",
            };
            format1.Initialize();
            format1["value"].Functoid = source.WebId;

            var parseDouble = new ParseDoubleFunctoid
            {
                Name = "parseDouble",
                WebId = "parseDouble"
            };
            parseDouble.Initialize();
            parseDouble["value"].Functoid = format1.WebId;

            var map = new TransformDefinition();

            var list = new List<Functoid> {source, parseDouble, format1};
            map.FunctoidCollection.AddRange(list);
            list.Sort(new FunctoidDependencyComparer(map));
            var code = string.Join(",", list.Select(x => x.WebId));
            Assert.AreEqual("source,format1,parseDouble", code);

        }
    }
}
