using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.IosCompiler
{
    [Export(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler))]
    [FormCompilerMetadata(Name = Constants.IOS_SWIFT)]
    public class SwftFormCompiler : FormCompiler
    {
        public override Task<WorkflowCompilerResult> CompileAsync(EntityForm entityForm)
        {
            var result = new WorkflowCompilerResult {Result = false};
            result.Errors.Add(new BuildError("", "Whoaaa mate.. do you really think it might work!! better luck next time!!"));

            return Task.FromResult(result);
        }
    }
}