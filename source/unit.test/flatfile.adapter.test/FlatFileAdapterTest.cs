using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using FileHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace flatfile.adapter.test
{
    [TestClass]
    public class FlatFileAdapterTest
    {
        [TestMethod]
        public async Task CompileAsync()
        {
            var adapter = new FlatFileAdapter
            {
                IsPositional = false,
                Delimiter = "|",
                EscapeCharacted = "\"",
                Name = "TestCustomerFlatFile",
                Id = "test-customer-flatfile",
                Schema = "FlatFile",
                WebId = Strings.GenerateId(),
                Tables = new[]
                {
                    new AdapterTable {Name = "Customer"},
                }
            };

            var customer = new TableDefinition { Name = "Customer" , Schema = adapter.Schema, ClassAttribute = "[FileHelpers.DelimitedRecord(\", \")]" };
            customer.MemberCollection.Add(new FlatFileMember { Name = "No" , Type = typeof(string), IsNullable = true });
            customer.MemberCollection.Add(new FlatFileMember { Name = "RegisterdDate" , Type = typeof(string), IsNullable = true });
            customer.ActionCodeGenerators = new ObjectCollection<ControllerAction>();

            adapter.TableDefinitionCollection.Add(customer);
            
            await adapter.OpenAsync(true);

            var options = new CompilerOptions();
            options.ReferencedAssembliesLocation.Add(typeof(CsvEngine).Assembly.Location);
            await adapter.CompileAsync(options);

        }
    }
}
