﻿using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class WorkflowGenerationTest
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();

        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.Setup(x => x.GetContent(m_schemaStoreId))
                .Returns(doc);
            ObjectBuilder.AddCacheList(store.Object);
        }

        [Test]
        public void GenerateCsharpClasses()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });
            var screen = new ScreenActivity { Name = "Test", WebId = "A", IsInitiator = true, NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var options = new CompilerOptions { IsDebug = true, IsVerbose = false };
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll")));

            var code = wd.GenerateXsdCsharpClasses();
            StringAssert.Contains("public partial class Vehicle", code);

        }

        [Test]
        public async Task GenerateJavascriptClasses()
        {
            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });
            var screen = new ScreenActivity { Name = "Test", WebId = "A", IsInitiator = true, NextActivityWebId = "B" };
            wd.ActivityCollection.Add(screen);
            wd.ActivityCollection.Add(new EndActivity { Name = "Habis test", WebId = "B" });

            var options = new CompilerOptions { IsDebug = true, IsVerbose = false };
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll")));

            var script = await wd.GenerateCustomXsdJavascriptClassAsync();
            Assert.IsNotNull(script);
            StringAssert.Contains("bespoke.sph.w_8_0.Vehicle", script);

        }

        [Test]
        public void Compile()
        {

            var wd = new WorkflowDefinition { Name = "Permohonan Tanah Wakaf", WorkflowDefinitionId = 8, SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "Title", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "pemohon", TypeName = "Applicant" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "alamat", TypeName = "Address" });


            var apply = new ScreenActivity
            {
                Title = "Apply",
                ViewVirtualPath = "~/Views/Workflows_8_1/pohon.cshtml",
                WebId = Guid.NewGuid().ToString(),
                NextActivityWebId = Guid.NewGuid().ToString(),
                IsInitiator = true,
                Name = "Starts Screen"
            };
            apply.FormDesign.FormElementCollection.Add(new TextBox { Path = "Nama", Label = "Test" });
            apply.FormDesign.FormElementCollection.Add(new TextBox { Path = "Title", Label = "Tajuk" });
            wd.ActivityCollection.Add(apply);
            wd.ActivityCollection.Add(new EndActivity { WebId = apply.NextActivityWebId, Name = "Habis" });

            wd.Version = Directory.GetFiles(".", "workflows.8.*.dll").Length + 1;

            var options = new CompilerOptions { IsDebug = true };
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.json.dll")));

            var result = wd.Compile(options);
            result.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(result.Result);
            Assert.IsTrue(File.Exists(result.Output), "assembly " + result.Output);

            var view = apply.GetView(wd);
            Assert.IsNotNull(view);

            // try to instantiate the Workflow
            var assembly = Assembly.LoadFrom(result.Output);
            var wfTypeName = string.Format("Bespoke.Sph.Workflows_{0}_{1}.{2}", wd.WorkflowDefinitionId, wd.Version,
                wd.WorkflowTypeName);

            var wfType = assembly.GetType(wfTypeName);
            Assert.IsNotNull(wfType, wfTypeName + " is null");

            var wf = Activator.CreateInstance(wfType) as Entity;
            Assert.IsNotNull(wf);

        }
    }
}