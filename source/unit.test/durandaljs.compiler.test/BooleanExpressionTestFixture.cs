using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    static class HtmlCompileHelper
    {

        public static string CompileHtml(this string enableExpression, string visibleExpression = "true")
        {
            var button = new Button { Enable = enableExpression, Visible = visibleExpression };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            return html;
        }
    }
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
            var button = new Button { Enable = "item.IsMarried" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().IsMarried");
        }
        [TestMethod]
        public void EqualExpressionToBooleanFalse()
        {
            var button = new Button { Enable = "!item.IsMarried" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: !item().IsMarried");
        }

        [TestMethod]
        public void EqualExpressionToStringLiteral()
        {
            var button = new Button { Enable = "item.Name == \"Kelantan\"" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name() === 'Kelantan'");
        }
        [TestMethod]
        public void OrExpression()
        {
            var button = new Button { Enable = "item.Name == \"Kelantan\" || item.IsMarried" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name() === 'Kelantan' || item().IsMarried()");
        }
        [TestMethod]
        public void AndExpression()
        {
            var button = new Button { Enable = "item.Name == \"Kelantan\" && item.IsMarried" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name() === 'Kelantan' && item().IsMarried()");
        }
        [TestMethod]
        public void CompoundAndEOrxpression()
        {
            var button = new Button { Enable = "(item.Name == \"Zaki\" || item.IsMarried) && item.Age < 25" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: (item().Name() === 'Zaki' || item().IsMarried()) && item().Age() < 25");
        }

        [TestMethod]
        public void EqualExpressionNotNullLiteral()
        {
            var button = new Button { Enable = "item.Name != null" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name() !== null");
        }
        [TestMethod]
        public void NotStringEmptyConstant()
        {
            var button = new Button { Enable = "item.Name != string.Empty" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name() !== ''");
        }
        [TestMethod]
        public void StringIsNullOrWhiteSpace()
        {
            var button = new Button { Enable = "string.IsNullOrWhiteSpace(item.Name)" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: !item().Name()");
        }
        [TestMethod]
        public void NotStringIsNullOrWhiteSpace()
        {
            var button = new Button { Enable = "!string.IsNullOrWhiteSpace(item.Name)" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name()");
        }

        [TestMethod]
        public void StringIsNullOrEmpty()
        {
            var button = new Button { Enable = "string.IsNullOrEmpty(item.Name)" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: !item().Name()");
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
                "enable: item().Name()");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void NotBooleanExpression()
        {
            StringAssert.Contains(
                "\"test\"".CompileHtml(), 
                "whatever","this should not be valid");
        }


        [TestMethod]
        public void FlipOverEqualExpressionToStringLiteral()
        {
            var button = new Button { Enable = " \"Kelantan\" == item.Name " };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: 'Kelantan' === item().Name()");
        }
        [TestMethod]
        public void NotEqualExpressionToStringLiteral()
        {
            var button = new Button { Enable = "item.Name != \"Kelantan\"" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name() !== 'Kelantan'");
        }

        [TestMethod]
        public void GreaterExpressionToStringLiteral()
        {
            var button = new Button { Enable = "item.Age  > 25" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Age() > 25");
        }

        [TestMethod]
        public void GreaterOrEqualExpressionToStringLiteral()
        {
            var button = new Button { Enable = "item.Age  >= 25" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Age() >= 25");
        }
    }
}