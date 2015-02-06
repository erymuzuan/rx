using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using NUnit.Framework;

namespace domain.test.CodeGenerations
{
    [TestFixture]
    public class PropertyCode
    {
        [Test]
        public void NullableInt32()
        {
            var prop = new Property
            {
                Type = typeof(int),
                IsNullable = true,
                Name = "Age"
            };
            Assert.AreEqual("public int? Age { get; set; }", prop.Code);
        }
        [Test]
        public void NullableInt32_2()
        {
            Assert.AreEqual("int?",typeof(int?).ToCSharp());
        }
    }
}
