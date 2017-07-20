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
                var record = new IpsExpc();
                record.LocalId = e.Element(xn + "LocalId")?.Value;
                record.Dispatch = new Dispatch();

                record.Dispatch.OrigOfficeCd = e.Element(xn + "Dispatch")?.Element(xn + "OrigOfficeCd")?.Value;
                record.Dispatch.DestOfficeCd = e.Element(xn + "Dispatch")?.Element(xn + "DestOfficeCd")?.Value;


                ;
                record.InnrbagId = e.Element(xn + "InnrbagId")?.Value;
                record.RecptclId = e.Element(xn + "RecptclId")?.Value;
                record.ItemWeight = decimal.Parse(e.Element(xn + "ItemWeight")?.Value ?? "");
                record.Value = e.Element(xn + "Value")?.Value;
                record.CurrencyCd = e.Element(xn + "CurrencyCd")?.Value;
                record.DutiableInd = e.Element(xn + "DutiableInd")?.Value;
                record.DutiableValue = e.Element(xn + "DutiableValue")?.Value;
                record.CustomNo = e.Element(xn + "CustomNo")?.Value;
                record.ClassCd = e.Element(xn + "ClassCd")?.Value;
                record.SubclassCd = e.Element(xn + "SubclassCd")?.Value;
                record.Content = e.Element(xn + "Content")?.Value;
                record.OperatorCd = e.Element(xn + "OperatorCd")?.Value;
                record.OrigCountryCd = e.Element(xn + "OrigCountryCd")?.Value;
                record.DestCountryCd = e.Element(xn + "DestCountryCd")?.Value;
                record.Misc4 = e.Element(xn + "Misc4")?.Value;
                record.PostalStatusFcd = e.Element(xn + "PostalStatusFcd")?.Value;
                record.PostagePaidValue = e.Element(xn + "PostagePaidValue")?.Value;
                record.PostagePaidCurrencyCd = e.Element(xn + "PostagePaidCurrencyCd")?.Value;
                record.AdditionalFeesValue = e.Element(xn + "AdditionalFeesValue")?.Value;
                record.AdditionalFeesCurrencyCd = e.Element(xn + "AdditionalFeesCurrencyCd")?.Value;
                record.CustomsTaxPId = e.Element(xn + "CustomsTaxPId")?.Value;
                record.NetworkEntryLocationTypeCd = e.Element(xn + "NetworkEntryLocationTypeCd")?.Value;
                record.MrsStatus = e.Element(xn + "MrsStatus")?.Value;
                record.MrsExpirationDate = DateTime.Parse(e.Element(xn + "MrsExpirationDate")?.Value ?? "");
                record.MrsOriginalId = e.Element(xn + "MrsOriginalId")?.Value;
                record.Letter = new Letter();

                record.Letter.ConveyanceTypeCd = e.Element(xn + "Letter")?.Element(xn + "ConveyanceTypeCd")?.Value;
                record.Letter.MailCategoryCd = e.Element(xn + "Letter")?.Element(xn + "MailCategoryCd")?.Value;
                record.Letter.MailItemCategoryCd = e.Element(xn + "Letter")?.Element(xn + "MailItemCategoryCd")?.Value;
                record.Letter.ExpressInd = int.Parse(e.Element(xn + "Letter")?.Element(xn + "ExpressInd")?.Value ?? "");
                record.Letter.CoDInd = int.Parse(e.Element(xn + "Letter")?.Element(xn + "CoDInd")?.Value ?? "");
                record.Letter.LetterCharacteristicCd = e.Element(xn + "Letter")?.Element(xn + "LetterCharacteristicCd")?.Value;


                ;
                record.Parcel = new Parcel();

                record.Parcel.ConveyanceTypeCd = e.Element(xn + "Parcel")?.Element(xn + "ConveyanceTypeCd")?.Value;
                record.Parcel.MailCategoryCd = e.Element(xn + "Parcel")?.Element(xn + "MailCategoryCd")?.Value;
                record.Parcel.MailItemCategoryCd = e.Element(xn + "Parcel")?.Element(xn + "MailItemCategoryCd")?.Value;
                record.Parcel.ExpressInd = int.Parse(e.Element(xn + "Parcel")?.Element(xn + "ExpressInd")?.Value ?? "");
                record.Parcel.CoDInd = int.Parse(e.Element(xn + "Parcel")?.Element(xn + "CoDInd")?.Value ?? "");


                ;
                record.Addressee = new Addressee();

                record.Addressee.Name = e.Element(xn + "Addressee")?.Element(xn + "Name")?.Value;
                record.Addressee.Address = e.Element(xn + "Addressee")?.Element(xn + "Address")?.Value;
                record.Addressee.City = e.Element(xn + "Addressee")?.Element(xn + "City")?.Value;
                record.Addressee.Postcode = int.Parse(e.Element(xn + "Addressee")?.Element(xn + "Postcode")?.Value ?? "");
                record.Addressee.CountryCode = e.Element(xn + "Addressee")?.Element(xn + "CountryCode")?.Value;


                ;
                record.Sender = new Sender();

                record.Sender.Name = e.Element(xn + "Sender")?.Element(xn + "Name")?.Value;
                record.Sender.Address = e.Element(xn + "Sender")?.Element(xn + "Address")?.Value;
                record.Sender.City = e.Element(xn + "Sender")?.Element(xn + "City")?.Value;
                record.Sender.Postcode = int.Parse(e.Element(xn + "Sender")?.Element(xn + "Postcode")?.Value ?? "");
                record.Sender.CountryCode = e.Element(xn + "Sender")?.Element(xn + "CountryCode")?.Value;


                ;
                record.ItemId = e.Parent.Attribute("ItemId")?.Value;
                record.InterfaceCode = doc.Attribute("InterfaceCode")?.Value;

                // AllowMultiple properties
                foreach (var ce in e.Elements("{http://upu.int/ips}IPSEvent"))
                {
                    var item = new IPSEvent();
                    item.TNCd = ce.Element(xn + "TNCd")?.Value;
                    item.Date = DateTime.Parse(ce.Element(xn + "Date")?.Value ?? "");
                    item.OfficeCd = ce.Element(xn + "OfficeCd")?.Value;
                    item.UserFid = ce.Element(xn + "UserFid")?.Value;
                    item.WorkstationFid = ce.Element(xn + "WorkstationFid")?.Value;
                    item.ConditionCd = ce.Element(xn + "ConditionCd")?.Value.ParseNullableInt32();
                    item.RetentionReasonCd = ce.Element(xn + "RetentionReasonCd")?.Value.ParseNullableInt32();
                    record.IPSEvent.Add(item);
                }

                this.ProcessHeader(record);

                yield return record;
            }


        }

    }
}
