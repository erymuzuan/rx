using Bespoke.Sph.Domain;
using Xunit;

namespace domain.test.triggers
{
    
    public class FieldChangedTest
    {
        [Fact]
        public void GetValueInt()
        {
            var building = new Designation { Name = "A" };
            var log = new AuditTrail();
            log.ChangeCollection.Add(new Change
            {
                Action = "Changed",
                PropertyName = "Floors",
                OldValue = "4",
                NewValue = "15"
            });
            var fcf = new PropertyChangedField
            {
                TypeName = typeof(int).AssemblyQualifiedName,
                Path = "Floors"
            };
            var val = fcf.GetValue(new RuleContext(building) { Log = log });

            Assert.Equal(4, val);
        }

        [Fact]
        public void GetValue()
        {
            var building = new Designation { Name = "A" };
            var log = new AuditTrail();
            log.ChangeCollection.Add(new Change
            {
                Action = "Changed",
                PropertyName = "LotNo",
                OldValue = "B",
                NewValue = "A"
            });
            var fcf = new PropertyChangedField
            {
                Type = typeof(string),
                Path = "LotNo"
            };
            var val = fcf.GetValue(new RuleContext(building) { Log = log });

            Assert.Equal("B", val);
        }
    }
}
