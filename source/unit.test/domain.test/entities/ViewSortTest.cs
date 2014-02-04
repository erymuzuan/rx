using Bespoke.Sph.Domain;
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

            var sortDsl = view.GenerateElasticSearchFilterDsl();
            StringAssert.Contains("\"Name\":\"KLCC\"", sortDsl);
        }
    }
}
