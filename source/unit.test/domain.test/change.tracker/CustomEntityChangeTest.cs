using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;
using Xunit.Abstractions;
using domain.test.Extensions;

namespace domain.test.change.tracker
{

    public class CustomEntityChangeTest
    {
        public ITestOutputHelper Console { get; }

        public CustomEntityChangeTest(ITestOutputHelper console)
        {
            Console = console;
        }
        private EntityDefinition GetCustomerEntityDefinition()
        {
            var customerDefinition = File.ReadAllText(Path.Combine(ConfigurationManager.SphSourceDirectory, "EntityDefinition/Customer.json"));
            return customerDefinition.DeserializeFromJson<EntityDefinition>();

        }

        private async Task<dynamic> GetCustomerInstanceAsync()
        {
            var ed = this.GetCustomerEntityDefinition();
            var type = await CustomerEntityHelper.CompileEntityDefinitionAsync(ed);
            return CustomerEntityHelper.CreateCustomerInstance(type);
        }

        [Fact]
        public async Task ChangeFirstName()
        {
            var c = await this.GetCustomerInstanceAsync();
            c.FirstName = "1";

            var c1 = JsonSerializerService.JsonClone(c);
            c1.FirstName = "2";

            var generator = new ChangeGenerator();
            IEnumerable<Change> changes = generator.GetChanges(c, c1);
            foreach (var change in changes)
            {
                Console.WriteLine("{0}: {1} -> {2}", change.PropertyName.PadRight(15), change.OldValue, change.NewValue);
            }

            Assert.Single(changes);
        }

        [Fact]
        public async Task ChangeAddressState()
        {
            var c2 = await this.GetCustomerInstanceAsync();
            c2.FirstName = "1";
            c2.Address.State = "Selangor";

            var c1 = JsonSerializerService.JsonClone(c2);
            c1.FirstName = "2";
            c1.Address.State = "Kelantan";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1) as ObjectCollection<Change>;
            Assert.NotNull(changes);

            changes.ForEach(x => Console.WriteLine(x));
            Assert.Equal(2, changes.Count);
            Assert.Contains(changes, c => c.PropertyName == "Address.State");
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

            changes.ForEach(x => Console.WriteLine(x));
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

            changes.ForEach(x => Console.WriteLine(x));
            Assert.Single(changes);
        }

        [Fact]
        public async Task CollectionChildItemAdded()
        {
            var c2 = await this.GetCustomerInstanceAsync();
            c2.FirstName = "erymuzuan";
            c2.Contact.Name = "wan fatimah";

            var c1 = JsonSerializerService.JsonClone(c2);

            var assembly = Assembly.Load($"{ConfigurationManager.ApplicationName}.Customer");
            var type = assembly.GetType("Bespoke.DevV1.Customers.Domain.Attachment");
            dynamic attachment = Activator.CreateInstance(type);
            attachment.Title = "My essay";

            c1.Contact.AttachmentCollection.Add(attachment);


            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1) as ObjectCollection<Change>;
            Assert.NotNull(changes);

            changes.ForEach(x => Console.WriteLine(x));
            Assert.Single(changes);
        }

        [Fact]
        public async Task CollectionItemChanged()
        {
            var assembly = Assembly.Load($"{ConfigurationManager.ApplicationName}.Customer");
            var type = assembly.GetType($"Bespoke.DevV1.Customers.Domain.Attachment");

            dynamic attachment = Activator.CreateInstance(type);
            attachment.Title = "My essay";
            attachment.WebId = "ABC";

            var c2 = await this.GetCustomerInstanceAsync();
            c2.FirstName = "erymuzuan";
            c2.Contact.Name = "wan fatimah";
            c2.Contact.AttachmentCollection.Add(attachment);

            var c1 = JsonSerializerService.JsonClone(c2);
            c1.Contact.AttachmentCollection[0].Title = "Your essay";

            var generator = new ChangeGenerator();
            var changes = generator.GetChanges(c2, c1) as ObjectCollection<Change>;
            Assert.NotNull(changes);

            changes.ForEach(x => Console.WriteLine(x));
            Assert.Single(changes);
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
            Assert.Single(changes);
            changes.ForEach(x => Console.WriteLine(x));
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
            changes.ForEach(x => Console.WriteLine(x));
            Assert.Contains(changes, c => c.PropertyName == "Owner.Name");
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
            changes.ForEach(x => Console.WriteLine(x));
            Assert.Contains(changes, c => c.PropertyName == "Owner.Name");
        }

    }
}