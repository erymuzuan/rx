﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;
namespace subscriber.entities
{
    public class EntityCodeSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName
        {
            get { return "ed_code_gen"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".changed.Publish" }; }
        }

        protected override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var options = new CompilerOptions();
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.CorePath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));
            
            var result = item.Compile(options);

            result.Errors.ForEach(Console.WriteLine);
            if (!result.Result)
                this.WriteError(new Exception(result.ToJsonString(Formatting.Indented)));


            // NOTE : copy dlls, this will cause the appdomain to unload and we want it happend
            // after the Ack to the broker
            this.QueueUserWorkItem(Deploy, item);

            return Task.FromResult(0);
        }


        private void Deploy(EntityDefinition item)
        {
            Thread.Sleep(5 * 1000);

            var dll = string.Format("{0}.{1}.dll",ConfigurationManager.ApplicationName, item.Name);
            var pdb = string.Format("{0}.{1}.pdb",ConfigurationManager.ApplicationName, item.Name);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);
           
            File.Copy(dllFullPath, ConfigurationManager.WebPath + @"\bin\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.WebPath + @"\bin\" + pdb, true);


            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

        }



    }
}
