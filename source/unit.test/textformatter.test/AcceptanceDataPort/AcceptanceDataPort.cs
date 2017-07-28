using System;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Bespoke.MyApp.ReceivePorts
{
    public class AcceptanceDataPort
    {
        public AcceptanceDataPort(ILogger logger) { this.Logger = logger; }


        public System.Uri Uri { get; set; }
        public ILogger Logger { get; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();


        public void AddHeader<T>(string name, T value)
        {
            this.Headers.Add(name, $"{value}");
        }
        private void ProcessHeader(AcceptanceData record)
        {

            // Uri: Date
            var dateRaw = Strings.RegexSingleValue(this.Uri.ToString(), @"AcceptanceData(?<value>[0-9]{8})[0-9]{6}.xml", "value");
            record.Date = System.DateTime.ParseExact(dateRaw, @"yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);


        }

        public IEnumerable<AcceptanceData> Process(IEnumerable<string> lines)
        {

            var text = string.Join("\r\n", lines);
            var doc = XElement.Parse(text);
            var xn = doc.GetDefaultNamespace();

            var elements = doc.Elements(xn + "AcceptanceData");
            foreach (var e in elements)
            {
                var record = new AcceptanceData();
                record.TellerID = e.Element(xn + "TellerID")?.Value;
                record.BranchCode = int.Parse(e.Element(xn + "BranchCode")?.Value ?? "");
                record.TrxObject = new TrxObject();

                record.TrxObject.TrxID = e.Element(xn + "TrxObject")?.Element(xn + "TrxID")?.Value;
                record.TrxObject.Status = e.Element(xn + "TrxObject")?.Element(xn + "Status")?.Value;
                record.TrxObject.CancelReason = e.Element(xn + "TrxObject")?.Element(xn + "CancelReason")?.Value;
                record.TrxObject.TrxDateTime = System.DateTime.ParseExact(e.Element(xn + "TrxObject")?.Element(xn + "TrxDateTime")?.Value, @"d/M/yyyy h:m:ss tt", System.Globalization.CultureInfo.InvariantCulture); ;
                record.TrxObject.MHLIndicator = e.Element(xn + "TrxObject")?.Element(xn + "MHLIndicator")?.Value;
                record.TrxObject.MHLDate = e.Element(xn + "TrxObject")?.Element(xn + "MHLDate")?.Value;
                record.TrxObject.PaymentType = e.Element(xn + "TrxObject")?.Element(xn + "PaymentType")?.Value;
                record.TrxObject.PaymentMethod = e.Element(xn + "TrxObject")?.Element(xn + "PaymentMethod")?.Value;
                record.TrxObject.AccountNumber = e.Element(xn + "TrxObject")?.Element(xn + "AccountNumber")?.Value;
                record.TrxObject.PL9Number = e.Element(xn + "TrxObject")?.Element(xn + "PL9Number")?.Value;
                record.TrxObject.RoundAdj = e.Element(xn + "TrxObject")?.Element(xn + "RoundAdj")?.Value;


                ;
                record.ConnoteObject = new ConnoteObject();

                record.ConnoteObject.ProductCode = e.Element(xn + "ConnoteObject")?.Element(xn + "ProductCode")?.Value;
                record.ConnoteObject.ConnoteNumber = e.Element(xn + "ConnoteObject")?.Element(xn + "ConnoteNumber")?.Value;
                record.ConnoteObject.DestinationCountry = e.Element(xn + "ConnoteObject")?.Element(xn + "DestinationCountry")?.Value;
                record.ConnoteObject.Weight = e.Element(xn + "ConnoteObject")?.Element(xn + "Weight")?.Value.ParseNullableDecimal();
                record.ConnoteObject.Width = e.Element(xn + "ConnoteObject")?.Element(xn + "Width")?.Value;
                record.ConnoteObject.Length = e.Element(xn + "ConnoteObject")?.Element(xn + "Length")?.Value;
                record.ConnoteObject.Height = e.Element(xn + "ConnoteObject")?.Element(xn + "Height")?.Value;
                record.ConnoteObject.DimWeight = int.Parse(e.Element(xn + "ConnoteObject")?.Element(xn + "DimWeight")?.Value ?? "");
                record.ConnoteObject.ItemCategory = int.Parse(e.Element(xn + "ConnoteObject")?.Element(xn + "ItemCategory")?.Value ?? "");
                record.ConnoteObject.RoutingCode = e.Element(xn + "ConnoteObject")?.Element(xn + "RoutingCode")?.Value;
                record.ConnoteObject.StandardOfService = e.Element(xn + "ConnoteObject")?.Element(xn + "StandardOfService")?.Value;


                ;
                record.SenderObject = new SenderObject();

                record.SenderObject.Name = e.Element(xn + "SenderObject")?.Element(xn + "Name")?.Value;
                record.SenderObject.Address1 = e.Element(xn + "SenderObject")?.Element(xn + "Address1")?.Value;
                record.SenderObject.Address2 = e.Element(xn + "SenderObject")?.Element(xn + "Address2")?.Value;
                record.SenderObject.Address3 = e.Element(xn + "SenderObject")?.Element(xn + "Address3")?.Value;
                record.SenderObject.City = e.Element(xn + "SenderObject")?.Element(xn + "City")?.Value;
                record.SenderObject.State = e.Element(xn + "SenderObject")?.Element(xn + "State")?.Value;
                record.SenderObject.Country = e.Element(xn + "SenderObject")?.Element(xn + "Country")?.Value;
                record.SenderObject.Postcode = int.Parse(e.Element(xn + "SenderObject")?.Element(xn + "Postcode")?.Value ?? "");
                record.SenderObject.Email = e.Element(xn + "SenderObject")?.Element(xn + "Email")?.Value;
                record.SenderObject.PhoneNumber = e.Element(xn + "SenderObject")?.Element(xn + "PhoneNumber")?.Value;
                record.SenderObject.ReferenceNumber = e.Element(xn + "SenderObject")?.Element(xn + "ReferenceNumber")?.Value;


                ;
                record.ReceiverObject = new ReceiverObject();

                record.ReceiverObject.Name = e.Element(xn + "ReceiverObject")?.Element(xn + "Name")?.Value;
                record.ReceiverObject.Address1 = e.Element(xn + "ReceiverObject")?.Element(xn + "Address1")?.Value;
                record.ReceiverObject.Address2 = e.Element(xn + "ReceiverObject")?.Element(xn + "Address2")?.Value;
                record.ReceiverObject.Address3 = e.Element(xn + "ReceiverObject")?.Element(xn + "Address3")?.Value;
                record.ReceiverObject.City = e.Element(xn + "ReceiverObject")?.Element(xn + "City")?.Value;
                record.ReceiverObject.State = e.Element(xn + "ReceiverObject")?.Element(xn + "State")?.Value;
                record.ReceiverObject.Country = e.Element(xn + "ReceiverObject")?.Element(xn + "Country")?.Value;
                record.ReceiverObject.Postcode = int.Parse(e.Element(xn + "ReceiverObject")?.Element(xn + "Postcode")?.Value ?? "");
                record.ReceiverObject.Email = e.Element(xn + "ReceiverObject")?.Element(xn + "Email")?.Value;
                record.ReceiverObject.PhoneNumber = e.Element(xn + "ReceiverObject")?.Element(xn + "PhoneNumber")?.Value;
                record.ReceiverObject.ReferenceNumber = e.Element(xn + "ReceiverObject")?.Element(xn + "ReferenceNumber")?.Value;


                ;


                // AllowMultiple properties

                this.ProcessHeader(record);

                yield return record;
            }


        }

    }
}
