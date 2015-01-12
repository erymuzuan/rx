using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using NUnit.Framework;


namespace durandaljs.compiler.test
{
    [TestFixture]
    public class BooleanExpressionWithNumber
    {
        [SetUp]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }


        [Test]
        public void ParseInt()
        {
            Assert.AreEqual(
                "$data.Age() > parseInt('25')",
                "item.Age > int.Parse(\"25\")"
                .CompileHtml());
        }


        [Test]
        public void DecimalZero()
        {
            Assert.AreEqual(
                "$data.Age() > 0",
                "item.Age > decimal.Zero"
                .CompileHtml());
        }
        [Test]
        public void DecimalOne()
        {
            Assert.AreEqual(
                "$data.Age() > 1",
                "item.Age > decimal.One"
                .CompileHtml());
        }

        [Test]
        public void DecimalRound()
        {
            var d = decimal.Round(1.24879889798897898795m);
            Assert.AreEqual(1m,d);
            Assert.AreEqual(
                "$data.Age() > 1.24879889798897898795.toFixed(0)",
                "item.Age > decimal.Round(1.24879889798897898795m)"
                .CompileHtml());
        }

        [Test]
        public void DecimalRound2()
        {
            var d = decimal.Round(1.24879889798897898795m, 2);
            Assert.AreEqual(1.25m,d);
            Assert.AreEqual(
                "$data.Age() > 1.24879889798897898795.toFixed(2)",
                "item.Age > decimal.Round(1.24879889798897898795m, 2)"
                .CompileHtml());
        }
        [Test]
        public void Add()
        {
            Assert.AreEqual(
                "1 + 2 > 15",
                "1 + 2 > 15"
                .CompileHtml());
        }
        [Test]
        public void Add2()
        {
            Assert.AreEqual(
                "$data.Age() > 1 + 2",
                "item.Age > 1 + 2"
                .CompileHtml());
        }


        [Test]
        public void DecimalMinusOne()
        {
            Assert.AreEqual(
                "$data.Age() > 1",
                "item.Age > decimal.One"
                .CompileHtml());
        }

        [Test]
        public void ParseInt32()
        {
            Assert.AreEqual(
                "$data.Age() > parseInt('25')",
                "item.Age > Int32.Parse(\"25\")"
                .CompileHtml());
        }

        [Test]
        public void IntMax()
        {
            Assert.AreEqual(
                "$data.Age() > Infinity",
                "item.Age > int.MaxValue"
                .CompileHtml());
           
        }

        [Test]
        public void IntMinValue()
        {
            Assert.AreEqual(
                "$data.Age() > -Infinity",
                "item.Age > int.MinValue"
                .CompileHtml());
           
        }
        

    }
}
