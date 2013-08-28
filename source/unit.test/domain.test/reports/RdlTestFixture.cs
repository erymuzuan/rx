using System;
using Bespoke.Sph.SqlReportDataSource;
using Bespoke.SphCommercialSpaces.Domain;
using domain.test.triggers;
using NUnit.Framework;
using roslyn.scriptengine;

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
            ds.ParameterCollection.Add(new Parameter { Name = "Today", Value = 30.00m });
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            var script = ObjectBuilder.GetObject<IScriptEngine>();
            var result = script.Evaluate("@Today", rdl);
            Assert.AreEqual(30.00m, result);

        }
        [Test]
        public void GetParamInExpression()
        {
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            ds.ParameterCollection.Add(new Parameter { Name = "Price", Value = 30.00m });
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            var script = ObjectBuilder.GetObject<IScriptEngine>();
            var result = script.Evaluate("@Price", rdl);
            Assert.AreEqual(30.00m, result);

        }

        [Test]
        public void GetColumns()
        {
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            rdl.GetAvailableColumnsAsync()
                .ContinueWith(_ =>
                {
                    var result = _.Result;
                    foreach (var reportColumn in result)
                    {
                        Console.WriteLine(reportColumn.Name);
                    }
                })
            .Wait(5000)
            ;

        }

        [Test]
        public void GetColumnsValue()
        {
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };
            var xml = (new Building
            {
                Name = "Test 1",
                Floors = 15,
                Address = new Address { State = "Kelantan" }
            }).ToXElement();

            var row = new ReportRow();
            rdl.GetAvailableColumnsAsync()
                .ContinueWith(_ =>
                {
                    var colums = _.Result;
                    row.ReportColumnCollection.AddRange(colums);
                    m_sql.FillColumnValue(xml, row);
                    foreach (var c in colums)
                    {
                        Console.WriteLine("{0}\t= {1}", c.Name, c.Value);
                    }
                    Assert.AreEqual("Kelantan", row["Address.State"].Value);
                }).Wait(1000);

        }

        [Test]
        public void ExecuteGetRowsCountWithQuery()
        {
            var count = "Sph".GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Building]");
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            rdl.ExecuteResultAsync()
                .ContinueWith(_ =>
                {
                    var result = _.Result;
                    Assert.AreEqual(count, result.Count);
                    Console.WriteLine(result);
                })
            .Wait(5000)
            ;

        }

        [Test]
        public void ExecuteWithFilterParameterValue()
        {
            var count = "Sph".GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Land] WHERE [Location] = 'Bukit Bunga'");
            var ds = new DataSource { EntityName = "Land" };
            ds.ParameterCollection.Add(new Parameter { Name = "Location", Label = "Lokasi", Type = typeof(string), Value = "Bukit Bunga" });
            ds.ReportFilterCollection.Add(new ReportFilter { FieldName = "Location", Type = typeof(string), Operator = "Eq", Value = "@Location" });

            var rdl = new ReportDefinition { Title = "Test tanah", Description = "test", DataSource = ds };

            rdl.ExecuteResultAsync()
                .ContinueWith(_ =>
                {
                    var result = _.Result;

                    Assert.AreEqual(count, result.Count);
                    Console.WriteLine(result);
                })
            .Wait(5000)
            ;

        }

        [Test]
        public void ExecuteGetRowsList()
        {
            var name = "Sph".GetDatabaseScalarValue<string>("SELECT TOP 1 [Name] FROM [Sph].[Building]");
            var count = "Sph".GetDatabaseScalarValue<int>("SELECT COUNT([BuildingId]) FROM [Sph].[Building]");
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            rdl.ExecuteResultAsync()
                .ContinueWith(_ =>
                {
                    var result = _.Result;
                    Assert.AreEqual(count, result.Count);
                    Assert.AreEqual(name, result[0]["Name"].Value);
                    Console.WriteLine(result[0]);
                })
            .Wait(5000)
            ;

        }
    }

}
