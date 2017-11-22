using System;
using System.Diagnostics;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.CosmosDb.Models
{
    [DebuggerDisplay("{Mrn}({FullName})")]
    public class Patient : Entity
    {
        public Patient()
        {
            var rc = new RuleContext(this);
            this.NextOfKin = new NextOfKin();
            this.HomeAddress = new CustomHomeAddress();
            this.Wife = new Spouse();
        }

        public string Mrn { get; set; }

        public string FullName { get; set; }

        public DateTime Dob { get; set; }

        public string Gender { get; set; }

        public string Religion { get; set; }

        public string Race { get; set; }

        public DateTime RegisteredDate { get; set; }

        public string IdentificationNo { get; set; }

        public string PassportNo { get; set; }

        public string Nationality { get; set; }

        public NextOfKin NextOfKin { get; set; }

        public CustomHomeAddress HomeAddress { get; set; }

        public string Occupation { get; set; }

        public string Status { get; set; }

        public int Age { get; set; }

        public decimal Income { get; set; }

        public string Empty { get; set; }

        public string Spouse { get; set; }

        public string MaritalStatus { get; set; }

        public string OldIC { get; set; }

        public DateTime? DeathDate { get; set; }

        public string OccupationStatus { get; set; }

        public Spouse Wife { get; set; }
        public string Ward { get; set; }




        public override string ToString()
        {
            return "Patient:" + Mrn;
        }
    }
}