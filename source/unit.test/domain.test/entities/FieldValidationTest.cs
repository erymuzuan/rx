using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.entities
{
    
    public class FieldValidationTest
    {
        [Fact]
        public void ValidateExpression()
        {
            var fv = new FieldValidation
            {
                Max = 42.5f
            };
            Assert.Equal(" data-rule-max=\"42.5\"\r\n", fv.GetHtmlAttributes());
        }
        [Fact]
        public void ValidateMinExpression()
        {
            var fv = new FieldValidation
            {
                Min = 42.5f
            };
            Assert.NotStrictEqual(" data-rule-min=\"42.5\"\r\n", fv.GetHtmlAttributes());
        }
    }
}
