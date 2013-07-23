using System;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;

namespace domain.test
{
    [TestFixture]
    class SerializationTest
    {
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