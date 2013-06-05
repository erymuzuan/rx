using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;
using System.Linq;

namespace domain.test
{
    [TestFixture]
    public class ContractChangeTest
    {
        [Test]
        public void ContractChange()
        {
            var c = new Contract
            {
                Title = "1",
                Option = 1,
                Owner = new Owner
                    {
                        Name = "erymuzuan"
                    }
            };

            var c1 = c.Clone();
            c1.Title = "2";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c, c1);
            Assert.AreEqual(1, changes.Count(), "Title");
        }
        [Test]
        public void ContractChangeOwner()
        {
            var c2 = new Contract
            {
                Title = "1",
                Option = 1,
                Owner = new Owner
                    {
                        Name = "erymuzuan"
                    }
            };

            var c1 = c2.Clone();
            c1.Title = "2";
            c1.Owner.Name = "Erymuzuan";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1).ToList();
            Assert.AreEqual(2, changes.Count(), "Title and owner name");
            changes.ForEach(System.Console.WriteLine);
            Assert.IsTrue(changes.Any(c => c.PropertyName == "Owner.Name"));
        }
        [Test]
        public void ContractChangeOwnerAddress()
        {
            var c2 = new Contract
            {
                Title = "1",
                Option = 1,
                Owner = new Owner
                    {
                        Name = "erymuzuan",
                        Address = new Address { UnitNo = "3A" }
                    }
            };

            var c1 = c2.Clone();
            c1.Title = "2";
            c1.Owner.Name = "Erymuzuan";
            c1.Owner.Address.UnitNo = "4";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1).ToList();
            Assert.AreEqual(3, changes.Count(), "Title and owner name");
            changes.ForEach(System.Console.WriteLine);
            Assert.IsTrue(changes.Any(c => c.PropertyName == "Owner.Name"));
        }

        [Test]
        public void ContractChangeOwnerAddedField()
        {
            var c2 = new Contract
            {
                Title = "1",
                Option = 1,
                Owner = new Owner
                    {
                        Name = "erymuzuan"
                    }
            };

            var c1 = c2.Clone();
            c1.Title = "2";
            c1.Owner.Name = "Erymuzuan";
            c1.Owner.TelephoneNo = "0123889200";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1).ToList();
            Assert.AreEqual(3, changes.Count(), "Title, owner name, and phone");
            changes.ForEach(System.Console.WriteLine);
            Assert.IsTrue(changes.Any(c => c.PropertyName == "Owner.Name"));
        }

        [Test]
        public void ContractDocumentAdded()
        {
            var c2 = new Contract
            {
                Title = "1",
                Option = 1,
                Owner = new Owner
                    {
                        Name = "erymuzuan"
                    }
            };

            var c1 = c2.Clone();
            c1.DocumentCollection.Add(new Document { Title = "whatever test", Extension = "test.docx" });

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1).ToList();
            Assert.AreEqual(1, changes.Count(), "Title, owner name, and phone");
            changes.ForEach(System.Console.WriteLine);
        }

        [Test]
        public void ContractDocumentChanged()
        {
            var c2 = new Contract
            {
                Title = "1",
                Option = 1,
                Owner = new Owner
                    {
                        Name = "erymuzuan"
                    }
            };
            c2.DocumentCollection.Add(new Document { Title = "whatever test", Extension = "test.docx", WebId = "2" });

            var c1 = c2.Clone();
            c1.DocumentCollection[0].Title = "x";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1).ToList();
            Assert.AreEqual(1, changes.Count(), "Title, owner name, and phone");
            changes.ForEach(System.Console.WriteLine);
        }
        [Test]
        public void NullableStruct()
        {
            var c2 = new LatLng
            {
                Lat = 101d,
                Lng = 3
            };

            var c1 = c2.Clone();
            c1.Elevation = 102d;

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1).ToList();
            Assert.AreEqual(1, changes.Count(), "Elevation");
            changes.ForEach(System.Console.WriteLine);
        }
    }
}
