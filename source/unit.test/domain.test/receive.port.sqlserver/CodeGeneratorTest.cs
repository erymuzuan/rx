using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.receive.port.sqlserver
{
    public class CodeGeneratorTest
    {
        public ITestOutputHelper Console { get; }

        public CodeGeneratorTest(ITestOutputHelper helper)
        {
            Environment.SetEnvironmentVariable($"RX_{ConfigurationManager.ApplicationName}_HOME", @"c:\temp\rx", EnvironmentVariableTarget.Process);
            Console = helper;
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
        }

        [Fact]
        public async Task RecordClass()
        {
            var port = await GenerateReceivePort();
            var classes = (await port.GenerateCodeAsync()).ToArray();
            var department = classes.SingleOrDefault(x => x.Name == "Department");
            Assert.NotNull(department);
            Assert.Equal(4, department.PropertyCollection.Count);
            Console.WriteLine(department.GetCode());

        }

        [Fact]
        public async Task PortClass()
        {
            var port = await GenerateReceivePort();
            var classes = (await port.GenerateCodeAsync()).ToArray();
            var portType = classes.SingleOrDefault(x => x.Name == port.Name.ToPascalCase());
            Assert.NotNull(portType);
            Console.WriteLine(portType.GetCode());

        }
        [Fact]
        public async Task CompilePort()
        {
            var port = await GenerateReceivePort();
            port.ReferencedAssemblyCollection.Add(new ReferencedAssembly {Location = typeof(SqlConnection).Assembly.Location });

            var cr = await port.CompileAsync();
            foreach (var eror in cr.Errors)
            {
                Console.WriteLine(eror.ToString());
            }
            Assert.True(cr.Result, cr.ToString());
            Console.WriteLine(cr.Output);

        }

        private static async Task<ReceivePort> GenerateReceivePort()
        {
            var sql = new SqlServerRecordSetFormatter
            {
                Name = "HISDepartment",
                SampleStoreId = "text-csv-with-escape.txt",
                Server = "(localdb)\\ProjectsV13",
                Database = "His",
                Trusted = true,
                Query = "SELECT TOP 1 * FROM [Department]"

            };
            var port = new ReceivePort
            {
                Name = "His Department Sample",
                Entity = "Department",
                Id = "his-department-sample",
                Formatter = nameof(SqlServerRecordSetFormatter),
                TextFormatter = sql
            };


            var fields = await sql.GetFieldMappingsAsync();
            port.FieldMappingCollection.ClearAndAddRange(fields);

            Assert.Equal(4, fields.Length);

            var departmentNameField = fields[1];
            Assert.Equal("A&E", departmentNameField.SampleValue);
            departmentNameField.Name = "Name";
            departmentNameField.Type = typeof(string);


            return port;
        }

    }
}