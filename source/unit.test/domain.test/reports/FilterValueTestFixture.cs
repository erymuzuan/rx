using System;
using Bespoke.Sph.RoslynScriptEngines;
using Bespoke.Sph.SqlReportDataSource;
using Bespoke.Sph.Domain;
using domain.test.triggers;
using Xunit;

namespace domain.test.reports
{
    
    public class FilterValueTestFixture
    {
        private SqlDataSource m_sql;

        public FilterValueTestFixture()
        {
            m_sql = new SqlDataSource();
            ObjectBuilder.AddCacheList<IReportDataSource>(m_sql);

            var roslyn = new RoslynScriptEngine();
            ObjectBuilder.AddCacheList<IScriptEngine>(roslyn);
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
        }


        [Fact]
        public void GetEqFilterOperator()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter { FieldName = "Location", Operator = "Eq", Value = "@Location" };
            var op = compiler.GetFilterOperator(filter);
            Assert.Equal("=", op);


        }
        [Fact]
        public void GetSubstringOfFilterOperator()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter { FieldName = "Location", Operator = "Substringof", Value = "@Location" };
            var op = compiler.GetFilterOperator(filter);
            Assert.Equal("LIKE '%' + {0} + '%'", op);
        }

        [Fact]
        public void GetGeFilterOperator()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter
            {
                FieldName = "Size",
                Operator = "Ge",
                Value = "@Size"
            };
            var op = compiler.GetFilterOperator(filter);
            Assert.Equal(">=", op);
        }



        [Fact]
        public void GetFilterValueFromParameter()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter
            {
                FieldName = "Location",
                Type = typeof(string),
                Operator = "Eq",
                Value = "@Location"
            };
            var op = compiler.GetFilterValue(filter);
            Assert.Equal("@Location", op);

        }

        [Fact]
        public void GetFilterValueFromConstantString()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter
            {
                FieldName = "Location",
                Operator = "Eq",
                Type = typeof(string),
                Value = "Kelantan"
            };
            var op = compiler.GetFilterValue(filter);
            Assert.Equal("'Kelantan'", op);

        }

        [Fact]
        public void GetFilterValueFromConstantBoolean()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter { FieldName = "IsActive", Type = typeof(bool), Operator = "Eq", Value = "false" };
            var val = compiler.GetFilterValue(filter);
            Assert.Equal("0", val);

        }

        [Fact]
        public void GetFilterValueFromConstantDateTime()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter
            {
                FieldName = "Location",
                Type = typeof(DateTime),
                Operator = "Eq",
                Value = "2012-01-01"
            };
            var op = compiler.GetFilterValue(filter);
            Assert.Equal("'2012-01-01'", op);

        }

        [Fact]
        public void GetFilterValueFromExpressionDateTime()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter
            {
                FieldName = "Location",
                Type = typeof(DateTime),
                Operator = "Eq",
                Value = "=DateTime.Today"
            };
            var op = compiler.GetFilterValue(filter);
            Assert.Equal(string.Format("'{0:s}'", DateTime.Today), op);

        }

        [Fact]
        public void GetFilterValueFromExpressionBoolean()
        {

            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter
            {
                FieldName = "IsActive",
                Type = typeof(bool),
                Operator = "Eq",
                Value = "=DateTime.Today.DayOfWeek == DayOfWeek.Friday"
            };
            var op = compiler.GetFilterValue(filter);
            Assert.Equal(DateTime.Today.DayOfWeek == DayOfWeek.Friday ? "1" : "0", op);

        }

        [Fact]
        public void GetFilterValueFromExpressionString()
        {

            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter
            {
                FieldName = "Location",
                Type = typeof(string),
                Operator = "Eq",
                Value = "=@Today.DayOfWeek +  \"-\" + @UserName"
            };
            var op = compiler.GetFilterValue(filter);
            Assert.Equal(string.Format("'{0}'", DateTime.Today.DayOfWeek + "-" + new MockLdap().CurrentUserName), op);

        }
    }
}