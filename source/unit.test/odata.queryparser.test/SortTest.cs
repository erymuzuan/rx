using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Tests.SqlServer.Extensions;
using Moq;
using odata.queryparser;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.ODataQueryParserTests
{
    public class SortTest
    {
        public SortTest(ITestOutputHelper console)
        {
            Console = console;

            var cache = new Mock<ICacheManager>(MockBehavior.Strict);
            ObjectBuilder.AddCacheList<ISourceRepository>(SourceRepository);
            ObjectBuilder.AddCacheList(cache.Object);
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));

            var ed = CreateEmployeeEntityDefinition();
            SourceRepository.AddOrReplace(ed);
        }

        private ITestOutputHelper Console { get; }
        private MockSourceRepository SourceRepository { get; } = new MockSourceRepository();

        private static EntityDefinition CreateEmployeeEntityDefinition(string name = "Employee")
        {
            var ed = new EntityDefinition { Name = name, Plural = "Employees", RecordName = "No", Id = "Employee" };
            ed.AddSimpleMember<int>("No");
            ed.AddSimpleMember<string>("FirstName");
            ed.AddSimpleMember<string>("LastName");
            ed.AddSimpleMember<string>("Description");
            ed.AddSimpleMember<string>("Gender");
            ed.AddSimpleMember<int>("Age");
            ed.AddSimpleMember<DateTime>("DateOfBirth");
            ed.AddSimpleMember<DateTime>("HireDate");

            var address = new ComplexMember { Name = "HomeAddress", TypeName = "Address" };
            address.AddMember<string>("Street1");
            address.AddMember<string>("Street2");
            address.AddMember<string>("City");
            address.AddMember<string>("State");
            address.AddMember<string>("Country");
            address.AddMember<int>("Postcode");
            ed.MemberCollection.Add(address);

            var contacts = new ComplexMember { Name = "Contacts", TypeName = "Contact", AllowMultiple = true };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ed.MemberCollection.Add(contacts);

            return ed;
        }


        [Theory]
        [InlineData("$orderby=FirstName desc", "Employee", SortDirection.Desc, "FirstName")]
        [InlineData("$orderby=HomeAddress/State", "Employee", SortDirection.Asc, "State")]
        [InlineData("$orderby=HomeAddress/State,No", "Employee", SortDirection.Asc, "State", "No")]
        [InlineData("$orderby=HomeAddress/State,No asc", "Employee", SortDirection.Asc, "State", "No")]
        [InlineData("$orderby=HomeAddress/State desc,No desc", "Employee", SortDirection.Desc, "State", "No")]
        [InlineData("$orderby=DateOfBirth desc", "Employee", SortDirection.Desc, "DateOfBirth")]
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