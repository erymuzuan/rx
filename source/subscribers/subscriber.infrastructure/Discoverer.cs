﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Bespoke.Sph.Domain;

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
            set { m_path = value; }
        }

        public SubscriberMetadata[] Find()
        {
            try
            {
                var subscribers = new List<SubscriberMetadata>();
                var assemblies = Directory.GetFiles(this.ProbingPath, "*.dll");
                foreach (var dll in assemblies)
                {
                    var list = FindSubscriber(dll);
                    subscribers.AddRange(list);
                }

                return subscribers.ToArray();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("*-*-*-*-*--");
                Console.WriteLine(e.FileName);
                Console.WriteLine("*-*-*-*-*--");
            }
            return new SubscriberMetadata[] { };
        }

        private static IEnumerable<SubscriberMetadata> FindSubscriber(string dll)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dll)) return new SubscriberMetadata[] { };
                var fileName = Path.GetFileName(dll);

                if (fileName == "subscriber.infrastructure.dll") return new SubscriberMetadata[] { };
                if (!fileName.StartsWith("subscriber")) return new SubscriberMetadata[] { };

                var fullFilePath = Path.Combine(ConfigurationManager.SubscriberPath, fileName);
                Console.WriteLine("Loading : {0}", fullFilePath);
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainReflectionOnlyAssemblyResolve;
                var assembly = Assembly.ReflectionOnlyLoadFrom(dll);

                return assembly.GetTypes()
                    .Where(t => t.IsMarshalByRef)
                    .Where(t => !t.IsAbstract)
                    .Where(t => t.FullName.EndsWith("Subscriber"))
                    .Select(t => new SubscriberMetadata
                    {
                        Assembly = fullFilePath,
                        FullName = t.FullName,
                        Name = Path.GetFileNameWithoutExtension(dll),
                    })
                    .ToArray();

            }
            catch (FileNotFoundException e)
            {

                Console.WriteLine("=================");
                Console.WriteLine(e.FileName);
                Console.WriteLine("****************");
                throw;
            }
            catch (ReflectionTypeLoadException e)
            {
                Console.WriteLine("=================");
                Console.WriteLine(e);
                foreach (var loaderException in e.LoaderExceptions)
                {
                    if (null != e.InnerException)
                        Console.WriteLine(e.InnerException);
                    Console.WriteLine(loaderException);
                }
                Console.WriteLine("****************");
                throw;
            }

        }

        static Assembly CurrentDomainReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith(ConfigurationManager.ApplicationName))
            {
                var dll = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, args.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).First() + ".dll");
                Console.WriteLine("Cannot find {0}, now loading from {1}", args.Name, dll);
                return Assembly.ReflectionOnlyLoad(File.ReadAllBytes(dll));
            }
            return Assembly.ReflectionOnlyLoad(args.Name);
        }


        public void Dispose()
        {

        }
    }
}
