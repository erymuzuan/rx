using System;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Bespoke.IpsExports.ReceivePorts
{
    public class IpsExpPort
    {
        public IpsExpPort(ILogger logger) { this.Logger = logger; }


        public System.Uri Uri { get; set; }
        public ILogger Logger { get; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();


        public void AddHeader<T>(string name, T value)
        {
            this.Headers.Add(name, $"{value}");
        }
        private void ProcessHeader(IpsExpc record)
        {


        }

        public IEnumerable<IpsExpc> Process(IEnumerable<string> lines)
        {

            var text = string.Join("\r\n", lines);
            var doc = XElement.Parse(text);
            var xn = doc.GetDefaultNamespace();

            var elements = doc.Elements(xn + "MailItem").Elements(xn + "FromIPS");
            foreach (var e in elements)
            {
                var record = new IpsExpc
                {
                    LocalId = e.Element(xn + "LocalId")?.Value,
                    Dispatch = new Dispatch
                    {

                        OrigOfficeCd = e.Element(xn + "Dispatch")?.Element(xn + "OrigOfficeCd")?.Value,
                        DestOfficeCd = e.Element(xn + "Dispatch")?.Element(xn + "DestOfficeCd")?.Value
                    },
                    InnrbagId = e.Element(xn + "InnrbagId")?.Value,
                    RecptclId = e.Element(xn + "RecptclId")?.Value,
                    ItemWeight = decimal.Parse(e.Element(xn + "ItemWeight")?.Value ?? ""),
                    Value = e.Element(xn + "Value")?.Value,
                    CurrencyCd = e.Element(xn + "CurrencyCd")?.Value,
                    DutiableInd = e.Element(xn + "DutiableInd")?.Value,
                    DutiableValue = e.Element(xn + "DutiableValue")?.Value,
                    CustomNo = e.Element(xn + "CustomNo")?.Value,
                    ClassCd = e.Element(xn + "ClassCd")?.Value,
                    SubclassCd = e.Element(xn + "SubclassCd")?.Value,
                    Content = e.Element(xn + "Content")?.Value,
                    OperatorCd = e.Element(xn + "OperatorCd")?.Value,
                    OrigCountryCd = e.Element(xn + "OrigCountryCd")?.Value,
                    DestCountryCd = e.Element(xn + "DestCountryCd")?.Value,
                    Misc4 = e.Element(xn + "Misc4")?.Value,
                    PostalStatusFcd = e.Element(xn + "PostalStatusFcd")?.Value,
                    PostagePaidValue = e.Element(xn + "PostagePaidValue")?.Value,
                    PostagePaidCurrencyCd = e.Element(xn + "PostagePaidCurrencyCd")?.Value,
                    AdditionalFeesValue = e.Element(xn + "AdditionalFeesValue")?.Value,
                    AdditionalFeesCurrencyCd = e.Element(xn + "AdditionalFeesCurrencyCd")?.Value,
                    CustomsTaxPId = e.Element(xn + "CustomsTaxPId")?.Value,
                    NetworkEntryLocationTypeCd = e.Element(xn + "NetworkEntryLocationTypeCd")?.Value,
                    MrsStatus = e.Element(xn + "MrsStatus")?.Value,
                    MrsExpirationDate = DateTime.Parse(e.Element(xn + "MrsExpirationDate")?.Value ?? ""),
                    MrsOriginalId = e.Element(xn + "MrsOriginalId")?.Value,
                    Letter = new Letter
                    {

                        ConveyanceTypeCd = e.Element(xn + "Letter")?.Element(xn + "ConveyanceTypeCd")?.Value,
                        MailCategoryCd = e.Element(xn + "Letter")?.Element(xn + "MailCategoryCd")?.Value,
                        MailItemCategoryCd = e.Element(xn + "Letter")?.Element(xn + "MailItemCategoryCd")?.Value,
                        ExpressInd = int.Parse(e.Element(xn + "Letter")?.Element(xn + "ExpressInd")?.Value ?? ""),
                        CoDInd = int.Parse(e.Element(xn + "Letter")?.Element(xn + "CoDInd")?.Value ?? ""),
                        LetterCharacteristicCd = e.Element(xn + "Letter")?.Element(xn + "LetterCharacteristicCd")?.Value
                    },
                    Parcel = new Parcel
                    {

                        ConveyanceTypeCd = e.Element(xn + "Parcel")?.Element(xn + "ConveyanceTypeCd")?.Value,
                        MailCategoryCd = e.Element(xn + "Parcel")?.Element(xn + "MailCategoryCd")?.Value,
                        MailItemCategoryCd = e.Element(xn + "Parcel")?.Element(xn + "MailItemCategoryCd")?.Value,
                        ExpressInd = int.Parse(e.Element(xn + "Parcel")?.Element(xn + "ExpressInd")?.Value ?? ""),
                        CoDInd = int.Parse(e.Element(xn + "Parcel")?.Element(xn + "CoDInd")?.Value ?? "")
                    },
                    Addressee = new Addressee
                    {

                        Name = e.Element(xn + "Addressee")?.Element(xn + "Name")?.Value,
                        Address = e.Element(xn + "Addressee")?.Element(xn + "Address")?.Value,
                        City = e.Element(xn + "Addressee")?.Element(xn + "City")?.Value,
                        Postcode = int.Parse(e.Element(xn + "Addressee")?.Element(xn + "Postcode")?.Value ?? ""),
                        CountryCode = e.Element(xn + "Addressee")?.Element(xn + "CountryCode")?.Value
                    },
                    Sender = new Sender
                    {

                        Name = e.Element(xn + "Sender")?.Element(xn + "Name")?.Value,
                        Address = e.Element(xn + "Sender")?.Element(xn + "Address")?.Value,
                        City = e.Element(xn + "Sender")?.Element(xn + "City")?.Value,
                        Postcode = int.Parse(e.Element(xn + "Sender")?.Element(xn + "Postcode")?.Value ?? ""),
                        CountryCode = e.Element(xn + "Sender")?.Element(xn + "CountryCode")?.Value
                    },
                    ItemId = e.Parent.Attribute("ItemId")?.Value,
                    InterfaceCode = doc.Attribute("InterfaceCode")?.Value

                };

                // AllowMultiple properties
                foreach (var ce in e.Elements(xn + "IPSEvent"))
                {
                    record.IPSEvent.Add(new IPSEvent
                    {
                        TNCd = ce.Element(xn + "TNCd")?.Value,
                        Date = DateTime.Parse(ce.Element(xn + "Date")?.Value ?? ""),
                        OfficeCd = ce.Element(xn + "OfficeCd")?.Value,
                        UserFid = ce.Element(xn + "UserFid")?.Value,
                        WorkstationFid = ce.Element(xn + "WorkstationFid")?.Value,
                        ConditionCd = ce.Element(xn + "ConditionCd")?.Value.ParseNullableInt32(),
                        RetentionReasonCd = ce.Element(xn + "RetentionReasonCd")?.Value.ParseNullableInt32()
                    });
                }

                this.ProcessHeader(record);

                yield return record;
            }


        }

    }
}
