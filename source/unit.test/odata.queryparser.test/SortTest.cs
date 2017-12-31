using System;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Tests.SqlServer.Extensions;
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
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));

            var product = new EntityDefinition { Name = "Student", Id = "student", Plural = "Students", WebId = Strings.GenerateId() };
            product.AddSimpleMember<string>("Mrn");
            product.AddSimpleMember<string>("FullName");
            product.AddSimpleMember<DateTime>("Dob");
            var address = product.AddMember<ComplexMember>("HomeAddress");
            address.AddMember("State", typeof(string));


            var git = new MockSourceRepository();
            git.AddOrReplace(product);
            ObjectBuilder.AddCacheList<ISourceRepository>(git);
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