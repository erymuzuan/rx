using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using sqlrepository.test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.SqlServer
{
    [Trait("Category", "Sql Server")]
    [Collection(SqlServerCollection.SQLSERVER_COLLECTION)]
    public class PatientReadOnlyRepositoryTest
    {
        public SqlServerFixture Fixture { get; }
        public PatientReadOnlyRepositoryTest(SqlServerFixture fixture, ITestOutputHelper console)
        {
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
            Fixture = fixture;
        }

        [Fact]
        public async Task PredicateWithUnaryNot()
        {
            Expression<Func<Patient, bool>> expression = x => !x.IsMalaysian;
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            var max = this.Fixture.Patients.Count(expression.Compile());
            Assert.Equal(db, max);
        }

        [Fact]
        public async Task PredicateWithUnary()
        {
            Expression<Func<Patient, bool>> expression = x => x.IsMalaysian && x.Gender == "Female";
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            var max = this.Fixture.Patients.Count(expression.Compile());
            Assert.Equal(db, max);
        }
        [Fact]
        public async Task PredicateWithUnaryNotAnd()
        {
            Expression<Func<Patient, bool>> expression = x => !x.IsMalaysian && x.Gender == "Female";
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            var max = this.Fixture.Patients.Count(expression.Compile());
            Assert.Equal(db, max);
        }


        [Fact]
        public async Task PredicateWithConvertExpression()
        {
            Expression<Func<Patient, bool>> expression = x => x.Dob < new DateTime(1960, 1, 1);
            var db = await this.Fixture.Repository.GetMaxAsync(expression, x => x.Age);
            var max = this.Fixture.Patients.Where(expression.Compile()).Max(x => x.Age);
            Assert.Equal(db, max);
        }

        [Fact]
        public async Task PredicateOrWithConstantString()
        {
            Expression<Func<Patient, bool>> expression = x => x.Gender == "Female" || x.Race == "Chinese";
            var db = await this.Fixture.Repository.GetMaxAsync(expression, x => x.Age);
            var max = this.Fixture.Patients.Where(expression.Compile()).Max(x => x.Age);
            Assert.Equal(db, max);
        }

        [Fact]
        public async Task PredicateWithExpressionPropery()
        {
            Expression<Func<Patient, bool>> expression = x => x.Dob.HasValue;
            var db = await this.Fixture.Repository.GetMaxAsync(expression, x => x.Age);
            var max = this.Fixture.Patients.Where(expression.Compile()).Max(x => x.Age);
            Assert.Equal(db, max);
        }
        [Fact]
        public async Task PredicateWithUnaryExpressionPropery()
        {
            Expression<Func<Patient, bool>> expression = x => !x.Dob.HasValue;
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            var expected = this.Fixture.Patients.Count(expression.Compile());
            Assert.Equal(expected,db);
        }
        [Fact]
        public async Task PredicateWithExpressionPropery2()
        {
            Expression<Func<Patient, bool>> expression = x => x.Dob.Value.Year == 1960;
            var expected = this.Fixture.Patients.Count(expression.Compile());
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            Assert.Equal(expected, db);
        }

        [Fact]
        public async Task PredicateOrWithInt32Constant()
        {
            Expression<Func<Patient, bool>> expression = x => x.Age >= 60 || x.Age < 10;
            var db = await this.Fixture.Repository.GetMaxAsync(expression, x => x.Age);
            var max = this.Fixture.Patients.Where(expression.Compile()).Max(x => x.Age);
            Assert.Equal(db, max);
        }


        [Fact]
        public async Task PredicateWithChildMemberAccess()
        {
            Expression<Func<Patient, bool>> expression = x => x.Wife.Name == null;
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            var max = this.Fixture.Patients.Count(expression.Compile());
            Assert.Equal(db, max);


        }

        [Fact]
        public async Task PredicateWithGrandChildMemberAccess()
        {
            Expression<Func<Patient, bool>> expression = x => x.Wife.WorkPlaceAddress.State == "Kelantan";
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            var max = this.Fixture.Patients.Count(expression.Compile());
            Assert.Equal(db, max);


        }

        [Fact]
        public async Task PredicateEqNull()
        {
            Expression<Func<Patient, bool>> expression2 = x => x.FullName == null;

            var db = await this.Fixture.Repository.GetCountAsync(expression2);
            var max = this.Fixture.Patients.Count(expression2.Compile());
            Assert.Equal(db, max);

        }


        [Fact]
        public async Task PredicateEqNotNull()
        {
            Expression<Func<Patient, bool>> expression = x => x.Wife.Name != null;
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            var max = this.Fixture.Patients.Count(expression.Compile());
            Assert.Equal(db, max);
        }


        //TODO : IsNullOr
        [Fact]
        public async Task PredicateStringIsNullOrWhitespace()
        {
            Expression<Func<Patient, bool>> expression = x => string.IsNullOrWhiteSpace(x.Wife.Name);
            var db = await this.Fixture.Repository.GetCountAsync(expression);
            var max = this.Fixture.Patients.Count(expression.Compile());
            Assert.Equal(db, max);
        }

        [Fact]
        public async Task PredicateEqStringConstant()
        {
            Expression<Func<Patient, bool>> expression = x => x.Gender == "Female";
            var db = await this.Fixture.Repository.GetMaxAsync(expression, x => x.Age);
            var max = this.Fixture.Patients.Where(expression.Compile()).Max(x => x.Age);
            Assert.Equal(db, max);
        }


        [Fact]
        public void Address()
        {
            var schema = this.Fixture.PatientSchema;
            var address = schema.AddMember<ComplexMember>("OfficeAddress");
            Assert.IsType<ComplexMember>(address);
        }

        [Fact]
        public void Age()
        {
            var age = this.Fixture.PatientSchema.MemberCollection.OfType<SimpleMember>()
                .SingleOrDefault(x => x.Name == "Age");
            Assert.NotNull(age);
            Assert.Equal(typeof(int), age.Type);
        }

        [Fact]
        public async Task ExecuteSimpleFilterQueryAsync()
        {
            var female = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }));
            Assert.Equal(33, female.TotalRows);
            var male = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Neq, "Female")
            }));
            Assert.Equal(67, male.TotalRows);
        }


        [Fact]
        public async Task ExecuteSimpleFilterIterateRowsQueryAsync()
        {
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            });
            var female = await Fixture.Repository.SearchAsync(query);
            Assert.Equal(33, female.TotalRows);
            var male = await Fixture.Repository.SearchAsync(new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Neq, "Female")
            }));
            Assert.Equal(67, male.TotalRows);
            Assert.Equal(query.Size, male.ItemCollection.Count);
        }

        [Fact]
        public async Task ExecuteSimpleFilterWithPageQueryAsync()
        {
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, skip: 30);
            var female = await Fixture.Repository.SearchAsync(query);
            Assert.Equal(33, female.TotalRows);
            Assert.Equal(3, female.ItemCollection.Count);
        }


        [Fact]
        public async Task ExecuteQueryWithFieldsAsync()
        {
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            var lo = await this.Fixture.Repository.SearchAsync(query);

            var reader = lo.Readers.OrderBy(x => Guid.NewGuid()).First();
            Assert.True(reader.ContainsKey("FullName"));
            Assert.True(reader.ContainsKey("Dob"));
            Assert.True(reader.ContainsKey("Age"));
            Assert.True(reader.ContainsKey("Id"));
            Assert.False(reader.ContainsKey("Religion"));

            Assert.IsType<int>(reader["Age"]);
            Assert.IsType<DateTime>(reader["Dob"]);
            Assert.Null(reader["Wife.Name"]);

            Assert.Equal(33, lo.TotalRows);
        }

        [Fact]
        public async Task MaxAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            query.Aggregates.Add(new MaxAggregate("LastModifiedDate", "ChangedDate"));
            var lo = await repos.SearchAsync(query);

            var max = lo.GetAggregateValue<DateTime>("LastModifiedDate");
            Assert.Equal("2001-05-26T00:00:00", max.ToString("s"));
        }


        [Fact]
        public async Task MinAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            query.Aggregates.Add(new MinAggregate("LastModifiedDate", "ChangedDate"));
            var lo = await repos.SearchAsync(query);

            var max = lo.GetAggregateValue<DateTime>("LastModifiedDate");
            Assert.Equal("1952-03-04T00:00:00", max.ToString("s"));
        }

        [Fact]
        public async Task AverageAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            query.Aggregates.Add(new AverageAggregate("AverageAge", "Age"));
            var lo = await repos.SearchAsync(query);

            var max = lo.GetAggregateValue<int>("AverageAge");
            Assert.Equal(67, max);
        }

        [Fact]
        public async Task SumAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });
            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");
            query.Aggregates.Add(new SumAggregate("TotalAge", "Age"));
            var lo = await repos.SearchAsync(query);

            var max = lo.GetAggregateValue<int>("TotalAge");
            Assert.Equal(2231, max);
        }

        [Fact]
        public async Task CountDistinctAggregate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl(sorts: new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });

            query.Aggregates.Add(new CountDistinctAggregate("States", "HomeAddress.State"));
            var lo = await repos.SearchAsync(query);

            var max = lo.GetAggregateValue<int>("States");
            Assert.Equal(13, max);
        }

        [Fact]
        public async Task CountDistinctWithPredicate()
        {
            var repos = Fixture.Repository;
            var query = new QueryDsl(new[]
            {
                new Filter("Gender", Operator.Eq, "Female")
            }, new[] { new Sort { Direction = SortDirection.Desc, Path = "Mrn" } });


            query.Fields.AddRange("FullName", "Age", "Dob", "Wife.Name");

            query.Aggregates.Add(new CountDistinctAggregate("States", "HomeAddress.State"));
            var lo = await repos.SearchAsync(query);

            var max = lo.GetAggregateValue<int>("States");
            Assert.Equal(8, max);
        }
    }
}