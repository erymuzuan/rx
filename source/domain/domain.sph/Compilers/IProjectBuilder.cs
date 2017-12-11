using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain.Compilers
{
    public interface IProjectBuilder
    {
        string Name { get; }
        string Description { get; }
        Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(IProjectDefinition project);
        Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(Member member);
        Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project);

        // TODO : should returns IEnumerable<(RxCompilerResult, IProjectDefinition)> since if we were to compile dependencies and parents
        Task<RxCompilerResult> BuildAsync(IProjectDefinition project, Func<IProjectDefinition, CompilerOptions2> getOption);
        bool IsAvailableInDesignMode { get; }
        bool IsAvailableInBuildMode { get; }
        bool IsAvailableInDeployMode { get; }

    }
}