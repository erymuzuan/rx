﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Csharp.CompilersServices.Extensions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.Csharp.CompilersServices
{
    [Export(typeof(IProjectBuilder))]
    public class EntityDefinitionCompiler : IProjectBuilder
    {
        public string Name => "EntityDefinition";
        public string Description => @"Compile EntityDefintion to .Net dll";
        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(IProjectDefinition project)
        {
            return AttachedProperty.EmtptyListTask;
        }

        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(Member member)
        {
            return AttachedProperty.EmtptyListTask;
        }

        [ImportMany(typeof(IBuildDiagnostics))]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

     

        public async Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new List<Class>();
            if (!(project is EntityDefinition ed)) return sources;
            var classes = await ed.GenerateCodeAsync();

            sources.AddRange(classes.Distinct(new ClassComparer()));

            return sources;
        }

        public async Task<RxCompilerResult> BuildAsync(IProjectDefinition project, Func<IProjectDefinition, CompilerOptions2> getOption)
        {
            var result = new RxCompilerResult { Result = true };
            if (!(project is EntityDefinition ed)) return result;

            var buildValidation = await ed.ValidateBuildAsync(this.BuildDiagnostics);
            if (!buildValidation.Result)
            {
                result.Errors.AddRange(buildValidation.Errors);
                return result;
            }

            result = await ed.CompileAsyncWithRoslynAsync(getOption(ed));
            return result;


        }

        public bool IsAvailableInDesignMode => false;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}
