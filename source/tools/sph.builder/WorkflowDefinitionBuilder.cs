using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
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


        private void Compile(WorkflowDefinition item)
        {
            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.SphSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(typeof(Controller).Assembly.Location);
            options.ReferencedAssembliesLocation.Add(ConfigurationManager.WebPath + @"\bin\core.sph.dll");
            options.ReferencedAssembliesLocation.Add(typeof(Newtonsoft.Json.JsonConvert).Assembly.Location);
            var outputPath = ConfigurationManager.CompilerOutputPath;
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
            var dll = $"workflows.{item.Id}.{item.Version}.dll";
            var pdb = $"workflows.{item.Id}.{item.Version}.pdb";
            var dllFullPath = Path.Combine(ConfigurationManager.CompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.CompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

        }


        private async Task<IEnumerable<Page>> GetPublishPagesAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException(nameof(wd));
            var screens = wd.ActivityCollection.OfType<ScreenActivity>();
            var pages = new List<Page>();
            foreach (var scr in screens)
            {
                // copy the previous version pages if there's any
                var scr1 = scr;
                var tag = $"wf_{wd.Id}_{scr1.WebId}";
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
                    VirtualPath = $"~/Views/{wd.WorkflowTypeName}/{scr1.ActionName}.cshtml"
                };
                pages.Add(page);

            }


            return pages;

        }


        public override async Task RestoreAsync(WorkflowDefinition wd)
        {
            Console.WriteLine("Compiling : {0} ", wd.Name);
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
                Id = $"wd.{wd.Id}.{wd.Version}",
                Content = Encoding.Unicode.GetBytes(wd.ToJsonString(true)),
                Extension = ".json",
                FileName = $"wd.{wd.Id}.{wd.Version}.json"
            };
            await store.DeleteAsync(archived.Id);
            await store.AddAsync(archived);
            var pageBuilder = new Builder<Page>();
            pageBuilder.Initialize();
            foreach (var page in pages)
            {
                page.Id = (Guid.NewGuid()).ToString();
            }
        }
    }
}