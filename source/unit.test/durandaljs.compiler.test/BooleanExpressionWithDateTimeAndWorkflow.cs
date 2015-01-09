using System;
using System.Globalization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class BooleanExpressionWithDateTimeAndWorkflow
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
        [TestMethod]
        public void AddDaysWithDouble()
        {
            Assert.AreEqual(
                "$data.Dob() > moment().add(1, 'day')",
                "item.Dob > DateTime.Now.AddDays(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddHoursWithDouble()
        {
            Assert.AreEqual(
                "$data.Dob() > moment().add(1, 'h')",
                "item.Dob > DateTime.Now.AddHours(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddMillisecondWithDouble()
        {
            Assert.AreEqual(
                "$data.Dob() < moment().add(1, 'ms')",
                "item.Dob < DateTime.Now.AddMilliseconds(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddMinute()
        {
            Assert.AreEqual(
                "$data.Dob() > moment().add(1, 'm')",
                "item.Dob > DateTime.Now.AddMinutes(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddMonth()
        {
            Assert.AreEqual(
                "$data.Dob() > moment().add(1, 'M')",
                "item.Dob > DateTime.Now.AddMonths(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddSeconds()
        {
            Assert.AreEqual(
                "$data.Dob() > moment().add(1, 's')",
                "item.Dob > DateTime.Now.AddSeconds(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddYears()
        {
            Assert.AreEqual(
                "$data.Dob() > moment().add(1, 'y')",
                "item.Dob > DateTime.Now.AddYears(1)"
                .CompileHtml());
        }

     



    }
}
