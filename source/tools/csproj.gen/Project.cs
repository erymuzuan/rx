﻿using System.IO;
using System.Reflection;
using Bespoke.Sph.Domain;
using RazorEngine;
using RazorEngine.Templating;

namespace csproj.gen
{
    public class Project
    {
        public string GetNugetPackagesConfig()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "csproj.gen.packages.xml";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                return raw;
            }

        }
        public string Generate(EntityDefinition item, string appName)
        {
            var vm = new
            {
                EntityDefinition = item,
                ApplicationName = appName,
                CsFiles = new string[] { }
            };

            var assembly = Assembly.GetExecutingAssembly();
            const string RESOURCE_NAME = "csproj.gen.ed.xml";

            using (var stream = assembly.GetManifestResourceStream(RESOURCE_NAME))
            using (var reader = new StreamReader(stream))
            {
                var raw = reader.ReadToEnd();
                var markup = this.TransformTemplate(raw, vm);
                return markup;

            }

        }

        public string TransformTemplate(string template, dynamic model)
        {
            var viewBag = new DynamicViewBag();
            if (string.IsNullOrWhiteSpace(template)) return string.Empty;
            var result = Engine.Razor.RunCompile(template, template, null, (object)model, viewBag);
            return result;
        }
    }
}