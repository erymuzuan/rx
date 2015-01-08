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
                "$data.Dob() > moment('05/06/2015', 'DD/MM/YYYY')",
                "item.Dob > DateTime.ParseExact(\"05/06/2015\", \"dd/MM/yyyy\",System.Globalization.CultureInfo.CurrentCulture)"
                .CompileHtml());
            var d = DateTime.ParseExact("01/02/2014", "dd/MM/yyyy", CultureInfo.CurrentCulture);
            Console.WriteLine(d);
        }

        [TestMethod]
        public void ParseExactDateTimeWithFormatValue()
        {
            Assert.AreEqual(
                "$data.Dob() > moment('05/06/2015', $data.Mrn())",
                "item.Dob > DateTime.ParseExact(\"05/06/2015\", item.Mrn, System.Globalization.CultureInfo.CurrentCulture)"
                .CompileHtml());
            var d = DateTime.ParseExact("01/02/2014", "dd/MM/yyyy", CultureInfo.CurrentCulture);
            Console.WriteLine(d);
        }

        [TestMethod]
        public void ParseExactDateTimeWithWithFormatFromConfig()
        {
            Assert.AreEqual(
                "$data.Dob() > moment('05/06/2015', config.shortDateFormatString)",
                "item.Dob > DateTime.ParseExact(\"05/06/2015\", config.ShortDateFormatString, System.Globalization.CultureInfo.CurrentCulture)"
                .CompileHtml());
        }

        [TestMethod]
        public void ParseExactDateTimeWithItem()
        {
            Assert.AreEqual(
                "$data.Dob() > moment($data.Name(), 'DD/MM/YYYY')",
                "item.Dob > DateTime.ParseExact(item.Name, \"dd/MM/yyyy\",System.Globalization.CultureInfo.CurrentCulture)"
                .CompileHtml());
            var d = DateTime.ParseExact("01/02/2014", "dd/MM/yyyy", CultureInfo.CurrentCulture);
            Console.WriteLine(d);
        }
        [TestMethod]
        public void Now()
        {
            Assert.AreEqual(
                "$data.Dob() > moment()",
                "item.Dob > DateTime.Now"
                .CompileHtml());
        }
        [TestMethod]
        public void DateTimeToString()
        {
            Assert.AreEqual(
                "$data.Name() === moment().format()",
                "item.Name == DateTime.Now.ToString()"
                .CompileHtml());
        }
        [TestMethod]
        public void Today()
        {
            Assert.AreEqual(
                "$data.Dob() > moment().startOf('day')",
                "item.Dob > DateTime.Today"
                .CompileHtml());
        }
     



    }
}
