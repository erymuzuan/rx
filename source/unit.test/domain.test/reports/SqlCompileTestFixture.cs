using System;
using Bespoke.Sph.SqlReportDataSource;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;
using roslyn.scriptengine;

namespace domain.test.reports
{
    [TestFixture]
    public class SqlCompileTestFixture
    {
        private SqlDataSource m_sql;
        [SetUp]
        public void Init()
        {
            m_sql = new SqlDataSource();
            ObjectBuilder.AddCacheList<IReportDataSource>(m_sql);

            var roslyn = new RoslynScriptEngine();
            ObjectBuilder.AddCacheList<IScriptEngine>(roslyn);
        }

        [Test]
        public void CompileWithFilter()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var location = new EntityField { Name = "Location" };
            var id = new EntityField { Name = "LandId" };

            var filter = new ReportFilter { FieldName = "Location", Type = typeof(string), Operator = "Eq", Value = "Bukit Bunga" };

            var source = new DataSource { EntityName = "Land" };
            source.EntityFieldCollection.Add(location);
            source.EntityFieldCollection.Add(id);
            source.ReportFilterCollection.Add(filter);

            var sql = compiler.Compile(source);
            Assert.AreEqual("SELECT [LandId], [Data]  FROM [Sph].[Land] WHERE [Location] = 'Bukit Bunga'", sql);
        }

        [Test]
        public void CompileWithSubstringofFilter()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var location = new EntityField { Name = "Location" };
            var id = new EntityField { Name = "LandId" };

            var filter = new ReportFilter { FieldName = "Location", Type = typeof(string), Operator = "Substringof", Value = "Bukit Bunga" };

            var source = new DataSource { EntityName = "Land" };
            source.EntityFieldCollection.Add(location);
            source.EntityFieldCollection.Add(id);
            source.ReportFilterCollection.Add(filter);

            var sql = compiler.Compile(source);
            Assert.AreEqual("SELECT [LandId], [Data]  FROM [Sph].[Land] WHERE [Location] LIKE '%' + 'Bukit Bunga' + '%'", sql);
        }


        [Test]
        public void CompileWithSubstringofFilterAndParameter()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var location = new EntityField { Name = "Location" };
            var id = new EntityField { Name = "LandId" };

            var filter = new ReportFilter
            {
                FieldName = "Location",
                Type = typeof(string),
                Operator = "Substringof",
                Value = "@Location"
            };

            var source = new DataSource { EntityName = "Land" };
            source.EntityFieldCollection.Add(location);
            source.EntityFieldCollection.Add(id);
            source.ReportFilterCollection.Add(filter);

            var sql = compiler.Compile(source);
            Assert.AreEqual("SELECT [LandId], [Data]  FROM [Sph].[Land] WHERE [Location] LIKE '%' + @Location + '%'", sql);
        }

        [Test]
        public void CompileWithSubstringofFilterAndExpression()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var location = new EntityField { Name = "Location" };
            var id = new EntityField { Name = "LandId" };

            var filter = new ReportFilter
            {
                FieldName = "Location",
                Type = typeof(string),
                Operator = "Substringof",
                Value = "=DateTime.Today.ToString(\"s\")"
            };

            var source = new DataSource { EntityName = "Land" };
            source.EntityFieldCollection.Add(location);
            source.EntityFieldCollection.Add(id);
            source.ReportFilterCollection.Add(filter);

            var sql = compiler.Compile(source);
            Assert.AreEqual("SELECT [LandId], [Data]  FROM [Sph].[Land] WHERE [Location] LIKE '%' + '" + DateTime.Today.ToString("s") +"' + '%'", sql);
        }


        [Test]
        public void CompileWithOrderAndFilter()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var location = new EntityField { Name = "Location", Order = "DESC", OrderPosition = 1 };
            var id = new EntityField { Name = "LandId", Order = "DESC", OrderPosition = 2 };

            var filter = new ReportFilter { FieldName = "Location", Type = typeof(string), Operator = "Eq", Value = "Bukit Bunga" };

            var source = new DataSource { EntityName = "Land" };
            source.EntityFieldCollection.Add(location);
            source.EntityFieldCollection.Add(id);
            source.ReportFilterCollection.Add(filter);

            var sql = compiler.Compile(source);
            Assert.AreEqual("SELECT [LandId], [Data]  FROM [Sph].[Land] WHERE [Location] = 'Bukit Bunga' ORDER BY [Location] DESC, [LandId] DESC", sql);
        }

        [Test]
        public void CompileGroup()
        {

            var location = new EntityField { Name = "Location", Aggregate = "GROUP" };
            var id = new EntityField { Name = "LandId", Aggregate = "COUNT" };
            var compiler = new SqlCompiler(new ReportDefinition());

            var source = new DataSource { EntityName = "Land" };
            source.EntityFieldCollection.Add(location);
            source.EntityFieldCollection.Add(id);

            var sql = compiler.Compile(source);
            Assert.AreEqual("SELECT COUNT([LandId]) AS LandId_COUNT, [Location] FROM [Sph].[Land]  GROUP BY [Location]", sql);
        }

        [Test]
        public void Compile2ColumsGroup()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var location = new EntityField { Name = "Location", Aggregate = "GROUP" };
            var lot = new EntityField { Name = "LotNo", Aggregate = "GROUP" };
            var id = new EntityField { Name = "LandId", Aggregate = "COUNT" };
            var source = new DataSource { EntityName = "Land" };
            source.EntityFieldCollection.Add(location);
            source.EntityFieldCollection.Add(lot);
            source.EntityFieldCollection.Add(id);

            var sql = compiler.Compile(source);
            Assert.AreEqual("SELECT COUNT([LandId]) AS LandId_COUNT, [Location], [LotNo] FROM [Sph].[Land]  GROUP BY [Location], [LotNo]", sql);
        }


    }

}
