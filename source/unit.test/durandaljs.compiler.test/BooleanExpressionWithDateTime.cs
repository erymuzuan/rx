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
                "$data.CreatedDate().moment() > moment('2005-10-01')",
                "item.CreatedDate > DateTime.Parse(\"2005-10-01\")"
                .CompileHtml());
        }

        [TestMethod]
        public void ParseExactDateTime()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment('05/06/2015', 'DD/MM/YYYY')",
                "item.CreatedDate > DateTime.ParseExact(\"05/06/2015\", \"dd/MM/yyyy\",System.Globalization.CultureInfo.CurrentCulture)"
                .CompileHtml());
            var d = DateTime.ParseExact("01/02/2014", "dd/MM/yyyy", CultureInfo.CurrentCulture);
            Console.WriteLine(d);
        }

    

        [TestMethod]
        public void ParseExactDateTimeWithWithFormatFromConfig()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment('05/06/2015', config.shortDateFormatString)",
                "item.CreatedDate > DateTime.ParseExact(\"05/06/2015\", config.ShortDateFormatString, System.Globalization.CultureInfo.CurrentCulture)"
                .CompileHtml());
        }

   
        [TestMethod]
        public void Now()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment()",
                "item.CreatedDate > DateTime.Now"
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
                "$data.CreatedDate().moment() > moment().startOf('day')",
                "item.CreatedDate > DateTime.Today"
                .CompileHtml());
        }
        [TestMethod]
        public void AddDaysWithDouble()
        {
            // TODO : extend javacscript string
            // String.prototype.add = function (unit, period){ return moment(this).add(unit ,period);}
            Assert.AreEqual(
                "$data.CreatedDate().moment().add(2, 'day') > moment().add(1, 'day')",
                "item.CreatedDate.AddDays(2) > DateTime.Now.AddDays(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddHoursWithDouble()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'h')",
                "item.CreatedDate > DateTime.Now.AddHours(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddMillisecondWithDouble()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() < moment().add(1, 'ms')",
                "item.CreatedDate < DateTime.Now.AddMilliseconds(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddMinute()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'm')",
                "item.CreatedDate > DateTime.Now.AddMinutes(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddMonth()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'M')",
                "item.CreatedDate > DateTime.Now.AddMonths(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddSeconds()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 's')",
                "item.CreatedDate > DateTime.Now.AddSeconds(1)"
                .CompileHtml());
        }
        [TestMethod]
        public void AddYears()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'y')",
                "item.CreatedDate > DateTime.Now.AddYears(1)"
                .CompileHtml());
        }

     



    }
}
