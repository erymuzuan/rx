using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.JsonSchemas
{
    public class ObjectSchema
    {

        [Fact]
        public void AdventureWorkPerson()
        {
            var file = $"{ConfigurationManager.CompilerOutputPath}\\DevV1.AdventureWorks.dll";
            Assert.True(File.Exists(file), file);
            var dll = Assembly.LoadFile(file);
            Assert.NotNull(dll);
            var type = dll.GetType("Bespoke.DevV1.Adapters.AdventureWorks.Person");
            Assert.NotNull(type);

            var schema = new StringBuilder();
            var properties = from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             where p.DeclaringType != typeof(DomainObject)
                             select p.GetJsonSchema();

            schema.Append($@"
{{
  ""type"": ""object"",
  ""properties"": {{
   {properties.ToString(",\r\n")}
  }}
}}");


        }
    }
}
