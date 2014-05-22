using System;
using System.Diagnostics;
using System.IO;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WordGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace generate.template.test
{
    [TestClass]
    public class WordDocumentGenerateTest
    {
        [TestMethod]
        public void WithCustomEntity()
        {
            var contract = new Designation
            {
                Name = "1234",
                StartModule = "test"
            };
            IDocumentGenerator gen = new WordGenerator { DefaultNamespace = typeof(Designation).Namespace };
            Console.WriteLine(((WordGenerator)gen).DefaultNamespace);
            var output = Path.GetTempFileName() + ".docx";
            File.Copy(@"\project\work\sph\source\unit.test\generate.template.test\DATETIME.docx", output);

            gen.Generate(output, contract);

            Process.Start(output);
        }
    }
}
