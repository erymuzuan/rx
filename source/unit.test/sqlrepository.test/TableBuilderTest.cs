using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using Bespoke.Sph.Tests.SqlServer.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    public class MockSqlServerMetadata : ISqlServerMetadata
    {
        public Table GetTable(string name)
        {
            return new Table
            {
                Name = name,
                Columns = new[]
            {
                new Column{Name = "Id",IsNullable = false,CanRead = true, CanWrite = true,SqlType = "VARCHAR(255)"},
                new Column{Name = "Name",IsNullable = false,CanRead = true, CanWrite = true,SqlType = "VARCHAR(255)"},
                new Column{Name = "JSONN",IsNullable = false,CanRead = true, CanWrite = true,SqlType = "VARCHAR(MAX)"},
            }
            };
        }
    }

    public class TableBuilderTest
    {
        public ITestOutputHelper Console { get; }

        public TableBuilderTest(ITestOutputHelper console)
        {
            Console = console;
            ObjectBuilder.AddCacheList<ISqlServerMetadata>(new MockSqlServerMetadata());
        }
        [Fact]
        public void GenerateColumn()
        {
            var ent = new EntityDefinition { Name = "CustomerAccount", Plural = "Customeraccounts", Id = "customer-account", RecordName = "Name" };
            ent.AddMember("Name", typeof(string), true);
            ent.AddMember("No", typeof(string), true);
            ent.AddMember("RegistratioNo", typeof(string), true);
            ent.AddMember("CreditLimit", typeof(decimal), true);

            var address = new ComplexMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.AddMember("Street1", typeof(string), false);
            address.AddMember("Postcode", typeof(string), true);
            address.AddMember("State", typeof(string), true);
            ent.MemberCollection.Add(address);


            var sql = new TableSchemaBuilder();
            var columns = sql.GetFilterableMembers("", ent.MemberCollection).ToList();

            Assert.All(columns, Assert.NotNull);
            Assert.Contains(columns, c => c.FullName == "Address.State");
        }

        [Fact]
        public async Task GenerateCreateTableSql()
        {
            var file = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\CustomerAccount.sql";
            if (File.Exists(file))
                File.Delete(file);
            var ent = new EntityDefinition { Name = "CustomerAccount", Plural = "CustomerAccounts", Id = "customer-account", RecordName = "Name" };
            ent.AddMember("Name", typeof(string), true);
            ent.AddMember("No", typeof(string), true);
            ent.AddMember("RegistratioNo", typeof(string), true);
            ent.AddMember("CreditLimit", typeof(decimal), true);

            var address = new ComplexMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.AddMember("Street1", typeof(string), false);
            address.AddMember("Postcode", typeof(string), true);
            address.AddMember("State", typeof(string), true);
            ent.MemberCollection.Add(address);


            var builder = new TableSchemaBuilder();
            await builder.BuildAsync(ent, null);

            Assert.True(File.Exists(file));
            var sql = File.ReadAllText(file);

            Assert.Contains("[Address.State]", sql);

            File.Delete(file);

        }
    }
}
