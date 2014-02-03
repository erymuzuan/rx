using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.entities
{
    public class ViewSortTest
    {
        [Test]
        public void SimpleSort()
        {
            var ed = new EntityDefinition { Name = "Building", Plural = "Buildings" , RecordName = "Name"};
            ed.MemberCollection.Add(new Member { Name = "Name", Type = typeof(string) });
            ed.MemberCollection.Add(new Member { Name = "Floors", Type = typeof(int) });


            var view = new EntityView {Name = "All buildings", Route = "all-buildings"};
            view.SortCollection.Add(new Sort{ Direction = SortDirection.Asc, Path = "Name"});
            view.SortCollection.Add(new Sort{ Direction = SortDirection.Desc, Path = "Floors"});

            var sortDsl = view.GenerateEsSortDsl();
            StringAssert.Contains("{\"Name\":\"asc\"}", sortDsl);
        }
    }
}
