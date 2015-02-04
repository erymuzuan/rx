using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class DateTimeExpressionTextFixure : ExpressionTestFixture
    {


        [Test]
        public async Task NillableDateTimeProperty()
        {
            await AssertAsync<DateTime?>(
                "$data.Dob().moment()",
                "item.Dob");

        }
        [Test]
        public async Task ParseExactDateTimeWithFormatValue()
        {
            await AssertAsync<DateTime>(
                "moment('05/06/2015', $data.Tag())",
                "DateTime.ParseExact(\"05/06/2015\", item.Tag, System.Globalization.CultureInfo.CurrentCulture)");

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
        public async Task ParseExactDateTimeWithItem()
        {
            await AssertAsync<DateTime>("moment($data.Name(), 'DD/MM/YYYY')",
                "DateTime.ParseExact(item.Name, \"dd/MM/yyyy\",System.Globalization.CultureInfo.CurrentCulture)");

        }

        [Test]
        public async Task InstanceDayPropertyOnNow()
        {
            await AssertAsync<int>(
                "moment().date()",
                "DateTime.Now.Day"
                );
        }


        [Test]
        public async Task InstanceYearPropertyOnNow()
        {
            await AssertAsync<int>(
                "moment().year()",
                "DateTime.Now.Year"
                );
            Console.WriteLine(DateTime.Today.Year);
        }
        [Test]
        public async Task InstanceYearPropertyOnItem()
        {
            await AssertAsync<int>(
                "$data.CreatedDate().moment().year()",
                "item.CreatedDate.Year"
                );
        }

        [Test]
        public async Task ToShortDateString()
        {
            await AssertAsync<string>(
                "$data.CreatedDate().moment().format('DD/MM/YYYY')",
                "item.CreatedDate.ToShortDateString()"
                );
            Console.WriteLine(DateTime.Today.ToShortDateString());
        }

        [Test]
        public async Task InstancePropertyOnItem()
        {
            await AssertAsync<DateTime>(
                "$data.CreatedDate().moment()",
                "item.CreatedDate"
                );
        }

        [Test]
        public async Task AggregateInstancePropertyOnItem()
        {
            await AssertAsync<DateTime>(
                "$data.Address().CreatedDate().moment()",
                "item.Address.CreatedDate"
                );
        }

        [Test]
        [Ignore]
        public async Task DateTimeToStringFormatd()
        {
            // TODO: need to translate .Net date format string to moment format string
            Process.Start("http://momentjs.com/docs/#/displaying/");
            await AssertAsync<string>(
                "moment().format('DD/MM/YYYY')",
                "DateTime.Now.ToString(\"d\")"
                );
        }
    }
}