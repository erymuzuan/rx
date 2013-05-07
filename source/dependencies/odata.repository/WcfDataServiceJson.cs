
// ReSharper disable InconsistentNaming
namespace Bespoke.Station.OdataRepository
{
    internal class RepositoryJsonData
    {
        public int __count { get; set; }
        public _result[] __results { get; set; }
    }

    class WcfDataServiceJson
    {
        public _d d { get; set; }
    }

    internal class _d
    {
        public _result[] results { get; set; }
        public int __count { get; set; }
        public string __next{ get; set; }
    }

    internal class _result
    {
        public __metadata __metadata { get; set; }
        public string Data { get; set; }
        public string __webid { get; set; }
        public int __id{ get; set; }
    }

    internal class __metadata
    {
        public string type { get; set; }
        public string uri { get; set; }
    }
}

// ReSharper restore InconsistentNaming