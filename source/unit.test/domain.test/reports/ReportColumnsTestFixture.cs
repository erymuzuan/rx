using System;
using Bespoke.Sph.SqlReportDataSource;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.reports
{
    [TestFixture]
    public class ReportColumnsTestFixture
    {
        [Test]
        public void GetColumns()
        {
            var columns = new ObjectCollection<ReportColumn>();
            var ds = new SqlDataSource();
            ds.GetColumns(columns, typeof(Contract));
            columns.ForEach(x=>Console.WriteLine(x.Name));
        }
    }
}
