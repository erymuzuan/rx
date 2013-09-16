using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    public class FieldChangedTest
    {
        [Test]
        public void GetValueInt()
        {
            var building = new Building { LotNo = "A" };
            var log = new AuditTrail();
            log.ChangeCollection.Add(new Change
            {
                Action = "Changed",
                PropertyName = "Floors",
                OldValue = "4",
                NewValue = "15"
            });
            var fcf = new FieldChangeField
            {
                TypeName = typeof(int).AssemblyQualifiedName,
                Path = "Floors"
            };
            var val = fcf.GetValue(new RuleContext(building) { Log = log });

            Assert.AreEqual(4, val);
        }

        [Test]
        public void GetValue()
        {
            var building = new Building { LotNo = "A" };
            var log = new AuditTrail();
            log.ChangeCollection.Add(new Change
            {
                Action = "Changed",
                PropertyName = "LotNo",
                OldValue = "B",
                NewValue = "A"
            });
            var fcf = new FieldChangeField
            {
                Type = typeof(string),
                Path = "LotNo"
            };
            var val = fcf.GetValue(new RuleContext(building) { Log = log });

            Assert.AreEqual("B", val);
        }
    }
}
