using System;
using Bespoke.Sph.SqlReportDataSource;
using Bespoke.SphCommercialSpaces.Domain;
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
                    Assert.AreEqual("Kelantan",row["Address.State"].Value);
                }).Wait(1000);

        }

        [Test]
        public void ExecuteGetRowsCount()
        {
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            rdl.ExecuteResultAsync()
                .ContinueWith(_ =>
                {
                    var result = _.Result;
                    Assert.AreEqual(1, result.Count);
                    Console.WriteLine(result);
                })
            .Wait(5000)
            ;

        }

        [Test]
        public void ExecuteGetRowsList()
        {
            var ds = new DataSource { EntityName = "Building", Query = "SELECT * FROM [Sph].[Building]" };
            var rdl = new ReportDefinition { Title = "Test", Description = "test", DataSource = ds };

            rdl.ExecuteResultAsync()
                .ContinueWith(_ =>
                {
                    var result = _.Result;
                    Assert.AreEqual(1, result.Count);
                    Assert.AreEqual("Damansara Intan", result[0]["Name"]);
                    Console.WriteLine(result[0]);
                })
            .Wait(5000)
            ;

        }
    }

}
