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
    public class QueryEndpointCompiler : IProjectBuilder
    {
        public string Name => "QueryEndpoint";
        public string Description => @"Compile QueryEndpoint or if in case of EntityDefinition, all its QueryEndpoints";
        public Task<IEnumerable<AttachProperty>> GetAttachPropertiesAsycn(IProjectDefinition project)
        {
            return Task.FromResult(Array.Empty<AttachProperty>().AsEnumerable());
        }

        public Task<IEnumerable<AttachProperty>> GetAttachPropertiesAsycn(Member member)
        {
            return Task.FromResult(Array.Empty<AttachProperty>().AsEnumerable());
        }
        public async Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new List<Class>();
            if (!(project is QueryEndpoint endpoint)) return sources;
            var ed = new SphDataContext().LoadOneFromSources<EntityDefinition>(x => x.Name == endpoint.Entity);

            var wrapper = new QueryEndpointCsharp(endpoint, ed);

            var controller = wrapper.GenerateCode();
            var info = await wrapper.GetAssemblyInfoCodeAsync();

            sources.AddRange(controller, info);

            return sources;

        }

        public async Task<RxCompilerResult> BuildAsync(IProjectDefinition project, Func<IProjectDefinition, CompilerOptions2> getOption)
        {
            //if (project is EntityDefinition schema)
            //{
            //    var results = new List<RxCompilerResult>();
            //    var endpoints = new SphDataContext().LoadFromSources<QueryEndpoint>(x => x.Entity == schema.Name);
            //    foreach (var qe in endpoints)
            //    {
            //        var options2 = getOption(qe);
            //        var endpointCompilationResult = await qe.CompileAsyncWithRoslynAsync(options2);
            //        results.Add(endpointCompilationResult);
            //    }

            //    var rx = new RxCompilerResult { Result = results.TrueForAll(x => x.Result) };
            //    rx.Errors.AddRange(results.SelectMany(x => x.Errors));
            //    return rx;
            //}

            if (project is QueryEndpoint endpoint)
            {
                return await endpoint.CompileAsyncWithRoslynAsync(getOption(endpoint));
            }
            return new RxCompilerResult();
        }

        public bool IsAvailableInDesignMode => false;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}
