using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using NUnit.Framework;


namespace durandaljs.compiler.test
{
    [TestFixture]
    public class BooleanExpressionWithDateTimeAndWorkflow
    {
        [SetUp]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }


        [Test]
        public void ParseDateTime()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment('2005-10-01')",
                "item.CreatedDate> DateTime.Parse(\"2005-10-01\")"
                .CompileHtml());
        }

        [Test]
        public void ParseExactDateTime()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment('05/06/2015', 'DD/MM/YYYY')",
                "item.CreatedDate> DateTime.ParseExact(\"05/06/2015\", \"dd/MM/yyyy\",System.Globalization.CultureInfo.CurrentCulture)"
                .CompileHtml());

        }


        [Test]
        public void Now()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment()",
                "item.CreatedDate> DateTime.Now"
                .CompileHtml());
        }
        [Test]
        public void DateTimeToString()
        {
            Assert.AreEqual(
                "$data.Name() === moment().format()",
                "item.Name == DateTime.Now.ToString()"
                .CompileHtml());
        }
        [Test]
        public void Today()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().startOf('day')",
                "item.CreatedDate> DateTime.Today"
                .CompileHtml());
        }
        [Test]
        public void AddDaysWithDouble()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'day')",
                "item.CreatedDate> DateTime.Now.AddDays(1)"
                .CompileHtml());
        }
        [Test]
        public void AddHoursWithDouble()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'h')",
                "item.CreatedDate> DateTime.Now.AddHours(1)"
                .CompileHtml());
        }
        [Test]
        public void AddMillisecondWithDouble()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() < moment().add(1, 'ms')",
                "item.CreatedDate< DateTime.Now.AddMilliseconds(1)"
                .CompileHtml());
        }
        [Test]
        public void AddMinute()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'm')",
                "item.CreatedDate> DateTime.Now.AddMinutes(1)"
                .CompileHtml());
        }
        [Test]
        public void AddMonth()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'M')",
                "item.CreatedDate> DateTime.Now.AddMonths(1)"
                .CompileHtml());
        }
        [Test]
        public void AddSeconds()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 's')",
                "item.CreatedDate> DateTime.Now.AddSeconds(1)"
                .CompileHtml());
        }
        [Test]
        public void AddYears()
        {
            Assert.AreEqual(
                "$data.CreatedDate().moment() > moment().add(1, 'y')",
                "item.CreatedDate> DateTime.Now.AddYears(1)"
                .CompileHtml());
        }

     



    }
}
