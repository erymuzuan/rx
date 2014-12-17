using System;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    public class AssemblyActionTextFixture
    {
        [Test]
        public void CallPatientControllerValidate()
        {
            var action = new AssemblyAction
            {
                Title = "Validate Dob",
                Assembly = "Dev.Patient",
                TypeName = "Bespoke.Dev_patient.Domain.PatientController",
                Method = "Validate",
                IsAsyncMethod = true
            };
            action.MethodArgCollection.Add(new MethodArg
            {
                Name = "id",
                Type = typeof(string),
                ValueProvider = new ConstantField
                {
                    Name = "Dob",
                    Type = typeof(string),
                    Value = "Dob"
                }
            });
            action.MethodArgCollection.Add(new MethodArg
            {
                Name = "item",
                TypeName = "Bespoke.Dev_patient.Domain.Patient, Dev.Patient",
                ValueProvider = new FunctionField
                {
                    Name = "item",
                    Script = "item"
                }
            });
            Console.WriteLine(action.GeneratorCode());

        }
    }
}
