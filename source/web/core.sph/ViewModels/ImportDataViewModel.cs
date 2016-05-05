namespace Bespoke.Sph.Web.ViewModels
{
    public class ImportDataViewModel
    {
        public string Name { get; set; }
        public string Adapter { get; set; }
        public string Map { get; set; }
        public string Sql { get; set; }
        public string Table { get; set; }
        public string Entity { get; set; }
        public int BatchSize { get; set; } = 40;
        public int? DelayThrottle { get; set; }
        public int? SqlRetry { get; set; }
        public int? SqlWait { get; set; }
        public int? EsRetry { get; set; }
        public int? EsWait { get; set; }
        public bool IgnoreMessaging { get; set; }
    }
}