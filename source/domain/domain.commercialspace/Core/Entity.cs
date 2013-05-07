using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Station.Domain
{
    [XmlInclude(typeof(Alert))]
    [XmlInclude(typeof(Account))]
    [XmlInclude(typeof(Attendance))]
    [XmlInclude(typeof(Benefit))]
    [XmlInclude(typeof(Billing))]
    [XmlInclude(typeof(ChequeRegistry))]
    [XmlInclude(typeof(DailySummary))]
    [XmlInclude(typeof(Delivery))]
    [XmlInclude(typeof(Dipping))]
    [XmlInclude(typeof(DippingLookup))]
    [XmlInclude(typeof(Employee))]
    [XmlInclude(typeof(Inventory))]
    [XmlInclude(typeof(Leave))]
    [XmlInclude(typeof(LeaveSchedule))]
    [XmlInclude(typeof(LogEntry))]
    [XmlInclude(typeof(Order))]
    [XmlInclude(typeof(Payroll))]
    [XmlInclude(typeof(Product))]
    [XmlInclude(typeof(Pump))]
    [XmlInclude(typeof(PumpSale))]
    [XmlInclude(typeof(PumpTest))]
    [XmlInclude(typeof(PurchaseTransaction))]
    [XmlInclude(typeof(Reconciliation))]
    [XmlInclude(typeof(Salary))]
    [XmlInclude(typeof(Sale))]
    [XmlInclude(typeof(SaleTransaction))]
    [XmlInclude(typeof(Setting))]
    [XmlInclude(typeof(Shift))]
    [XmlInclude(typeof(ShiftSchedule))]
    [XmlInclude(typeof(Submission))]
    [XmlInclude(typeof(Supplier))]
    [XmlInclude(typeof(Staff))]
    [XmlInclude(typeof(Tank))]
    [XmlInclude(typeof(Transaction))]
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
