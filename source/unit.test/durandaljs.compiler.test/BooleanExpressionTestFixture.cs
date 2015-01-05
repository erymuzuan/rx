using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class BooleanExpressionTestFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }
        

        [TestMethod]
        public void ReturnString()
        {
            StringAssert.Contains(
                "item.Mrn".CompileHtml(),
                "CS0029");
        }

        [TestMethod]
        public void ReturnDateTime()
        {
            StringAssert.Contains(
                "item.Dob".CompileHtml(),
                "CS0029");
        }
        [TestMethod]
        public void InvalidCode()
        {
            var html = "item.Dobd && !k".CompileHtml();
            StringAssert.Contains(html, "CS1061");
            StringAssert.Contains(html, "CS0103");
        }

        [TestMethod]
        public void EqualExpressionToBoolean()
        {
            StringAssert.Contains(
                "item.IsMarried".CompileHtml(),
                "enable: $data.IsMarried");
        }

        [TestMethod]
        public void EqualExpressionToBooleanFalse()
        {
            StringAssert.Contains(
                "!item.IsMarried".CompileHtml(),
                "enable: !$data.IsMarried");
        }

        [TestMethod]
        public void EqualExpressionToStringLiteral()
        {
            StringAssert.Contains(
                "item.Name == \"Pantani\"".CompileHtml(),
                "enable: $data.Name() === 'Pantani'");
        }
        [TestMethod]
        public void OrExpression()
        {
            StringAssert.Contains(
                "item.Name == \"Pantani\" || item.IsMarried".CompileHtml(),
                "enable: $data.Name() === 'Pantani' || $data.IsMarried()");
        }
        [TestMethod]
        public void AndExpression()
        {
            StringAssert.Contains(
                "item.Name == \"Pantani\" && item.IsMarried".CompileHtml(),
                "enable: $data.Name() === 'Pantani' && $data.IsMarried()");
        }
        [TestMethod]
        public void CompoundAndEOrxpression()
        {
            StringAssert.Contains(
                "(item.Name == \"Zaki\" || item.IsMarried) && item.Age < 25".CompileHtml(),
                "enable: ($data.Name() === 'Zaki' || $data.IsMarried()) && $data.Age() < 25");
        }

        [TestMethod]
        public void EqualExpressionNotNullLiteral()
        {

            StringAssert.Contains(
                "item.Name != null".CompileHtml(),
                "enable: $data.Name() !== null");
        }
        [TestMethod]
        public void NotStringEmptyConstant()
        {

            StringAssert.Contains(
                "item.Name != string.Empty".CompileHtml(),
                "enable: $data.Name() !== ''");
        }
        [TestMethod]
        public void StringIsNullOrWhiteSpace()
        {
            Assert.AreEqual(
                "String.isNullOrWhiteSpace($data.Name())",
                "string.IsNullOrWhiteSpace(item.Name)".CompileHtml());
        }
        [TestMethod]
        public void NotStringIsNullOrWhiteSpace()
        {

            Assert.AreEqual(
                "!String.isNullOrWhiteSpace($data.Name())",
                "!string.IsNullOrWhiteSpace(item.Name)".CompileHtml());
        }

        [TestMethod]
        public void StringIsNullOrEmpty()
        {
            Assert.AreEqual(
                "String.isNullOrEmpty($data.Name())",
                "string.IsNullOrEmpty(item.Name)".CompileHtml());
        }


        [TestMethod]
        public void LiteralTrueString()
        {
            Assert.AreEqual(
                "true".CompileHtml(),
                "true");
        }
        [TestMethod]
        public void LiteralFalseString()
        {
            Assert.AreEqual(
                "false".CompileHtml(),
                "false");
        }

        [TestMethod]
        public void NotStringIsNullOrEmpty()
        {
            StringAssert.Contains(
                "!string.IsNullOrEmpty(item.Name)".CompileHtml(),
                "!String.isNullOrEmpty($data.Name())");
        }

        [TestMethod]
        public void NotBooleanExpression()
        {
            "\"test\"".CompileHtml();
        }
        [TestMethod]
        public void IntegerExpression()
        {
            "0".CompileHtml();
        }


        [TestMethod]
        public void FlipOverEqualExpressionToStringLiteral()
        {
            StringAssert.Contains(
                " \"Pantani\" == item.Name ".CompileHtml(),
                "enable: 'Pantani' === $data.Name()");
        }

        [TestMethod]
        public void NotEqualExpressionToStringLiteral()
        {
            StringAssert.Contains(
                "item.Name != \"Pantani\"".CompileHtml(),
                "enable: $data.Name() !== 'Pantani'");
        }

        [TestMethod]
        public void GreaterExpressionToStringLiteral()
        {
            StringAssert.Contains(
                "item.Age  > 25".CompileHtml(),
                "enable: $data.Age() > 25");
        }

        [TestMethod]
        public void GreaterOrEqualExpressionToStringLiteral()
        {
            StringAssert.Contains(
                "item.Age  >= 25".CompileHtml(),
                "enable: $data.Age() >= 25");
        }
    }
}