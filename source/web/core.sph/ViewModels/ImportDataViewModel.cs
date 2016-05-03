using System;

namespace Bespoke.Sph.Web.ViewModels
{
    public class ImportDataViewModel
    {
        public string Adapter { get; set; }
        public string Map { get; set; }
        public string Sql { get; set; }
        public string Table { get; set; }
        public string Entity { get; set; }
        public int BatchSize { get; set; } = 40;
        public string Name { get; set; }
        public int? DelayThrottle { get; set; }
    }
}