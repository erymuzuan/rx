using System;
using System.Globalization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class BooleanExpressionWithNumber
    {
        [TestInitialize]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }


        [TestMethod]
        public void ParseInt()
        {
            Assert.AreEqual(
                "$data.Dob() > parseInt('25')",
                "item.Age > int.Parse(\"25\")"
                .CompileHtml());
        }
        [TestMethod]
        public void ParseInt32()
        {
            Assert.AreEqual(
                "$data.Dob() > parseInt('25')",
                "item.Age > Int32.Parse(\"25\")"
                .CompileHtml());
        }

        [TestMethod]
        public void IntMax()
        {
            Assert.AreEqual(
                "$data.Age >Infinity",
                "item.Age > int.Max"
                .CompileHtml());
        }
        

    }
}
