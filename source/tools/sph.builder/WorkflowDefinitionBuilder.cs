using System;
using System.Collections.Generic;
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
        public override async Task Restore()
        {
            var pageBuilder = new Builder<Page>();
            pageBuilder.Initialize();
            this.Initialize();
            var workflowDefinitions = this.GetItems();
            foreach (var wd in workflowDefinitions)
            {
                Console.WriteLine("Compiling : {0} ", wd.Name);
                var exist = await this.InsertSchemaAsync(wd);
                if (!exist) continue;

                await this.InsertAsync(wd);
                this.Compile(wd);

                // save
                var pages = await GetPublishPagesAsync(wd);
                await this.DeletePreviousPagesAsync(wd);
                //archive the WD
                var store = ObjectBuilder.GetObject<IBinaryStore>();
                var archived = new BinaryStore
                {
                    StoreId = string.Format("wd.{0}.{1}", wd.WorkflowDefinitionId, wd.Version),
                    Content = Encoding.Unicode.GetBytes(wd.ToXmlString()),
                    Extension = ".xml",
                    FileName = string.Format("wd.{0}.{1}.xml", wd.WorkflowDefinitionId, wd.Version)

                };
                await store.DeleteAsync(archived.StoreId);
                await store.AddAsync(archived);
                foreach (var page in pages)
                {
                    await pageBuilder.InsertAsync(page);
                }

            }
        }

        private async Task<bool> InsertSchemaAsync(WorkflowDefinition wd)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var folder = Path.Combine(wc, typeof(WorkflowDefinition).Name);
            var xsd = Path.Combine(folder, wd.Name + ".xsd");
            if (!File.Exists(xsd)) return false;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(xsd),
                Extension = ".xsd",
                StoreId = wd.SchemaStoreId,
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
                SourceCodeDirectory = ConfigurationManager.WorkflowSourceDirectory
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
            var dll = string.Format("workflows.{0}.{1}.dll", item.WorkflowDefinitionId, item.Version);
            var pdb = string.Format("workflows.{0}.{1}.pdb", item.WorkflowDefinitionId, item.Version);
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
                var tag = string.Format("wf_{0}_{1}", wd.WorkflowDefinitionId, scr1.WebId);
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
                    VirtualPath = string.Format("~/Views/Workflow_{0}_{1}/{2}.cshtml", wd.WorkflowDefinitionId,
                        wd.Version, scr1.ActionName)
                };


                pages.Add(page);

            }


            return pages;

        }

        private async Task DeletePreviousPagesAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            var pages = new List<Page>();
            foreach (var act in wd.ActivityCollection.OfType<ScreenActivity>())
            {
                var act1 = act;
                var page = await context.LoadOneAsync<Page>(p =>
                                p.Version == wd.Version &&
                                p.Tag == string.Format("wf_{0}_{1}", wd.WorkflowDefinitionId, act1.WebId));
                if (null != page)
                    pages.Add(page);
            }
            using (var session = context.OpenSession())
            {
                session.Delete(pages.Cast<Entity>().ToArray());
                await session.SubmitChanges();
            }
        }

    }
}