﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    sealed class EntityDefinitionDiagnostics : BuilDiagnostic
    {
        public override Task<BuildDiagnostic[]> ValidateErrorsAsync(EntityDefinition entity)
        {
            var errors = new List<BuildDiagnostic>();
            if (string.IsNullOrWhiteSpace(entity.RecordName))
                errors.Add(new BuildDiagnostic(entity.WebId, "Record name is missing"));
            if (string.IsNullOrWhiteSpace(entity.Plural))
                errors.Add(new BuildDiagnostic(entity.WebId, "Plural is missing"));

            if (entity.MemberCollection.All(m => m.Name != entity.RecordName))
                errors.Add(new BuildDiagnostic(entity.WebId, "Record name is not registered in your schema as a first level member"));

            return Task.FromResult(errors.ToArray());
        }
    }
}
