using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;

namespace domain.test.reports
{
    
    public class DataGridTestFixture
    {
        public DataGridTestFixture()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }

        [Fact]
        public void CsharpExpressionColumn()
        {
            var row = new ReportRow();
            row.ReportColumnCollection.Add(new ReportColumn
            {
                Name = "Name",
                Value = "Canyon",
                Type = typeof(decimal)
            });
            row.ReportColumnCollection.Add(new ReportColumn
            {
                Name = "Weight",
                Value = 6.3d,
                Type = typeof(double)
            });
            var column = new DataGridColumn
            {
                Expression = "=\"Berat \" + [Name] + \" - \" + [Weight] + \" KG\"",
                Format = "RM {0:F2}"
            };
            var html = column.GenerateHtml(row);


            Assert.Equal("Berat Canyon - 6.3 KG", html);
        }

        [Fact]
        public void FormatColumn()
        {
            var row = new ReportRow();
            row.ReportColumnCollection.Add(new ReportColumn
            {
                Name = "Amount",
                Value = 25.02001m,
                Type = typeof(decimal)
            });
            var column = new DataGridColumn
            {
                Expression = "[Amount]",
                Format = "RM {0:F2}"
            };
            var html = column.GenerateHtml(row);


            Assert.Equal("RM 25.02", html);
        }

        [Fact]
        public void ColumnValue()
        {
            var row = new ReportRow();
            row.ReportColumnCollection.Add(new ReportColumn
            {
                Name = "Amount",
                Value = 25.02001m,
                Type = typeof(decimal)
            });
            var column = new DataGridColumn
            {
                Expression = "[Amount]"
            };
            var html = column.GenerateHtml(row);


            Assert.Equal("25.02001", html);
        }


    }
}
