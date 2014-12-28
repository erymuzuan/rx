using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
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
        public void EqualExpressionToStringLiteral()
        {
            var button = new Button { Enable = "item.Name == \"Kelantan\"" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name() === 'Kelantan'");
        }

        [TestMethod]
        public void EqualExpressionNotNullLiteral()
        {
            var button = new Button { Enable = "item.Name != null" };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button);
            StringAssert.Contains(html, "enable: item().Name() === null");
        }

        [TestMethod]
        public void FlipOverEqualExpressionToStringLiteral()
        {
            var button = new Button { Enable = " \"Kelantan\"==item.Name " };
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