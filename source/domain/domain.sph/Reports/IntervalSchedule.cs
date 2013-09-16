using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(HourlySchedule))]
    [XmlInclude(typeof(WeeklySchedule))]
    [XmlInclude(typeof(DailySchedule))]
    [XmlInclude(typeof(MonthlySchedule))]
    public partial class IntervalSchedule : DomainObject
    {

    }

    public partial class WeeklySchedule : IntervalSchedule
    {
        
    }
    public partial class DailySchedule : IntervalSchedule
    {
        
    }
    public partial class HourlySchedule : IntervalSchedule
    {
        
    }
    public partial class MonthlySchedule : IntervalSchedule
    {
        
    }
}
