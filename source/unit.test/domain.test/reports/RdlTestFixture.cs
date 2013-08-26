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
