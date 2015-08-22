using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.diagnostics;

namespace es.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class TestDiagnostics : BuilDiagnostic
    {

        public override Task<BuildError[]> ValidateWarningsAsync(EntityDefinition ed)
        {
            throw new InvalidOperationException("You should not get this one");
        }

        public override Task<BuildError[]> ValidateErrorsAsync(EntityDefinition ed)
        {
            throw new InvalidOperationException("You should not get this one ->ValidateErrorsAsync");

        }


    }
}