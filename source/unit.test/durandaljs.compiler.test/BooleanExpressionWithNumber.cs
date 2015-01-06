using Bespoke.Sph.Domain;
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
                "$data.Age() > parseInt('25')",
                "item.Age > int.Parse(\"25\")"
                .CompileHtml());
        }


        [TestMethod]
        public void DecimalZero()
        {
            Assert.AreEqual(
                "$data.Age() > 0",
                "item.Age > decimal.Zero"
                .CompileHtml());
        }
        [TestMethod]
        public void DecimalOne()
        {
            Assert.AreEqual(
                "$data.Age() > 1",
                "item.Age > decimal.One"
                .CompileHtml());
        }

        [TestMethod]
        public void DecimalRound()
        {
            Assert.AreEqual(
                "$data.Age() > -1",
                "item.Age > decimal.Round(1.25m)"
                .CompileHtml());
        }
        [TestMethod]
        public void DecimalMinusOne()
        {
            Assert.AreEqual(
                "$data.Age() > 1",
                "item.Age > decimal.One"
                .CompileHtml());
        }

        [TestMethod]
        public void ParseInt32()
        {
            Assert.AreEqual(
                "$data.Age() > parseInt('25')",
                "item.Age > Int32.Parse(\"25\")"
                .CompileHtml());
        }

        [TestMethod]
        public void IntMax()
        {
            Assert.AreEqual(
                "$data.Age() > Infinity",
                "item.Age > int.MaxValue"
                .CompileHtml());
           
        }

        [TestMethod]
        public void IntMinValue()
        {
            Assert.AreEqual(
                "$data.Age() > -Infinity",
                "item.Age > int.MinValue"
                .CompileHtml());
           
        }
        

    }
}
