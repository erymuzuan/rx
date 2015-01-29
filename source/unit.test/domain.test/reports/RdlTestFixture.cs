using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.SqlReportDataSource;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.reports
{
    [TestFixture]
    public class RdlTestFixture
    {
        private SqlDataSource m_sql;
        private MockRepository<EntityDefinition> m_efMock;


        [Test]
        public void GetParamInExpressionConflict()
        {
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            ds.ParameterCollection.Add(new Parameter { Name = "Volume", Value = 30.00m, Type = typeof(decimal) });
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            var script = ObjectBuilder.GetObject<IScriptEngine>();
            var result = script.Evaluate<decimal, Entity>("@Volume", rdl);
            Assert.AreEqual(30.00m, result);

        }
        [Test]
        public void GetParamInExpression()
        {
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            ds.ParameterCollection.Add(new Parameter { Name = "Price", Value = 30.00m, Type = typeof(decimal) });
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            var script = ObjectBuilder.GetObject<IScriptEngine>();
            var result = script.Evaluate<decimal, Entity>("@Price", rdl);
            Assert.AreEqual(30.00m, result);

        }

        [Test]
        public async Task GetColumns()
        {
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            var result = await rdl.GetAvailableColumnsAsync();
            foreach (var reportColumn in result)
            {
                Console.WriteLine(reportColumn.Name);
            }


        }

        [SetUp]
        public void Init()
        {
            m_efMock = new MockRepository<EntityDefinition>();
            m_sql = new SqlDataSource();
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(m_efMock);
            ObjectBuilder.AddCacheList<IReportDataSource>(m_sql);
        }
        [Test]
        public async Task GetColumnsValue()
        {
            var vd = File.ReadAllText(Path.Combine(ConfigurationManager.SphSourceDirectory, "EntityDefinition/Customer.json"));
            var ed = vd.DeserializeFromJson<EntityDefinition>();
            var type = CompileCustomerDefinition(ed);

            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());

            var ds = new DataSource { EntityName = "Customer" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };


            dynamic customer = Activator.CreateInstance(type);
            customer.FullName = "erymuzuan";
            customer.Age = 39;
            customer.Address.State = "Kelantan";
            var json = JsonSerializerService.ToJsonString(customer);
            Console.WriteLine(json);

            Console.WriteLine("Exec");
            var row = new ReportRow();


            var colums = await rdl.GetAvailableColumnsAsync();
            row.ReportColumnCollection.AddRange(colums);
            m_sql.FillColumnValue(json, row);

            
            Console.WriteLine("FullName: " + row["FullName"].Value);
            Assert.AreEqual("erymuzuan", row["FullName"].Value);
            Assert.AreEqual("Kelantan", row["Address.State"].Value);
            Assert.AreEqual(39, row["Age"].Value);


        }

        [Test]
        public async Task ExecuteAggregateProperties()
        {
            const string path = "Tenant.Address.Street";
            var count = "Sph".GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Contract]");
            Assert.IsTrue(count > 0, "Fuck no contract");

            var contractId = "Sph".GetDatabaseScalarValue<int>("SELECT TOP 1 [ContractId] FROM [Sph].[Contract] ORDER BY NEWID()");
            var sql = "SELECT [Json] FROM [Sph].[Contract] WHERE [ContractId] = " + contractId;
            var xml = XElement.Parse("Sph".GetDatabaseScalarValue<string>(sql));

            var tenantStreet = xml.GetAttributeStringValue("Tenant", "Address", "Street");

            var ds = new DataSource { EntityName = "Contract", Query = sql };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            var rows = await rdl.ExecuteResultAsync();

            Assert.AreEqual(count, rows.Count);
            Assert.IsTrue(rows[0].ReportColumnCollection.Any(c => c.Name == path));
            var street = rows[0][path];
            Assert.AreEqual(tenantStreet, street.Value as string);


        }

        [Test]
        public async Task ExecuteGetRowsCountWithQuery()
        {
            var count = "Sph".GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Building]");
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };
            var result = await rdl.ExecuteResultAsync();
            Assert.AreEqual(count, result.Count);
            Console.WriteLine(result);
        }

        [Test]
        public async Task ExecuteWithFilterParameterValue()
        {
            var count = "Sph".GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Land] WHERE [Location] = 'Bukit Bunga'");
            var ds = new DataSource { EntityName = "Land" };
            ds.ParameterCollection.Add(new Parameter { Name = "Location", Label = "Lokasi", Type = typeof(string), Value = "Bukit Bunga" });
            ds.ReportFilterCollection.Add(new ReportFilter { FieldName = "Location", Type = typeof(string), Operator = "Eq", Value = "@Location" });

            var rdl = new ReportDefinition { Title = "Test tanah", Description = "test", DataSource = ds };

            var result = await rdl.ExecuteResultAsync();

            Assert.AreEqual(count, result.Count);
            Console.WriteLine(result);


        }

        [Test]
        public async Task ExecuteGetRowsList()
        {
            var name = "Sph".GetDatabaseScalarValue<string>("SELECT TOP 1 [Name] FROM [Sph].[Building]");
            var count = "Sph".GetDatabaseScalarValue<int>("SELECT COUNT([BuildingId]) FROM [Sph].[Building]");
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            var result = await rdl.ExecuteResultAsync();
            Assert.AreEqual(count, result.Count);
            Assert.AreEqual(name, result[0]["Name"].Value);
            Console.WriteLine(result[0]);
        }


        private Type CompileCustomerDefinition(EntityDefinition ed)
        {
            var options = new CompilerOptions
            {
                IsVerbose = false,
                IsDebug = true
            };


            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\core.sph\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));

            var result = ed.Compile(options);
            result.Errors.ForEach(Console.WriteLine);

            var assembly = Assembly.LoadFrom(result.Output);
            var type = assembly.GetType("Bespoke.Dev_customer.Domain.Customer");
            return type;
        }

    }

}
