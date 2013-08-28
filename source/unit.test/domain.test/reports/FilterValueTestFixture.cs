using System;
using Bespoke.Sph.SqlReportDataSource;
using Bespoke.SphCommercialSpaces.Domain;
using domain.test.triggers;
using NUnit.Framework;
using roslyn.scriptengine;

namespace domain.test.reports
{
    [TestFixture]
    public class FilterValueTestFixture
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
        public void GetEqFilterOperator()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter { FieldName = "Location", Operator = "Eq", Value = "@Location" };
            var op = compiler.GetFilterOperator(filter);
            Assert.AreEqual("=", op);


        }
        [Test]
        public void GetSubstringOfFilterOperator()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter { FieldName = "Location", Operator = "Substringof", Value = "@Location" };
            var op = compiler.GetFilterOperator(filter);
            Assert.AreEqual("LIKE '%{0}%'", op);
        }

        [Test]
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
            Assert.AreEqual(">=", op);
        }



        [Test]
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
            Assert.AreEqual("@Location", op);

        }

        [Test]
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
            Assert.AreEqual("'Kelantan'", op);

        }

        [Test]
        public void GetFilterValueFromConstantBoolean()
        {
            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter { FieldName = "IsActive", Type = typeof(bool), Operator = "Eq", Value = "false" };
            var val = compiler.GetFilterValue(filter);
            Assert.AreEqual("0", val);

        }

        [Test]
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
            Assert.AreEqual("'2012-01-01'", op);

        }

        [Test]
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
            Assert.AreEqual(string.Format("'{0:s}'", DateTime.Today), op);

        }

        [Test]
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
            Assert.AreEqual(DateTime.Today.DayOfWeek == DayOfWeek.Friday ? "1" : "0", op);

        }

        [Test]
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
            Assert.AreEqual(string.Format("'{0}'", DateTime.Today.DayOfWeek + "-" + new MockLdap().CurrentUserName), op);

        }
        [Test]
        public void GetFilterValueFromExpressionSum()
        {

            var compiler = new SqlCompiler(new ReportDefinition());
            var filter = new ReportFilter
            {
                FieldName = "Location",
                Type = typeof(int),
                Operator = "Eq",
                Value = "=SUM(new []{1,2,5})"
            };
            var op = compiler.GetFilterValue(filter);
            Assert.AreEqual("8", op);

        }
    }
}