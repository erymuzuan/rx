﻿using System;
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
            var sourceOffenceType = Assembly.LoadFrom(@".\rsc.RilekWeb.dll").GetType("rsc.Adapters.PdrmServices.Saman");
            dynamic sourceOffence = Activator.CreateInstance(sourceOffenceType);
            sourceOffence.NoKenderaan = "WVJ 7004";
            dynamic sourceOffence2 = Activator.CreateInstance(sourceOffenceType);
            sourceOffence2.NoKenderaan = "WVJ 7004";


            var destinationType = Assembly.LoadFrom(@".\rsc.Driver.dll").GetType("Bespoke.rsc_1.Domain.Driver");
            source.MyKad = "750418035249";
            source.SamanCollection.Add(sourceOffence);
            source.SamanCollection.Add(sourceOffence2);


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


            var sourceSummons = new SourceFunctoid
            {
                WebId = "sourceOffence",
                Field = "SamanCollection"
            };
            td.FunctoidCollection.Add(sourceSummons);
            var destinationOffences = new SourceFunctoid
            {
                WebId = "destinationOffences",
                Field = "OffenceCollection",
                OutputTypeName = "Bespoke.rsc_1.Domain.Offence, rsc.Driver"
            };
            td.FunctoidCollection.Add(destinationOffences);

            var loop = new LoopingFunctoid{WebId = "_loop"};
            loop.Initialize();
            loop["sourceCollection"].Functoid = sourceSummons.WebId;

            td.FunctoidCollection.Add(loop);
            
            td.MapCollection.Add(new DirectMap
            {
                Source = "SamanCollection.NoKenderaan",
                Destination = "OffenceCollection.VehicleNo"
            });

            var loopToDest = new FunctoidMap
            {
                WebId = "__loopToDest",
                Destination = "OffenceCollection",
                Functoid = loop.WebId,
                DestinationTypeName = "Bespoke.rsc_1.Domain.Offence, rsc.Driver"
            };
            td.MapCollection.Add(loopToDest);

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
            Assert.AreEqual("WVJ 7005", destination.OffenceCollection[0].VehicleNo);

        }
    }
}
