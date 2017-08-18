using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.change.tracker
{
    
    public class CustomEntityChangeTest
    {
        private EntityDefinition GetCustomerEntityDefinition()
        {
            var customerDefinition = File.ReadAllText(Path.Combine(ConfigurationManager.SphSourceDirectory, "EntityDefinition/Customer.json"));
            return customerDefinition.DeserializeFromJson<EntityDefinition>();

        }

        private dynamic GetCustomerInstance()
        {
            var ed = this.GetCustomerEntityDefinition();
            var type = CustomerEntityHelper.CompileEntityDefinition(ed);
            return CustomerEntityHelper.CreateCustomerInstance(type);
        }

        [Fact]
        public void ChangeFirstName()
        {
            var c = this.GetCustomerInstance();
            c.FirstName = "1";

            var c1 = JsonSerializerService.JsonClone(c);
            c1.FirstName = "2";

            var generator = new ChangeGenerator();
            IEnumerable<Change> changes = generator.GetChanges(c, c1);
            foreach (var change in changes)
            {
                Console.WriteLine("{0}: {1} -> {2}", change.PropertyName.PadRight(15), change.OldValue, change.NewValue);
            }

            Assert.Equal(1, changes.Count());
        }

        [Fact]
        public void ChangeAddressState()
        {
            var c2 = this.GetCustomerInstance();
            c2.FirstName = "1";
            c2.Address.State = "Selangor";

            var c1 = JsonSerializerService.JsonClone(c2);
            c1.FirstName = "2";
            c1.Address.State = "Kelantan";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1) as ObjectCollection<Change>;
            Assert.NotNull(changes);

            changes.ForEach(Console.WriteLine);
            Assert.Equal(2, changes.Count);
            Assert.True(changes.Any(c => c.PropertyName == "Address.State"));
        }

        [Fact]
        public void PrimitiveCollectionChildItemAdded()
        {
            var c2 = new Designation();
            c2.RoleCollection.Add("admin");
            var c1 = c2.JsonClone();
            
            c1.RoleCollection.Add("dev");

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1) as ObjectCollection<Change>;
            Assert.NotNull(changes);

            changes.ForEach(Console.WriteLine);
            Assert.Equal(2, changes.Count);
        }

        [Fact]
        public void PrimitiveCollectionItemChanged()
        {
            var c2 = new Designation();
            c2.RoleCollection.Add("admin");
            var c1 = c2.JsonClone();
            
            c1.RoleCollection[0] = "dev";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1) as ObjectCollection<Change>;
            Assert.NotNull(changes);

            changes.ForEach(Console.WriteLine);
            Assert.Equal(1, changes.Count);
        }

        [Fact]
        public void CollectionChildItemAdded()
        {
            var c2 = this.GetCustomerInstance();
            c2.FirstName = "erymuzuan";
            c2.Contact.Name = "wan fatimah";

            var c1 = JsonSerializerService.JsonClone(c2);

            var assembly = Assembly.Load($"{ConfigurationManager.ApplicationName}.Customer");
            var type = assembly.GetType($"Bespoke.DevV1.Customers.Domain.Attachment");
            dynamic attachment = Activator.CreateInstance(type);
            attachment.Title = "My essay";

            c1.Contact.AttachmentCollection.Add(attachment);


            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1) as ObjectCollection<Change>;
            Assert.NotNull(changes);

            changes.ForEach(Console.WriteLine);
            Assert.Equal(1, changes.Count);
        }

        [Fact]
        public void CollectionItemChanged()
        {
            var assembly = Assembly.Load($"{ConfigurationManager.ApplicationName}.Customer");
            var type = assembly.GetType($"Bespoke.DevV1.Customers.Domain.Attachment");

            dynamic attachment = Activator.CreateInstance(type);
            attachment.Title = "My essay";
            attachment.WebId = "ABC";

            var c2 = this.GetCustomerInstance();
            c2.FirstName = "erymuzuan";
            c2.Contact.Name = "wan fatimah";
            c2.Contact.AttachmentCollection.Add(attachment);

            var c1 = JsonSerializerService.JsonClone(c2);
            c1.Contact.AttachmentCollection[0].Title = "Your essay";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1) as ObjectCollection<Change>;
            Assert.NotNull(changes);

            changes.ForEach(Console.WriteLine);
            Assert.Equal(1, changes.Count);
        }


        [Fact]
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
            Assert.Equal(1, changes.Count());
            changes.ForEach(Console.WriteLine);
        }

        [Fact]
        public void DesignationChangeOwnerAddress()
        {
            var c2 = new Designation
            {
                Title = "1",
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
            Assert.Equal(3, changes.Count());
            changes.ForEach(System.Console.WriteLine);
            Assert.True(changes.Any(c => c.PropertyName == "Owner.Name"));
        }

        [Fact]
        public void DesignationChangeOwnerAddedField()
        {
            var c2 = new Designation
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
            Assert.Equal(3, changes.Count());
            changes.ForEach(System.Console.WriteLine);
            Assert.True(changes.Any(c => c.PropertyName == "Owner.Name"));
        }

    }
}