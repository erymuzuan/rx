using System;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mapping.transformation.test
{
    [TestClass]
    public class LoopTestFixture
    {
        [TestMethod]
        public async Task SimpleCollection()
        {
            var sourceType = Assembly.LoadFrom(@".\rsc.RilekWeb.dll").GetType("rsc.Adapters.PdrmServices.PostRilekRilekPdrmResponse");
            dynamic source = Activator.CreateInstance(sourceType);


            var destinationType = Assembly.LoadFrom(@".\rsc.Driver.dll").GetType("Bespoke.rsc_1.Domain.Driver");
            source.MyKad = "750418035249";


            var td = new TransformDefinition
            {
                InputType = sourceType,
                OutputType = destinationType,
                Name = "__SimpleCollectionMapping"
            };
            td.MapCollection.Add(new DirectMap
            {
                Destination = "MyKad",
                Source = "MyKad"
            });


            var sourceOffences = new SourceFunctoid
            {
                WebId = "sourceOffence",
                Field = "OffenceCollection"
            };
            td.FunctoidCollection.Add(sourceOffences);
            var destinationOffences = new SourceFunctoid
            {
                WebId = "destinationOffences",
                Field = "OffenceCollection",
                OutputTypeName = "Bespoke.rsc_1.Domain.Offence, rsc.Driver"
            };
            td.FunctoidCollection.Add(destinationOffences);

            var loop = new LoopingFunctoid();
            loop.Initialize();
            loop["sourceCollection"].Functoid = sourceOffences.WebId;
            loop["destinationCollection"].Functoid = destinationOffences.WebId;
            loop["destinationCollection"].TypeName = destinationOffences.OutputTypeName;

            td.FunctoidCollection.Add(loop);
            
            td.MapCollection.Add(new DirectMap
            {
                Source = "OffenceCollection.VehicleNo",
                Destination = "OffenceCollection.VehicleNo"
            });

            var options = new CompilerOptions();
            var sourceCodes = td.GenerateCode();
            var sourceFiles = td.SaveSources(sourceCodes);
            var cr = await td.CompileAsync(options, sourceFiles);
            Assert.IsTrue(cr.Result,"Cannot compile : " + cr.Errors);
            var mapType = Assembly.LoadFrom(cr.Output).GetType("Dev.Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mapType);


            var destination = await map.TransformAsync(source);


            Assert.AreEqual("750418035249", destination.MyKad);
            Assert.AreEqual(2, destination.OffenceCollection.Count);

        }
    }
}
