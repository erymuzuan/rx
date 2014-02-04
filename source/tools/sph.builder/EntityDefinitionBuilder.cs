﻿using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using subscriber.entities;

namespace sph.builder
{
    public class EntityDefinitionBuilder : Builder<EntityDefinition>
    {

        public override async Task Restore()
        {
            var folder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, "EntityDefinition");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync("dev");
                Console.WriteLine("DELETE dev : {0}", response.StatusCode);
                await client.PutAsync("dev", new StringContent(""));

            }
            this.Initialize();

            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {

                var json = File.ReadAllText(file);
                var ed = json.DeserializeFromJson<EntityDefinition>();

                await InsertAsync(ed);
                var type = CompileEntityDefinition(ed);
                Console.WriteLine("Compiled : {0}", type);

                var sqlSub = new SqlTableSubscriber();
                await sqlSub.ProcessMessageAsync(ed);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                    // mapping
                    var subs = new EntityIndexerMappingSubscriber();
                    await subs.ProcessMessageAsync(ed);
                }

                DeployCustomEntity(ed);
            }

        }

        private static Type CompileEntityDefinition(EntityDefinition ed)
        {
            var options = new CompilerOptions
            {
                IsVerbose = false,
                IsDebug = true
            };
            var webDir = ConfigurationManager.WebPath;
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(webDir + @"\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(webDir + @"\bin\core.sph.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(webDir + @"\bin\Newtonsoft.Json.dll")));

            var result = ed.Compile(options);
            result.Errors.ForEach(Console.WriteLine);

            var assembly = Assembly.LoadFrom(result.Output);
            var type = assembly.GetType(string.Format("Bespoke.Dev_{0}.Domain.{1}", ed.EntityDefinitionId, ed.Name));
            return type;
        }



        private static void DeployCustomEntity(EntityDefinition ed)
        {
            var dll = string.Format("{0}.{1}.dll", ConfigurationManager.ApplicationName, ed.Name);
            var pdb = string.Format("{0}.{1}.pdb", ConfigurationManager.ApplicationName, ed.Name);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.WebPath + @"\bin\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.WebPath + @"\bin\" + pdb, true);


            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);
        }

    }
}