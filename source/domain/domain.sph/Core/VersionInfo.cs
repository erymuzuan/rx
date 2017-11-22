using System;

namespace Bespoke.Sph.Domain
{
    [Flags]
    public enum VersionInfo
    {
        Version = 1,
        CommitId = 2,
        Revision = 4,
        All = Version | CommitId | Revision,
        None = 8,

    }
}