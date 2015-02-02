using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public partial class DateTimeExpressionTestFixture : ExpressionTestFixture
    {

        [Test]
        public async Task ParseDateTime()
        {
            await AssertAsync<DateTime>(
                "moment('2005-10-01')",
                "DateTime.Parse(\"2005-10-01\")"
                );
        }

        [Test]
        public async Task ParseExactDateTime()
        {
            await AssertAsync<DateTime>(
                "moment('05/06/2015', 'DD/MM/YYYY')",
                "DateTime.ParseExact(\"05/06/2015\", \"dd/MM/yyyy\",System.Globalization.CultureInfo.CurrentCulture)"
                );
        }



        [Test]
        public async Task ParseExactDateTimeWithWithFormatFromConfig()
        {
            await AssertAsync<DateTime>(
                "moment('05/06/2015', config.shortDateFormatString)",
                "DateTime.ParseExact(\"05/06/2015\", config.ShortDateFormatString, System.Globalization.CultureInfo.CurrentCulture)"
                );
        }


        [Test]
        public async Task Now()
        {
            await AssertAsync<DateTime>(
                "moment()",
                "DateTime.Now"
                );
        }

        [Test]
        public async Task DateTimeToString()
        {
            await AssertAsync<string>(
                "moment().format()",
                "DateTime.Now.ToString()"
                );
        }

        [Test]
        public async Task Today()
        {
            await AssertAsync<DateTime>(
                "moment().startOf('day')",
                "DateTime.Today"
                );
        }

        [Test]
        public async Task AddDaysWithDouble()
        {
            // TODO : extend javacscript string
            // String.prototype.add = function (unit, period){ return moment(this).add(unit ,period);}
            await AssertAsync<DateTime>(
                "moment().add(1, 'day')",
                "DateTime.Now.AddDays(1)"
                );
        }

        [Test]
        public async Task AddHoursWithDouble()
        {
            await AssertAsync<DateTime>(
                "moment().add(1, 'h')",
                "DateTime.Now.AddHours(1)"
                );
        }

        [Test]
        public async Task AddMillisecondWithDouble()
        {
            await AssertAsync<DateTime>(
                "moment().add(1, 'ms')",
                "DateTime.Now.AddMilliseconds(1)"
                );
        }

        [Test]
        public async Task AddMinute()
        {
            await AssertAsync<DateTime>(
                "moment().add(1, 'm')",
                "DateTime.Now.AddMinutes(1)"
                );
        }

        [Test]
        public async Task AddMonth()
        {
            await AssertAsync<DateTime>(
                "moment().add(1, 'M')",
                "DateTime.Now.AddMonths(1)"
                );
        }

        [Test]
        public async Task AddSeconds()
        {
            await AssertAsync<DateTime>(
                "moment().add(1, 's')",
                "DateTime.Now.AddSeconds(1)"
                );
        }

        [Test]
        public async Task AddYears()
        {
            await AssertAsync<DateTime>(
                "moment().add(1, 'y')",
                "DateTime.Now.AddYears(1)"
                );
        }
    }
}