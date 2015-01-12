using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class DateTimeExpressionTextFixure : ExpressionTestFixture
    {


        [TestMethod]
        public async Task NIllableDateTimeProperty()
        {
            await AssertAsync<DateTime?>(
                "$data.Dob().moment()",
                "item.Dob");

        }
        [TestMethod]
        public async Task ParseExactDateTimeWithFormatValue()
        {
            await AssertAsync<DateTime>(
                "moment('05/06/2015', $data.Tag())",
                "DateTime.ParseExact(\"05/06/2015\", item.Tag, System.Globalization.CultureInfo.CurrentCulture)");

        }

        [TestMethod]
        public async Task ParseExactDateTimeWithWithFormatFromConfig()
        {
            await AssertAsync<DateTime>(
                "moment('05/06/2015', config.shortDateFormatString)",
                "DateTime.ParseExact(\"05/06/2015\", config.ShortDateFormatString, System.Globalization.CultureInfo.CurrentCulture)"
                );
        }

        [TestMethod]
        public async Task ParseExactDateTimeWithItem()
        {
            await AssertAsync<DateTime>("moment($data.Name(), 'DD/MM/YYYY')",
                "DateTime.ParseExact(item.Name, \"dd/MM/yyyy\",System.Globalization.CultureInfo.CurrentCulture)");

        }

        [TestMethod]
        public async Task DateTimePropertyOnItem()
        {
            await AssertAsync<DateTime>(
                "$data.CreatedDate().moment()",
                "item.CreatedDate"
                );
        }

        [TestMethod]
        public async Task DateTimePropertyOnItem2()
        {
            await AssertAsync<DateTime>(
                "$data.Address().CreatedDate().moment()",
                "item.Address.CreatedDate"
                );
        }

        [TestMethod]
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