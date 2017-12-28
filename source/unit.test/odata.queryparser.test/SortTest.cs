using System.Linq;
using Bespoke.Sph.Domain;
using odata.queryparser;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.ODataQueryParserTests
{
    public class SortTest
    {
        private ITestOutputHelper Console { get; }

        public SortTest(ITestOutputHelper console)
        {
            Console = console;
        }


        [Theory]
        [InlineData("$orderby=FullName desc", "Student", SortDirection.Desc, "FullName")]
        [InlineData("$orderby=HomeAddress/State", "Student", SortDirection.Asc, "HomeAddress.State")]
        [InlineData("$orderby=HomeAddress/State,Mrn", "Student", SortDirection.Asc, "HomeAddress.State", "Mrn")]
        [InlineData("$orderby=HomeAddress/State,Mrn asc", "Student", SortDirection.Asc, "HomeAddress.State", "Mrn")]
        [InlineData("$orderby=HomeAddress/State,Mrn desc", "Student", SortDirection.Desc, "HomeAddress.State", "Mrn")]
        [InlineData("$orderby=Dob desc", "Student", SortDirection.Desc, "Dob")]
        public void ParseOrderBy(string odata
            , string entity
            , SortDirection expecteSortDirection /*TODO : each field gets its own sort*/
            , params string[] fields)
        {
            var parser = new OdataQueryParser();
            var query = parser.Parse(odata, entity);

            foreach (var field in fields)
            {
                Console.WriteLine($"Expecting {field} {expecteSortDirection}");
                var sort = query.Sorts.SingleOrDefault(x => x.Path == field);
                Assert.NotNull(sort);
                Assert.Equal(expecteSortDirection, sort.Direction);
            }
        }
    }
}