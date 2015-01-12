using System;
using System.Threading.Tasks;
using Bespoke.Dev_patient.Domain;
using Bespoke.Sph.Messaging;
using Bespoke.Sph.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace trigger.action.messaging.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var act = new MessagingAction
            {
                WebId = Guid.NewGuid().ToString(),
                Adapter = typeof(Dev.Adapters.dbo.__CisFinance).GetShortAssemblyQualifiedName(),
                OutboundMap = typeof(Dev.Integrations.Transforms.PatientToFinanceAccount).GetShortAssemblyQualifiedName(),
                Table = "Account",
                Crud = "Insert"
            };

            var patient = new Patient
            {
                FullName = "Ahmad Zakaria",
                Dob = new DateTime(1965, 5, 31),
                Gender = "Male",
                Race = "Malay",
                Id = Guid.NewGuid().ToString(),
                WebId = Guid.NewGuid().ToString(),
                Age = 49,
                HomeAddress = new HomeAddress
                {
                    Street = "No 12",
                    Street2 = "Jalan Sultan",
                    Postcode = "45700",
                    City = "Petaling Jaya",
                    State = "Selangor"
                }
            };

            await act.ExecuteAsync(new RuleContext(patient));
        }
    }
}
