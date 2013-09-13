using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;
using roslyn.scriptengine;

namespace domain.test.reports
{
    [TestFixture]
    public class DataGridTestFixture
    {
        [SetUp]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }

        [Test]
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


            Assert.AreEqual("Berat Canyon - 6.3 KG", html);
        }

        [Test]
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


            Assert.AreEqual("RM 25.02", html);
        }

        [Test]
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


            Assert.AreEqual("25.02001", html);
        }


    }
}
