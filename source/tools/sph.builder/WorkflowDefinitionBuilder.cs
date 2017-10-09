using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    internal class WorkflowDefinitionBuilder : Builder<WorkflowDefinition>
    {
        protected override Task<WorkflowCompilerResult> CompileAssetAsync(WorkflowDefinition item)
        {
            return item.CompileAsync();
        }

    



    }
}