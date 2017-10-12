using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class Discoverer : MarshalByRefObject, IDisposable
    {
        private string m_path;

        public string ProbingPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_path))
                    return ConfigurationManager.SubscriberPath;
                return m_path;
            }
            set => m_path = value;
        }

        public SubscriberMetadata[] Find()
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            try
            {
                var subscribers = new List<SubscriberMetadata>();
                var assemblies = Directory.GetFiles(this.ProbingPath, "subscriber.*.dll");
                foreach (var dll in assemblies)
                {
                    var list = FindSubscriber(dll);
                    subscribers.AddRange(list);
                }

                return subscribers.ToArray();
            }
            catch (FileNotFoundException e)
            {
                logger.WriteError(e, $"Cannot find file {e.FileName} when running Find in Discoverer");
            }
            return new SubscriberMetadata[] { };
        }

        private static IEnumerable<SubscriberMetadata> FindSubscriber(string dll)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            try
            {
                if (string.IsNullOrWhiteSpace(dll)) return new SubscriberMetadata[] { };
                var fileName = Path.GetFileName(dll);

                if (fileName == "subscriber.infrastructure.dll") return new SubscriberMetadata[] { };
                if (!fileName.StartsWith("subscriber")) return new SubscriberMetadata[] { };

                var fullFilePath = Path.Combine(ConfigurationManager.SubscriberPath, fileName);
                logger.WriteDebug($"Loading : {fullFilePath}");
                
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainReflectionOnlyAssemblyResolve;
                var assembly = Assembly.ReflectionOnlyLoadFrom(dll);

                return (from t in assembly.GetTypes()
                        where t.FullName != null && (t.IsMarshalByRef &&
                                                     !t.IsAbstract &&
                                                     t.FullName.EndsWith("Subscriber"))
                        select new SubscriberMetadata
                        {
                            Assembly = fullFilePath,
                            FullName = t.FullName,
                            Name = Path.GetFileNameWithoutExtension(dll),
                            QueueName = "TODO"
                        })
                    .ToArray();
            }
            catch (FileNotFoundException e)
            {
                logger.WriteError(e, $"Fail to find {e.FileName}");
                throw;
            }
            catch (ReflectionTypeLoadException e)
            {
                logger.WriteError(e, $"Fail to Relfect on {e.Message}");
                throw;
            }
        }

        static Assembly CurrentDomainReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs e)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();

            if (!e.Name.StartsWith(ConfigurationManager.ApplicationName)) return Assembly.ReflectionOnlyLoad(e.Name);
            logger.WriteWarning($"Fail to load {e.RequestingAssembly.Location}");

            var dll = Path.Combine(ConfigurationManager.CompilerOutputPath,
                e.Name.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries).First() + ".dll");
            logger.WriteWarning($"Cannot find {e.Name}, now loading from {dll}");
            return Assembly.ReflectionOnlyLoad(File.ReadAllBytes(dll));
        }


        public void Dispose()
        {
        }
    }
}