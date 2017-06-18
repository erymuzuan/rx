using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MyApp.ReceivePorts
{

    public class IpsExpc
    {
        public Dispatch Dispatch { get; set; }

        public string RecptclId { get; set; }

        public decimal ItemWeight { get; set; }

        public string ClassCd { get; set; }

        public string SubclassCd { get; set; }

        public string OrigCountryCd { get; set; }

        public string DestCountryCd { get; set; }

        public string PostalStatusFcd { get; set; }

        public Parcel Parcel { get; set; }

        public ObjectCollection<IPSEvent> IPSEvent { get; } = new ObjectCollection<IPSEvent>();



        //metadata
    }
}
