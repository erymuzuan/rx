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
                "$data.Address().State().toLocaleUpperCase() === 'KELANTAN'",
                "item.Address.State.ToUpper() == \"KELANTAN\"".CompileHtml());
        }

        [TestMethod]
        public void StringToUpperInvariant()
        {
            Assert.AreEqual(
                "$data.Address().State().toUpperCase() === 'KELANTAN'",
                "item.Address.State.ToUpperInvariant() == \"KELANTAN\"".CompileHtml());
        }
        [TestMethod]
        public void StringToLower()
        {
            Assert.AreEqual(
                "$data.Address().State().toLocaleLowerCase() === 'kelantan'",
                "item.Address.State.ToLower() == \"kelantan\"".CompileHtml());
        }

        [TestMethod]
        public void StringToLowerInvariant()
        {
            Assert.AreEqual(
                "$data.Address().State().toLowerCase() === 'kelantan'",
                "item.Address.State.ToLowerInvariant() == \"kelantan\"".CompileHtml());
        }
        [TestMethod]
        public void StringSubstringWithOneParameter()
        {
            Assert.AreEqual(
                "$data.Address().State().substring(3) === 'kel'",
                "item.Address.State.Substring(3) == \"kel\"".CompileHtml());
        }

        [TestMethod]
        public void StringSubstringWithTwoParameters()
        {
            Assert.AreEqual(
                "$data.Address().State().substring(3,1) === 'kel'",
                "item.Address.State.Substring(3, 1) == \"kel\"".CompileHtml());
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
