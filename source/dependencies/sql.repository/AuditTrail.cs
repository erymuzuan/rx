//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bespoke.Sph.SqlRepository
{
    using System;
    using System.Collections.Generic;
    
    public partial class AuditTrail
    {
        public int AuditTrailId { get; set; }
        public string Data { get; set; }
        public string User { get; set; }
        public System.DateTime DateTime { get; set; }
        public string Operation { get; set; }
        public string Type { get; set; }
        public string EntityId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime ChangedDate { get; set; }
        public string ChangedBy { get; set; }
    }
}
