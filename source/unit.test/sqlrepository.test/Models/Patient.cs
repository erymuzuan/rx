using System;
using System.Diagnostics;
using Bespoke.Sph.Domain;

namespace sqlrepository.test.Models
{
    [DebuggerDisplay("{Mrn}({FullName})")]
    [PersistenceOption(IsSource = true, IsSqlDatabase = true, IsElasticsearch = true)]
    public class Patient : Entity
    {
        public string Mrn { get; set; }
        public string FullName { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public NextOfKin NextOfKin { get; set; }
        public Spouse Wife { get; set; }
        public string Race { get; set; }
        public bool IsMalaysian => Race != "Others";
    }
}