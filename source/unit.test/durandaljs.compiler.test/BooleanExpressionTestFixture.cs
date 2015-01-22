using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using NUnit.Framework;


namespace durandaljs.compiler.test
{
    [TestFixture]
    public class BooleanExpressionTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }


        [Test]
        public void StringConcat()
        {
            Assert.AreEqual(
                "'test' + $data.Mrn() === $data.Name()",
                "\"test\" + item.Mrn == item.Name".CompileHtml());
        }
        [Test]
        public void ReturnString()
        {
            Assert.IsNull(
                "item.Mrn".CompileHtml(),
                "CS0029");
        }

        [Test]
        public void ReturnDateTime()
        {
            Assert.IsNull(
                "item.Dob".CompileHtml(),
                "CS0029");
        }
        [Test]
        public void InvalidCode()
        {
            var html = "item.Dobd && !k".CompileHtml();
            Assert.IsNull(html);
        }

        [Test]
        public void EqualExpressionToBoolean()
        {
            Assert.AreEqual(
                "item.IsMarried".CompileHtml(),
                "$data.IsMarried()");
        }

        [Test]
        public void ExclamationToFalse()
        {
            Assert.AreEqual(
                "!item.IsMarried".CompileHtml(),
                "!$data.IsMarried()");
        }

        [Test]
        public void EqualExpressionToStringLiteral()
        {
            Assert.AreEqual(
                "item.Name == \"Pantani\"".CompileHtml(),
                "$data.Name() === 'Pantani'");
        }
        [Test]
        public void OrExpression()
        {
            Assert.AreEqual(
                "$data.Name() === 'Pantani' || $data.IsMarried()",
                "item.Name == \"Pantani\" || item.IsMarried".CompileHtml());
        }
        [Test]
        public void AndExpression()
        {
            Assert.AreEqual(
                "$data.Name() === 'Pantani' && $data.IsMarried()",
                "item.Name == \"Pantani\" && item.IsMarried".CompileHtml());
        }
        [Test]
        public void CompoundAndOrExpression()
        {
            Assert.AreEqual(
                "($data.Name() === 'Zaki' || $data.IsMarried()) && $data.Age() < 25",
                "(item.Name == \"Zaki\" || item.IsMarried) && item.Age < 25".CompileHtml());
        }

        [Test]
        public void Aggregate()
        {
            Assert.AreEqual(
                "$data.Address().State() === 'Kelantan'",
                "item.Address.State == \"Kelantan\"".CompileHtml());
        }


        [Test]
        public void EqualExpressionNotNullLiteral()
        {

            Assert.AreEqual(
                "$data.Name() !== null",
                "item.Name != null".CompileHtml());
        }
        [Test]
        public void NotStringEmptyConstant()
        {

            Assert.AreEqual(
                "$data.Name() !== ''",
                "item.Name != string.Empty".CompileHtml());
        }
        [Test]
        public void StringIsNullOrWhiteSpace()
        {
            Assert.AreEqual(
                "String.isNullOrWhiteSpace($data.Name())",
                "string.IsNullOrWhiteSpace(item.Name)".CompileHtml());
        }
        [Test]
        public void NotStringIsNullOrWhiteSpace()
        {

            Assert.AreEqual(
                "!String.isNullOrWhiteSpace($data.Name())",
                "!string.IsNullOrWhiteSpace(item.Name)".CompileHtml());
        }

        [Test]
        public void StringIsNullOrEmpty()
        {
            Assert.AreEqual(
                "String.isNullOrEmpty($data.Name())",
                "string.IsNullOrEmpty(item.Name)".CompileHtml());
        }


        [Test]
        public void LiteralTrueString()
        {
            Assert.AreEqual(
                "true".CompileHtml(),
                "true");
        }
        [Test]
        public void LiteralFalseString()
        {
            Assert.AreEqual(
                "false".CompileHtml(),
                "false");
        }

        [Test]
        public void NotStringIsNullOrEmpty()
        {
            Assert.AreEqual(
                "!String.isNullOrEmpty($data.Name())",
                "!string.IsNullOrEmpty(item.Name)".CompileHtml(), "");
        }

        [Test]
        public void NotBooleanExpression()
        {
            "\"test\"".CompileHtml();
        }
        [Test]
        public void IntegerExpression()
        {
            "0".CompileHtml();
        }


        [Test]
        public void FlipOverEqualExpressionToStringLiteral()
        {
            Assert.AreEqual(
                " \"Pantani\" == item.Name ".CompileHtml(),
                "'Pantani' === $data.Name()");
        }

        [Test]
        public void NotEqualExpressionToStringLiteral()
        {
            Assert.AreEqual(
                "$data.Name() !== 'Pantani'",
                "item.Name != \"Pantani\"".CompileHtml(), "");
        }

        [Test]
        public void GreaterExpressionToStringLiteral()
        {
            Assert.AreEqual(
                "item.Age  > 25".CompileHtml(),
                "$data.Age() > 25");
        }

        [Test]
        public void GreaterOrEqualExpressionToStringLiteral()
        {
            Assert.AreEqual(
                "item.Age  >= 25".CompileHtml(),
                "$data.Age() >= 25");
        }
    }
}