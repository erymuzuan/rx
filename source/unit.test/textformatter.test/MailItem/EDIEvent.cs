using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MailItems.ReceivePorts
{

    public class EDIEvent
    {
        public string EDIMsg { get; set; }

        public string TNCd { get; set; }

        public int ConditionCd { get; set; }

        public DateTime LclDate { get; set; }

        public DateTime CaptureGMTDate { get; set; }

        public string SenderId { get; set; }

        public string Location { get; set; }

        public int RecipientsPostCode { get; set; }

        public string PlaceOfDestinationOfficeCd { get; set; }

        public string DespatchNumber { get; set; }

        public string DespatchOfficeCd { get; set; }

        public string AddresseeName { get; set; }

        public string DeliveryPointAddressLine1 { get; set; }



    }
}
