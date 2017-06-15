
// ====================== AcceptanceData ===========================
using System;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
// ReSharper disable InconsistentNaming


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



        //metadata
    }

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

    public class ConnoteObject
    {
        public string ProductCode { get; set; }

        public string ConnoteNumber { get; set; }

        public string DestinationCountry { get; set; }

        public decimal? Weight { get; set; }

        public string Width { get; set; }

        public string Length { get; set; }

        public string Height { get; set; }

        public int DimWeight { get; set; }

        public int ItemCategory { get; set; }

        public string RoutingCode { get; set; }

        public string StandardOfService { get; set; }



    }

    public class SenderObject
    {
        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int Postcode { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ReferenceNumber { get; set; }



    }

    public class ReceiverObject
    {
        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int Postcode { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ReferenceNumber { get; set; }



    }

    public class AcceptanceDataPort
    {
        public AcceptanceDataPort(ILogger logger) { this.Logger = logger; }


        public Uri Uri { get; set; }
        public ILogger Logger { get; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();


        public void AddHeader<T>(string name, T value)
        {
            this.Headers.Add(name, $"{value}");
        }
        private void ProcessHeader(AcceptanceData record)
        {


        }


        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public IEnumerable<AcceptanceData> Process(IEnumerable<string> lines)
        {

            var text = string.Join("\r\n", lines);
            var doc = XElement.Parse(text);

            //  var root = doc.Element("Data");
            var elements = doc.Elements("AcceptanceData");
            foreach (var e in elements)
            {
                var record = new AcceptanceData
                {
                    TellerID = e.Element("TellerID")?.Value,
                    BranchCode = int.Parse(e.Element("BranchCode")?.Value ?? "10141003"),
                    TrxObject = new TrxObject
                    {

                        TrxID = e.Element("TrxObject").Element("TrxID")?.Value,
                        Status = e.Element("TrxObject").Element("Status")?.Value,
                        CancelReason = e.Element("TrxObject").Element("CancelReason")?.Value,
                        TrxDateTime = DateTime.Parse(e.Element("TrxObject").Element("TrxDateTime")?.Value ?? "5/6/2017 2:38:02 PM"),
                        MHLIndicator = e.Element("TrxObject").Element("MHLIndicator")?.Value,
                        MHLDate = e.Element("TrxObject").Element("MHLDate")?.Value,
                        PaymentType = e.Element("TrxObject").Element("PaymentType")?.Value,
                        PaymentMethod = e.Element("TrxObject").Element("PaymentMethod")?.Value,
                        AccountNumber = e.Element("TrxObject").Element("AccountNumber")?.Value,
                        PL9Number = e.Element("TrxObject").Element("PL9Number")?.Value,
                        RoundAdj = e.Element("TrxObject").Element("RoundAdj")?.Value
                    },
                    ConnoteObject = new ConnoteObject
                    {

                        ProductCode = e.Element("ConnoteObject").Element("ProductCode")?.Value,
                        ConnoteNumber = e.Element("ConnoteObject").Element("ConnoteNumber")?.Value,
                        DestinationCountry = e.Element("ConnoteObject").Element("DestinationCountry")?.Value,
                        Weight = e.Element("ConnoteObject").Element("Weight")?.Value.ParseNullableDecimal(),
                        Width = e.Element("ConnoteObject").Element("Width")?.Value,
                        Length = e.Element("ConnoteObject").Element("Length")?.Value,
                        Height = e.Element("ConnoteObject").Element("Height")?.Value,
                        DimWeight = int.Parse(e.Element("ConnoteObject").Element("DimWeight")?.Value ?? "0"),
                        ItemCategory = int.Parse(e.Element("ConnoteObject").Element("ItemCategory")?.Value ?? "09"),
                        RoutingCode = e.Element("ConnoteObject").Element("RoutingCode")?.Value,
                        StandardOfService = e.Element("ConnoteObject").Element("StandardOfService")?.Value
                    },
                    SenderObject = new SenderObject
                    {

                        Name = e.Element("SenderObject").Element("Name")?.Value,
                        Address1 = e.Element("SenderObject").Element("Address1")?.Value,
                        Address2 = e.Element("SenderObject").Element("Address2")?.Value,
                        Address3 = e.Element("SenderObject").Element("Address3")?.Value,
                        City = e.Element("SenderObject").Element("City")?.Value,
                        State = e.Element("SenderObject").Element("State")?.Value,
                        Country = e.Element("SenderObject").Element("Country")?.Value,
                        Postcode = int.Parse(e.Element("SenderObject").Element("Postcode")?.Value ?? "01000"),
                        Email = e.Element("SenderObject").Element("Email")?.Value,
                        PhoneNumber = e.Element("SenderObject").Element("PhoneNumber")?.Value,
                        ReferenceNumber = e.Element("SenderObject").Element("ReferenceNumber")?.Value
                    },
                    ReceiverObject = new ReceiverObject
                    {

                        Name = e.Element("ReceiverObject").Element("Name")?.Value,
                        Address1 = e.Element("ReceiverObject").Element("Address1")?.Value,
                        Address2 = e.Element("ReceiverObject").Element("Address2")?.Value,
                        Address3 = e.Element("ReceiverObject").Element("Address3")?.Value,
                        City = e.Element("ReceiverObject").Element("City")?.Value,
                        State = e.Element("ReceiverObject").Element("State")?.Value,
                        Country = e.Element("ReceiverObject").Element("Country")?.Value,
                        Postcode = int.Parse(e.Element("ReceiverObject").Element("Postcode")?.Value ?? "32040"),
                        Email = e.Element("ReceiverObject").Element("Email")?.Value,
                        PhoneNumber = e.Element("ReceiverObject").Element("PhoneNumber")?.Value,
                        ReferenceNumber = e.Element("ReceiverObject").Element("ReferenceNumber")?.Value
                    }

                };
                var c1 = e.Element("ConnoteObject");
                record.ConnoteObject.ConnoteNumber = c1.Element("ConnoteNumber")?.Value;

                yield return record;
            }


        }

    }
}

