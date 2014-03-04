﻿using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class DocumentTemplate : Entity
    {
        public Task<BuildValidationResult> ValidateBuildAsync(EntityDefinition ed)
        {
            return Task.FromResult(new BuildValidationResult { Result = true });
        }
    }
}