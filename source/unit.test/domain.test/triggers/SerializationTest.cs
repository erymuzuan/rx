using System;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;
using roslyn.scriptengine;

namespace domain.test.triggers
{
    [TestFixture]
    class SerializationTest
    {
        [Test]
        public void FunctionFieldToXml()
        {
            var original = new FunctionField{ Script = "DateTime.Today"};
            var xml = XmlSerializerService.ToUtf8EncodedXmlString(original);
            Console.WriteLine(xml);
            
            
            var ff = XmlSerializerService.DeserializeFromXml<FunctionField>(xml);
            ff.ScriptEngine  = new RoslynScriptEngine();
            Assert.IsNotNull(ff);
            Assert.IsInstanceOf<FunctionField>(ff);

            var val = ff.GetValue(new RuleContext(new Building()));
            Assert.AreEqual(DateTime.Today,val );

        }

        [Test]
        public void DocumentFieldToXml()
        {
            var df = new DocumentField{ XPath = "//bs:Building/@BuildingId", Type = typeof(int), NamespacePrefix = "bs"};
            var xml = XmlSerializerService.ToUtf8EncodedXmlString(df);
            Console.WriteLine(xml);
            var ff = XmlSerializerService.DeserializeFromXml<DocumentField>(xml);
            Assert.IsNotNull(ff);
            Assert.IsInstanceOf<DocumentField>(ff);

            var df2 = (DocumentField)ff;
            Assert.IsTrue(typeof(int) == df2.Type);

        }
         
    }

}