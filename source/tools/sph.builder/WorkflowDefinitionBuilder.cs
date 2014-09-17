using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace sph.builder
{
    internal class WorkflowDefinitionBuilder : Builder<WorkflowDefinition>
    {
        public override async Task RestoreAllAsync()
        {
            this.Initialize();
            var workflowDefinitions = this.GetItems();
            foreach (var wd in workflowDefinitions)
            {
                await RestoreAsync(wd);

            }
        }

        private async Task<bool> InsertSchemaAsync(WorkflowDefinition wd)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var folder = Path.Combine(wc, typeof(WorkflowDefinition).Name);
            var xsd = Path.Combine(folder, wd.Name + ".xsd");
            if (!File.Exists(xsd)) return false;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(xsd),
                Extension = ".xsd",
                Id = wd.SchemaStoreId,
                FileName = wd.Name + ".xsd",
                WebId = wd.SchemaStoreId
            };
            await store.AddAsync(schema);
            return true;
        }

        private void Compile(WorkflowDefinition item)
        {
            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.SphSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(typeof(Controller).Assembly.Location);
            options.ReferencedAssembliesLocation.Add(ConfigurationManager.WebPath + @"\bin\core.sph.dll");
            options.ReferencedAssembliesLocation.Add(typeof(Newtonsoft.Json.JsonConvert).Assembly.Location);
            var outputPath = ConfigurationManager.WorkflowCompilerOutputPath;
            var customDllPattern = ConfigurationManager.ApplicationName + ".*.dll";
            var entityAssembiles = Directory.GetFiles(outputPath, customDllPattern);
            foreach (var dll in entityAssembiles)
            {
                options.ReferencedAssembliesLocation.Add(dll);
            }


            var result = item.Compile(options);
            result.Errors.ForEach(Console.WriteLine);

            this.Deploy(item);
        }

        private void Deploy(WorkflowDefinition item)
        {
            var dll = string.Format("workflows.{0}.{1}.dll", item.Id, item.Version);
            var pdb = string.Format("workflows.{0}.{1}.pdb", item.Id, item.Version);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

        }


        private async Task<IEnumerable<Page>> GetPublishPagesAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException("wd");
            var screens = wd.ActivityCollection.OfType<ScreenActivity>();
            var pages = new List<Page>();
            foreach (var scr in screens)
            {
                // copy the previous version pages if there's any
                var scr1 = scr;
                var tag = string.Format("wf_{0}_{1}", wd.Id, scr1.WebId);
                var currentVersion = await context.GetMaxAsync<Page, int>(p => p.Tag == tag, p => p.Version);
                var previousPage = await context.LoadOneAsync<Page>(p => p.Tag == tag && p.Version == currentVersion);
                var code = previousPage != null ? previousPage.Code : scr1.GetView(wd);
                var page = new Page
                {
                    Code = code,
                    Name = scr1.Name,
                    IsPartial = false,
                    IsRazor = true,
                    Tag = tag,
                    Version = wd.Version,
                    WebId = Guid.NewGuid().ToString(),
                    VirtualPath = string.Format("~/Views/Workflow_{0}_{1}/{2}.cshtml", wd.Id,
                        wd.Version, scr1.ActionName)
                };


                pages.Add(page);

            }


            return pages;

        }


        public override async Task RestoreAsync(WorkflowDefinition wd)
        {
            Console.WriteLine("Compiling : {0} ", wd.Name);
            var exist = await this.InsertSchemaAsync(wd);
            if (!exist) return;

            await this.InsertAsync(wd);
            try
            {
                this.Compile(wd);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to compile workflow {0}", wd.Name);
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.StackTrace);
                Console.ResetColor();
                return;
            }
            // save
            var pages = await GetPublishPagesAsync(wd);
            //archive the WD
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var archived = new BinaryStore
            {
                Id = string.Format("wd.{0}.{1}", wd.Id, wd.Version),
                Content = Encoding.Unicode.GetBytes(wd.ToXmlString()),
                Extension = ".xml",
                FileName = string.Format("wd.{0}.{1}.xml", wd.Id, wd.Version)

            };
            await store.DeleteAsync(archived.Id);
            await store.AddAsync(archived);
            var pageBuilder = new Builder<Page>();
            pageBuilder.Initialize();
            foreach (var page in pages)
            {
                page.Id = (Guid.NewGuid()).ToString();
                await pageBuilder.InsertAsync(page);
            }
        }
    }
}