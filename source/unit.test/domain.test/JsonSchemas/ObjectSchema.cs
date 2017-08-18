using System.Collections;
using System.IO;
using System.Linq;
using Bespoke.Sph.Domain;
using Mono.Cecil;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.JsonSchemas
{
    public class ObjectSchema
    {
        public ITestOutputHelper Console { get; }
        public ObjectSchema(ITestOutputHelper outputHelper)
        {
            this.Console = outputHelper;
        }


        [Fact]
        public void AssemblyToTest()
        {
            var file = $@"{ConfigurationManager.CompilerOutputPath}\assembly.test.dll";
            Assert.True(File.Exists(file), file);
            var dll = AssemblyDefinition.ReadAssembly(file);
            Assert.NotNull(dll);
            var type = dll.MainModule.Types.SingleOrDefault(x => x.FullName == "assembly.test.AssemblyClassToTest");
            Assert.NotNull(type);

            var json = type.GetJsonSchema();

            Assert.Contains("BaseStringProperty", json);
            Console.WriteLine(json);

        }
        [Fact]
        public void CollectionIListTest()
        {
            var file = $@"{ConfigurationManager.CompilerOutputPath}\assembly.test.dll";
            Assert.True(File.Exists(file), file);
            var dll = AssemblyDefinition.ReadAssembly(file);
            Assert.NotNull(dll);
            var type = dll.MainModule.Types.SingleOrDefault(x => x.FullName == "assembly.test.AssemblyClassToTest");
            Assert.NotNull(type);

            var listProperty = type.LoadProperties().Single(x => x.Name == "ChildCollection");

            var generic = listProperty.PropertyType as GenericInstanceType;
            Assert.NotNull(generic);

            var ct = listProperty.PropertyType.LoadTypeDefinition();
            while (null != ct)
            {
                if (ct.HasInterfaces)
                {
                }
                var ilist = ct.Interfaces.Any(x => x.FullName == typeof(IList).FullName);
                Console.WriteLine($"{ct.FullName} has {(ilist ? "" : "not")} IList");
                ct = ct.BaseType.LoadTypeDefinition();
            }


        }
    }
}
