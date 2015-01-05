using System;
using System.Globalization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class BooleanExpressionWithDateTime
    {
        [TestInitialize]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }


        [TestMethod]
        public void ParseDateTime()
        {
            Assert.AreEqual(
                "$data.Dob() > moment('2005-10-01')",
                "item.Dob > DateTime.Parse(\"2005-10-01\")"
                .CompileHtml());
        }

        [TestMethod]
        public void ParseExactDateTime()
        {
            Assert.AreEqual(
                "$data.Dob() > moment('2005-10-01', 'DD/MM/YYYY')",
                "item.Dob > DateTime.ParseExact(\"2005-10-01\", \"dd/MM/yyyy\",System.Globalization.CultureInfo.CurrentCulture)"
                .CompileHtml());
            var d = DateTime.ParseExact("1/2/2014", "dd/MM/yyyy", CultureInfo.CurrentCulture);
            Console.WriteLine(d);
        }


    }
}
