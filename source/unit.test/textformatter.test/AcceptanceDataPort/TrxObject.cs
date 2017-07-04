using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MyApp.ReceivePorts
{

    public class TrxObject
    {
        public string TrxID { get; set; }

        public string Status { get; set; }

        public string CancelReason { get; set; }

        public DateTime TrxDateTime { get; set; }

        public string MHLIndicator { get; set; }

        public string MHLDate { get; set; }

        public string PaymentType { get; set; }

        public string PaymentMethod { get; set; }

        public string AccountNumber { get; set; }

        public string PL9Number { get; set; }

        public string RoundAdj { get; set; }



    }
}
