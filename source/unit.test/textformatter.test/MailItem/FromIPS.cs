using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MailItems.ReceivePorts
{

    public class FromIPS
    {
        public string LocalId { get; set; }

        public Dispatch Dispatch { get; set; }

        public string InnrbagId { get; set; }

        public string RecptclId { get; set; }

        public decimal ItemWeight { get; set; }

        public string Value { get; set; }

        public string CurrencyCd { get; set; }

        public string DutiableInd { get; set; }

        public string DutiableValue { get; set; }

        public string CustomNo { get; set; }

        public string ClassCd { get; set; }

        public string SubclassCd { get; set; }

        public string Content { get; set; }

        public string OperatorCd { get; set; }

        public string OrigCountryCd { get; set; }

        public string DestCountryCd { get; set; }

        public string Misc4 { get; set; }

        public string PostalStatusFcd { get; set; }

        public string PostagePaidValue { get; set; }

        public string PostagePaidCurrencyCd { get; set; }

        public string AdditionalFeesValue { get; set; }

        public string AdditionalFeesCurrencyCd { get; set; }

        public string CustomsTaxPId { get; set; }

        public string NetworkEntryLocationTypeCd { get; set; }

        public string MrsStatus { get; set; }

        public DateTime MrsExpirationDate { get; set; }

        public string MrsOriginalId { get; set; }

        public Letter Letter { get; set; }

        public Parcel Parcel { get; set; }

        public Addressee Addressee { get; set; }

        public Sender Sender { get; set; }

        public ObjectCollection<IPSEvent> IPSEvent { get; } = new ObjectCollection<IPSEvent>();



    }
}
