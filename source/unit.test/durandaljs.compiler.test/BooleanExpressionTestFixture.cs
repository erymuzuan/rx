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
            Assert.IsNull(
                "item.Mrn".CompileHtml(),
                "CS0029");
        }

        [TestMethod]
        public void ReturnDateTime()
        {
            Assert.IsNull(
                "item.Dob".CompileHtml(),
                "CS0029");
        }
        [TestMethod]
        public void InvalidCode()
        {
            var html = "item.Dobd && !k".CompileHtml();
            Assert.IsNull(html);
        }

        [TestMethod]
        public void EqualExpressionToBoolean()
        {
            Assert.AreEqual(
                "item.IsMarried".CompileHtml(),
                "$data.IsMarried()");
        }

        [TestMethod]
        public void ExclamationToFalse()
        {
            Assert.AreEqual(
                "!item.IsMarried".CompileHtml(),
                "!$data.IsMarried()");
        }

        [TestMethod]
        public void EqualExpressionToStringLiteral()
        {
            Assert.AreEqual(
                "item.Name == \"Pantani\"".CompileHtml(),
                "$data.Name() === 'Pantani'");
        }
        [TestMethod]
        public void OrExpression()
        {
            Assert.AreEqual(
                "$data.Name() === 'Pantani' || $data.IsMarried()",
                "item.Name == \"Pantani\" || item.IsMarried".CompileHtml());
        }
        [TestMethod]
        public void AndExpression()
        {
            Assert.AreEqual(
                "$data.Name() === 'Pantani' && $data.IsMarried()",
                "item.Name == \"Pantani\" && item.IsMarried".CompileHtml());
        }
        [TestMethod]
        public void CompoundAndEOrxpression()
        {
            Assert.AreEqual(
                "($data.Name() === 'Zaki' || $data.IsMarried()) && $data.Age() < 25",
                "(item.Name == \"Zaki\" || item.IsMarried) && item.Age < 25".CompileHtml());
        }

        [TestMethod]
        public void EqualExpressionNotNullLiteral()
        {

            Assert.AreEqual(
                "$data.Name() !== null",
                "item.Name != null".CompileHtml());
        }
        [TestMethod]
        public void NotStringEmptyConstant()
        {

            Assert.AreEqual(
                "$data.Name() !== ''",
                "item.Name != string.Empty".CompileHtml());
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
            Assert.AreEqual(
                "!String.isNullOrEmpty($data.Name())",
                "!string.IsNullOrEmpty(item.Name)".CompileHtml(),"");
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
            Assert.AreEqual(
                " \"Pantani\" == item.Name ".CompileHtml(),
                "'Pantani' === $data.Name()");
        }

        [TestMethod]
        public void NotEqualExpressionToStringLiteral()
        {
            Assert.AreEqual(
                "$data.Name() !== 'Pantani'",
                "item.Name != \"Pantani\"".CompileHtml(),"");
        }

        [TestMethod]
        public void GreaterExpressionToStringLiteral()
        {
            Assert.AreEqual(
                "item.Age  > 25".CompileHtml(),
                "$data.Age() > 25");
        }

        [TestMethod]
        public void GreaterOrEqualExpressionToStringLiteral()
        {
            Assert.AreEqual(
                "item.Age  >= 25".CompileHtml(),
                "$data.Age() >= 25");
        }
    }
}