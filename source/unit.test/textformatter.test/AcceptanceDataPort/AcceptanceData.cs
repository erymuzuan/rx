using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MyApp.ReceivePorts
{

    public class AcceptanceData
    {
        public string TellerID { get; set; }

        public int BranchCode { get; set; }

        public TrxObject TrxObject { get; set; }

        public ConnoteObject ConnoteObject { get; set; }

        public SenderObject SenderObject { get; set; }

        public ReceiverObject ReceiverObject { get; set; }

               public DateTime? Date;


        //metadata
    }
}
