using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.RoslynScriptEngines;
using Bespoke.Sph.SqlReportDataSource;
using Bespoke.Sph.Domain;
using domain.test.triggers;
using NUnit.Framework;

namespace domain.test.reports
{
    [TestFixture]
    public class RdlTestFixture
    {
        private SqlDataSource m_sql;

        [SetUp]
        public void Init()
        {
            m_sql = new SqlDataSource();
            ObjectBuilder.AddCacheList<IReportDataSource>(m_sql);

            var roslyn = new RoslynScriptEngine();
            ObjectBuilder.AddCacheList<IScriptEngine>(roslyn);
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
        }

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

        [Test]
        public async Task GetColumnsValue()
        {
            var ds = new DataSource { EntityName = "Building" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };


            var building = new Designation
            {
                Name = "Test 1",
                //   Floors = 15,
                //   Address = new Address { State = "Kelantan" },
            };
            //building.CustomFieldValueCollection.Add(new CustomFieldValue { Name = "Custom01", Value = "XXX", Type = typeof(string).GetShortAssemblyQualifiedName() });
            var xml = (building).ToXElement();

            Console.WriteLine("Exec");
            var row = new ReportRow();


            var colums = await rdl.GetAvailableColumnsAsync();
            row.ReportColumnCollection.AddRange(colums);
            m_sql.FillColumnValue(xml, row);


            Console.WriteLine("Custom01: " + row["Custom01"].Value);
            Assert.AreEqual("XX", row["Custom01"].Value);
            Assert.AreEqual("Kelantan", row["Address.State"].Value);


        }

        [Test]
        public async Task ExecuteAggregateProperties()
        {
            const string path = "Tenant.Address.Street";
            var count = "Sph".GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Contract]");
            Assert.IsTrue(count > 0, "Fuck no contract");

            var contractId = "Sph".GetDatabaseScalarValue<int>("SELECT TOP 1 [ContractId] FROM [Sph].[Contract] ORDER BY NEWID()");
            var sql = "SELECT [Data] FROM [Sph].[Contract] WHERE [ContractId] = " + contractId;
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
    }

}
