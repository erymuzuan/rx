using System;
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
                "item.Name == \"Pantani\"" .CompileHtml(),
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
            StringAssert.Contains(
                "string.IsNullOrWhiteSpace(item.Name)".CompileHtml(), 
                "enable: !$data.Name()");
        }
        [TestMethod]
        public void NotStringIsNullOrWhiteSpace()
        {

            StringAssert.Contains(
                "!string.IsNullOrWhiteSpace(item.Name)".CompileHtml(), 
                "enable: !!$data.Name()");
        }

        [TestMethod]
        public void StringIsNullOrEmpty()
        {
            StringAssert.Contains(
                "string.IsNullOrEmpty(item.Name)".CompileHtml(), 
                "enable: !$data.Name()");
        }


        [TestMethod]
        public void LiteralTrueString()
        {
            StringAssert.Contains(
                "true".CompileHtml(),
                "enable: true");
        }
        [TestMethod]
        public void LiteralFalseString()
        {
            StringAssert.Contains(
                "false".CompileHtml(),
                "enable: false");
        }

        [TestMethod]
        public void NotStringIsNullOrEmpty()
        {
            StringAssert.Contains(
                "!string.IsNullOrEmpty(item.Name)".CompileHtml(),
                "enable: !!$data.Name()");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void NotBooleanExpression()
        {
            "\"test\"".CompileHtml();
        }
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
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