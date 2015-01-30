using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using NUnit.Framework;


namespace durandaljs.compiler.test
{
    [TestFixture]
    public class SymbolWalkerTestFixture
    {
        [SetUp]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }
        [Test]
        [Ignore]
        public void LiteralStringToUpper()
        {
            Assert.AreEqual(
                "'whatever'.toUpper() === 'KELANTAN'",
                "\"whatever\".ToUpper() == \"KELANTAN\"".CompileHtml());
        }
        [Test]
        public void StringToUpper()
        {
            Assert.AreEqual(
                "$data.Address().State().toLocaleUpperCase() === 'KELANTAN'",
                "item.Address.State.ToUpper() == \"KELANTAN\"".CompileHtml());
        }

        [Test]
        public void StringToUpperInvariant()
        {
            Assert.AreEqual(
                "$data.Address().State().toUpperCase() === 'KELANTAN'",
                "item.Address.State.ToUpperInvariant() == \"KELANTAN\"".CompileHtml());
        }
        [Test]
        public void StringToLower()
        {
            Assert.AreEqual(
                "$data.Address().State().toLocaleLowerCase() === 'kelantan'",
                "item.Address.State.ToLower() == \"kelantan\"".CompileHtml());
        }

        [Test]
        public void StringToLowerInvariant()
        {
            Assert.AreEqual(
                "$data.Address().State().toLowerCase() === 'kelantan'",
                "item.Address.State.ToLowerInvariant() == \"kelantan\"".CompileHtml());
        }
        [Test]
        public void StringSubstringWithOneParameter()
        {
            Assert.AreEqual(
                "$data.Address().State().substring(3) === 'kel'",
                "item.Address.State.Substring(3) == \"kel\"".CompileHtml());
        }

        [Test]
        public void StringSubstringWithTwoParameters()
        {
            Assert.AreEqual(
                "$data.Address().State().substring(3,1) === 'kel'",
                "item.Address.State.Substring(3, 1) == \"kel\"".CompileHtml());
        }

        [Test]
        public void StringLength()
        {
            Assert.AreEqual(
                "$data.Address().State().length > 8",
                "item.Address.State.Length > 8".CompileHtml());
        }
        [Test]
        public void ItemMarried()
        {
            Assert.AreEqual(
                "$data.IsMarried()",
                "item.IsMarried".CompileHtml());
        }
        [Test]
        public void ItemUrban()
        {
            Assert.AreEqual(
                "$data.Address().Urban()",
                "item.Address.Urban".CompileHtml());
        }
        [Test]
        public void ItemCountryToUpper()
        {
            Assert.AreEqual(
                "$data.Address().Country().toUpperCase()",
                "item.Address.Country.ToUpperInvariant()".CompileExpression<string>());
        }
        [Test]
        public void ItemNameToUpper()
        {
            Assert.AreEqual(
                "$data.Name().toUpperCase()",
                "item.Name.ToUpperInvariant()".CompileExpression<string>());
        }
    }
}
