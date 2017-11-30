﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Csharp.CompilersServices.Extensions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.Csharp.CompilersServices
{
    [Export(typeof(IProjectBuilder))]
    public class EntityDefinitionCompiler : IProjectBuilder
    {
        [ImportMany(typeof(IBuildDiagnostics))]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }
        private ILogger Logger => ObjectBuilder.GetObject<ILogger>();


        public Task<IEnumerable<AttachProperty>> GetAttachPropertiesAsycn(IProjectDefinition project)
        {
            return Task.FromResult(Array.Empty<AttachProperty>().AsEnumerable());
        }

        public Task<IEnumerable<AttachProperty>> GetAttachPropertiesAsycn(Member member)
        {
            return Task.FromResult(Array.Empty<AttachProperty>().AsEnumerable());
        }

        public async Task<Dictionary<string, string>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new Dictionary<string, string>();
            if (!(project is EntityDefinition ed)) return sources;
            var classes = await ed.GenerateCodeAsync();

            var folder = $@"{ConfigurationManager.GeneratedSourceDirectory}\{ed.Name}";
            if (!System.IO.Directory.Exists(folder))
                System.IO.Directory.CreateDirectory(folder);

            foreach (var @class in classes)
            {
                var file = $@"{folder}\{@class.FileName}";
                Logger.WriteDebug($"Generate class {@class.Name}-> {file}");
                sources.Add(file, @class.GetCode());
            }

            return sources;
        }

        public async Task<RxCompilerResult> BuildAsync(IProjectDefinition project, string[] sources)
        {
            var result = new RxCompilerResult { Result = true };
            if (!(project is EntityDefinition ed)) return result;

            var buildValidation = await ed.ValidateBuildAsync(this.BuildDiagnostics);
            if (!buildValidation.Result)
            {
                result.Errors.AddRange(buildValidation.Errors);
                return result;
            }

            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.GeneratedSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll");

            result = ed.Compile(options, sources);
            return result;


        }

        public bool IsAvailableInDesignMode => false;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}
