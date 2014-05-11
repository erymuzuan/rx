using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.entities
{
    [TestFixture]
    public class FieldValidationTest
    {
        [Test]
        public void ValidateExpression()
        {
            var fv = new FieldValidation
            {
                Max = 42.5f
            };
            StringAssert.AreEqualIgnoringCase(" data-rule-max=\"42.5\"\r\n", fv.GetHtmlAttributes());
        }
        [Test]
        public void ValidateMinExpression()
        {
            var fv = new FieldValidation
            {
                Min = 42.5f
            };
            StringAssert.AreEqualIgnoringCase(" data-rule-min=\"42.5\"\r\n", fv.GetHtmlAttributes());
        }
    }
}
