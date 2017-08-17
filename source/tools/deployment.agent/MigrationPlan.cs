using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements
{
    public class MigrationPlan  
    {
        public string PreviousCommitId { get; set; }
        public string CurrentCommitId { get; set; }
        public DateTime? DeployedDateTime { get; set; }

        public ObjectCollection<MemberChange> ChangeCollection { get; } = new ObjectCollection<MemberChange>();
    }
}