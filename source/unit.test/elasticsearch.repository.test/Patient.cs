using System;
using System.Diagnostics;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    [DebuggerDisplay("{Mrn}({FullName})")]
    public class Patient : Entity
    {
        public string Mrn { get; set; }
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public NextOfKin NextOfKin { get; set; }
    }
}