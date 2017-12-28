using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Csharp.CompilersServices;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests
{
    public class EntityDefinitonCompilerTest
    {
        public ITestOutputHelper Console { get; }

        public EntityDefinitonCompilerTest(ITestOutputHelper console)
        {
            Console = console;
            ObjectBuilder.AddCacheList<ICvsProvider>(new MockGit());
            var disk = new MockSourceRepository();
            var addressDefinition = new ValueObjectDefinition { Id = "address", Name = "Address" };
            addressDefinition.MemberCollection.Add(new SimpleMember { Name = "Street", Type = typeof(string) });
            addressDefinition.MemberCollection.Add(new SimpleMember { Name = "Postcode", Type = typeof(string) });

            disk.AddOrReplace(addressDefinition);

            ObjectBuilder.AddCacheList<ISourceRepository>(disk);
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }

        [Fact]
        public async Task WithValueObjectMember()
        {
            await Task.Delay(100);
            var patient = new EntityDefinition
            {
                Name = "Patient",
                Id = "patient",
                Plural = "Patients",
                WebId = "patient",
                RecordName = "Mrn"
            };
            patient += ("Mrn", typeof(string));
            patient += ("FullName", typeof(string));
            patient += ("MyKad", typeof(string));
            patient += ("Age", typeof(int));

            Assert.Equal(4, patient.MemberCollection.Count);

            patient.MemberCollection.Add(new ValueObjectMember
            {
                Name = "HomeAddress",
                ValueObjectName = "Address"
            });
            patient.MemberCollection.Add(new ValueObjectMember
            {
                Name = "OfficeAddress",
                ValueObjectName = "Address"
            });

            var compiler = new EntityDefinitionCompiler { BuildDiagnostics = Array.Empty<IBuildDiagnostics>() };
            var sources = (await compiler.GenerateCodeAsync(patient)).ToList();
            foreach (var @class in sources)
            {
                Console.WriteLine($"{@class.Name}.cs");
            }

            Assert.Equal(2, sources.Count);


            var sr = await compiler.BuildAsync(patient, p => new CompilerOptions2("test-patient.dll", "test-patient.pdb"));
            Assert.True(sr.Result);

            Assert.True(File.Exists(sr.Output));
        }
    }
}
