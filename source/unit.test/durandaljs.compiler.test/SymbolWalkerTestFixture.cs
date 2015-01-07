using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class SymbolWalkerTestFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }
        [TestMethod]
        [Ignore]
        public void LiteralStringToUpper()
        {
            Assert.AreEqual(
                "'whatever'.toUpper() === 'KELANTAN'",
                "\"whatever\".ToUpper() == \"KELANTAN\"".CompileHtml());
        }
        [TestMethod]
        public void StringToUpper()
        {
            Assert.AreEqual(
                "$data.Address().State().toUpper() === 'KELANTAN'",
                "item.Address.State.ToUpper() == \"KELANTAN\"".CompileHtml());
        }
        [TestMethod]
        public void StringLength()
        {
            Assert.AreEqual(
                "$data.Address().State().length > 8",
                "item.Address.State.Length > 8".CompileHtml());
        }
    }
}
