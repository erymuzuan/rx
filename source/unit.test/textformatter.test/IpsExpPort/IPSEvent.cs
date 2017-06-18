using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MyApp.ReceivePorts
{

    public class IPSEvent
    {
        public string TNCd { get; set; }

        public DateTime Date { get; set; }

        public string OfficeCd { get; set; }

        public string UserFid { get; set; }

        public string WorkstationFid { get; set; }

        public int ConditionCd { get; set; }

        public int RetentionReasonCd { get; set; }



    }
}
