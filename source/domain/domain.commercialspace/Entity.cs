using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Station.Domain
{
    [XmlInclude(typeof(Pump))]
    [XmlInclude(typeof(DailySummary))]
    [XmlInclude(typeof(Tank))]
    [XmlInclude(typeof(Product))]
    [XmlInclude(typeof(Submission))]
    [XmlInclude(typeof(PumpSale))]
    [XmlInclude(typeof(Order))]
    [XmlInclude(typeof(Supplier))]
    [XmlInclude(typeof(Employee))]
    [XmlInclude(typeof(Shift))]
    [XmlInclude(typeof(ShiftSchedule))]
    [XmlInclude(typeof(Attendance))]
    [XmlInclude(typeof(LeaveSchedule))]
    [XmlInclude(typeof(Leave))]
    [XmlInclude(typeof(Salary))]
    [XmlInclude(typeof(Benefit))]
    [XmlInclude(typeof(Staff))]
    [XmlInclude(typeof(Sale))]
    [XmlInclude(typeof(Delivery))]
    [XmlInclude(typeof(Dipping))]
    [XmlInclude(typeof(DippingLookup))]
    [XmlInclude(typeof(Account))]
    [XmlInclude(typeof(Transaction))]
    [XmlInclude(typeof(PurchaseTransaction))]
    [XmlInclude(typeof(SaleTransaction))]
    [XmlInclude(typeof(Billing))]
    [XmlInclude(typeof(Setting))]
    [XmlInclude(typeof(LogEntry))]
    [XmlInclude(typeof(Inventory))]
    [XmlInclude(typeof(ChequeRegistry))]
    
    public abstract class Entity : DomainObject
    {
        [XmlAttribute]
        public string WebId { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public string CreatedBy { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public string ChangedBy { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public DateTime ChangedDate { get; set; }


    }
}
