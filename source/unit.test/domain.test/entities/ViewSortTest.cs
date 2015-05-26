using System;
using Bespoke.Sph.Domain;
using domain.test.triggers;
using NUnit.Framework;

namespace domain.test.entities
{
    public class ViewSortTest
    {
        [Test]
        public void SimpleSort()
        {
            var ed = new EntityDefinition { Name = "Building", Plural = "Buildings", RecordName = "Name" };
            ed.MemberCollection.Add(new Member { Name = "Name", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "Floors", Type = typeof(int) });


            var view = new EntityView { Name = "All buildings", Route = "all-buildings" };
            view.SortCollection.Add(new Sort { Direction = SortDirection.Asc, Path = "Name" });
            view.SortCollection.Add(new Sort { Direction = SortDirection.Desc, Path = "Floors" });

            var sortDsl = view.GenerateEsSortDsl();
            StringAssert.Contains("{\"Name\":{\"order\":\"asc\"}}", sortDsl);
        }

        [Test]
        public void UsingCurrentUser()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            var ed = new EntityDefinition { Name = "Building", Plural = "Buildings", RecordName = "Name" };
            ed.MemberCollection.Add(new Member { Name = "Name", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "Floors", Type = typeof(int) });
            ed.MemberCollection.Add(new Member { Name = "Age", Type = typeof(int) });
            ed.MemberCollection.Add(new Member { Name = "CreatedBy", Type = typeof(string) });


            var view = new EntityView { Name = "All buildings", Route = "all-buildings" };
            view.SortCollection.Add(new Sort { Direction = SortDirection.Asc, Path = "Name" });
            view.SortCollection.Add(new Sort { Direction = SortDirection.Desc, Path = "Floors" });

            view.AddFilter("Name", Operator.Eq, new ConstantField { Type = typeof(string), Value = "KLCC" });
            view.AddFilter("Floors", Operator.Neq, new ConstantField { Type = typeof(int), Value = 0 });
            view.AddFilter("CreatedBy", Operator.Eq, new JavascriptExpressionField { Expression = "config.userName" });

            var filter = Filter.GenerateElasticSearchFilterDsl(view, view.FilterCollection);
            Console.WriteLine(filter);
            StringAssert.Contains("\"CreatedBy\":config.userName", filter);
        }
        [Test]
        public void UsingNotFilters()
        {
            var ed = new EntityDefinition { Name = "Building", Plural = "Buildings", RecordName = "Name" };
            ed.MemberCollection.Add(new Member { Name = "Name", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "Floors", Type = typeof(int) });
            ed.MemberCollection.Add(new Member { Name = "Age", Type = typeof(int) });
            ed.MemberCollection.Add(new Member { Name = "Created", Type = typeof(DateTime) });


            var view = new EntityView { Name = "All buildings", Route = "all-buildings" };
            view.SortCollection.Add(new Sort { Direction = SortDirection.Asc, Path = "Name" });
            view.SortCollection.Add(new Sort { Direction = SortDirection.Desc, Path = "Floors" });

            view.AddFilter("Name", Operator.Eq, new ConstantField { Type = typeof(string), Value = "KLCC" });
            view.AddFilter("Floors", Operator.Neq, new ConstantField { Type = typeof(int), Value = 0 });
            view.AddFilter("Created", Operator.Eq, new ConstantField { Type = typeof(DateTime), Value = DateTime.Today });

            var filter = Filter.GenerateElasticSearchFilterDsl(view, view.FilterCollection);
            Console.WriteLine(filter);
            StringAssert.Contains("\"Floors\":0", filter);
        }

        [Test]
        public void UsingAndFilters()
        {
            var ed = new EntityDefinition { Name = "Building", Plural = "Buildings", RecordName = "Name" };
            ed.MemberCollection.Add(new Member { Name = "Name", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "Floors", Type = typeof(int) });
            ed.MemberCollection.Add(new Member { Name = "Age", Type = typeof(int) });


            var view = new EntityView { Name = "All buildings", Route = "all-buildings" };
            view.SortCollection.Add(new Sort { Direction = SortDirection.Asc, Path = "Name" });
            view.SortCollection.Add(new Sort { Direction = SortDirection.Desc, Path = "Floors" });

            view.AddFilter("Age", Operator.Ge, new ConstantField { Type = typeof(int), Value = 40 });
            view.AddFilter("Age", Operator.Le, new ConstantField { Type = typeof(int), Value = 50 });
            view.AddFilter("Name", Operator.Eq, new ConstantField { Type = typeof(string), Value = "KLCC" });

            var filter = Filter.GenerateElasticSearchFilterDsl(view, view.FilterCollection);
            StringAssert.Contains("\"Name\":\"KLCC\"", filter);
        }

        [Test]
        public void UsingDateBinaryFilter()
        {
            var ed = new EntityDefinition { Name = "Building", Plural = "Buildings", RecordName = "Name" };
            ed.MemberCollection.Add(new Member { Name = "Name", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "BuiltDate", Type = typeof(DateTime) });
            ed.MemberCollection.Add(new Member { Name = "Floors", Type = typeof(int) });


            var view = new EntityView { Name = "Built before 2000", Route = "built-buildings-before-2000" };
            view.SortCollection.Add(new Sort { Direction = SortDirection.Asc, Path = "Name" });
            view.SortCollection.Add(new Sort { Direction = SortDirection.Desc, Path = "Floors" });

            view.AddFilter("BuiltDate", Operator.Le, new ConstantField { Type = typeof(DateTime), Value = new DateTime(2000, 1, 1) });
            view.AddFilter("Age", Operator.Le, new ConstantField { Type = typeof(int), Value = 50 });
            view.AddFilter("Name", Operator.Eq, new ConstantField { Type = typeof(string), Value = "KLCC" });

            var filter = Filter.GenerateElasticSearchFilterDsl(view, view.FilterCollection);
            StringAssert.Contains("\"to\":\"2000-01-01", filter);
        }
    }
}
