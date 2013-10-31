using System;
using System.Diagnostics;
using System.IO;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WordGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace generate.template.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var contract = new Contract
            {
                ReferenceNo = "1234",
                Tenant = new Tenant
                {
                    Name = "MY NAME" ,
                    Address = new Address
                    {
                        Street = "No 1",
                        State = "Johor",
                        Postcode = "787878",
                        City = "JB"
                    }
                }
            };
            IDocumentGenerator gen = new WordGenerator{DefaultNamespace = typeof(Contract).Namespace};
            Console.WriteLine(((WordGenerator) gen).DefaultNamespace);
            var output = Path.GetTempFileName() + ".docx";
            File.Copy(@"\project\work\sph\source\unit.test\generate.template.test\DATETIME.docx", output);

            gen.Generate(output, contract);

            Process.Start(output);
        }
    }
}
