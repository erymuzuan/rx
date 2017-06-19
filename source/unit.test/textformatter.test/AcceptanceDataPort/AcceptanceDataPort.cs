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


        }

        public IEnumerable<AcceptanceData> Process(IEnumerable<string> lines)
        {

            var text = string.Join("\r\n", lines);
            var doc = XElement.Parse(text);
            var xn = doc.GetDefaultNamespace();

            var elements = doc.Elements(xn + "AcceptanceData");
            foreach (var e in elements)
            {
                var record = new AcceptanceData
                {
                    TellerID = e.Element(xn + "TellerID")?.Value,
                    BranchCode = int.Parse(e.Element(xn + "BranchCode")?.Value ?? "10141003"),
                    TrxObject = new TrxObject
                    {

                        TrxID = e.Element(xn + "TrxObject")?.Element(xn + "TrxID")?.Value,
                        Status = e.Element(xn + "TrxObject")?.Element(xn + "Status")?.Value,
                        CancelReason = e.Element(xn + "TrxObject")?.Element(xn + "CancelReason")?.Value,
                        TrxDateTime = DateTime.Parse(e.Element(xn + "TrxObject")?.Element(xn + "TrxDateTime")?.Value ?? "5/6/2017 2:38:02 PM"),
                        MHLIndicator = e.Element(xn + "TrxObject")?.Element(xn + "MHLIndicator")?.Value,
                        MHLDate = e.Element(xn + "TrxObject")?.Element(xn + "MHLDate")?.Value,
                        PaymentType = e.Element(xn + "TrxObject")?.Element(xn + "PaymentType")?.Value,
                        PaymentMethod = e.Element(xn + "TrxObject")?.Element(xn + "PaymentMethod")?.Value,
                        AccountNumber = e.Element(xn + "TrxObject")?.Element(xn + "AccountNumber")?.Value,
                        PL9Number = e.Element(xn + "TrxObject")?.Element(xn + "PL9Number")?.Value,
                        RoundAdj = e.Element(xn + "TrxObject")?.Element(xn + "RoundAdj")?.Value
                    },
                    ConnoteObject = new ConnoteObject
                    {

                        ProductCode = e.Element(xn + "ConnoteObject")?.Element(xn + "ProductCode")?.Value,
                        ConnoteNumber = e.Element(xn + "ConnoteObject")?.Element(xn + "ConnoteNumber")?.Value,
                        DestinationCountry = e.Element(xn + "ConnoteObject")?.Element(xn + "DestinationCountry")?.Value,
                        Weight = e.Element(xn + "ConnoteObject")?.Element(xn + "Weight")?.Value.ParseNullableDecimal(),
                        Width = e.Element(xn + "ConnoteObject")?.Element(xn + "Width")?.Value,
                        Length = e.Element(xn + "ConnoteObject")?.Element(xn + "Length")?.Value,
                        Height = e.Element(xn + "ConnoteObject")?.Element(xn + "Height")?.Value,
                        DimWeight = int.Parse(e.Element(xn + "ConnoteObject")?.Element(xn + "DimWeight")?.Value ?? "0"),
                        ItemCategory = int.Parse(e.Element(xn + "ConnoteObject")?.Element(xn + "ItemCategory")?.Value ?? "09"),
                        RoutingCode = e.Element(xn + "ConnoteObject")?.Element(xn + "RoutingCode")?.Value,
                        StandardOfService = e.Element(xn + "ConnoteObject")?.Element(xn + "StandardOfService")?.Value
                    },
                    SenderObject = new SenderObject
                    {

                        Name = e.Element(xn + "SenderObject")?.Element(xn + "Name")?.Value,
                        Address1 = e.Element(xn + "SenderObject")?.Element(xn + "Address1")?.Value,
                        Address2 = e.Element(xn + "SenderObject")?.Element(xn + "Address2")?.Value,
                        Address3 = e.Element(xn + "SenderObject")?.Element(xn + "Address3")?.Value,
                        City = e.Element(xn + "SenderObject")?.Element(xn + "City")?.Value,
                        State = e.Element(xn + "SenderObject")?.Element(xn + "State")?.Value,
                        Country = e.Element(xn + "SenderObject")?.Element(xn + "Country")?.Value,
                        Postcode = int.Parse(e.Element(xn + "SenderObject")?.Element(xn + "Postcode")?.Value ?? "01000"),
                        Email = e.Element(xn + "SenderObject")?.Element(xn + "Email")?.Value,
                        PhoneNumber = e.Element(xn + "SenderObject")?.Element(xn + "PhoneNumber")?.Value,
                        ReferenceNumber = e.Element(xn + "SenderObject")?.Element(xn + "ReferenceNumber")?.Value
                    },
                    ReceiverObject = new ReceiverObject
                    {

                        Name = e.Element(xn + "ReceiverObject")?.Element(xn + "Name")?.Value,
                        Address1 = e.Element(xn + "ReceiverObject")?.Element(xn + "Address1")?.Value,
                        Address2 = e.Element(xn + "ReceiverObject")?.Element(xn + "Address2")?.Value,
                        Address3 = e.Element(xn + "ReceiverObject")?.Element(xn + "Address3")?.Value,
                        City = e.Element(xn + "ReceiverObject")?.Element(xn + "City")?.Value,
                        State = e.Element(xn + "ReceiverObject")?.Element(xn + "State")?.Value,
                        Country = e.Element(xn + "ReceiverObject")?.Element(xn + "Country")?.Value,
                        Postcode = int.Parse(e.Element(xn + "ReceiverObject")?.Element(xn + "Postcode")?.Value ?? "32040"),
                        Email = e.Element(xn + "ReceiverObject")?.Element(xn + "Email")?.Value,
                        PhoneNumber = e.Element(xn + "ReceiverObject")?.Element(xn + "PhoneNumber")?.Value,
                        ReferenceNumber = e.Element(xn + "ReceiverObject")?.Element(xn + "ReferenceNumber")?.Value
                    }


                };
                //TODO : AllowMultiple properties

                yield return record;
            }


        }

    }
}
