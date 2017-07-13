using System;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace Bespoke.MailItems.ReceivePorts
{
    public class MailItemPort
    {
        public MailItemPort(ILogger logger) { this.Logger = logger; }


        public Uri Uri { get; set; }
        public ILogger Logger { get; }
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();


        public void AddHeader<T>(string name, T value)
        {
            this.Headers.Add(name, $"{value}");
        }
        private void ProcessHeader(MailItem record)
        {


        }

        [SuppressMessage("ReSharper", "UseObjectOrCollectionInitializer")]
        public IEnumerable<MailItem> Process(IEnumerable<string> lines)
        {

            var text = string.Join("\r\n", lines);
            var doc = XElement.Parse(text);
            var xn = doc.GetDefaultNamespace();

            var elements = doc.Elements(xn + "MailItem");
            foreach (var e in elements)
            {
                var record = new MailItem();
                record.FromIPS = new FromIPS();

                record.FromIPS.LocalId = e.Element(xn + "FromIPS")?.Element(xn + "LocalId")?.Value;
                record.FromIPS.InnrbagId = e.Element(xn + "FromIPS")?.Element(xn + "InnrbagId")?.Value;
                record.FromIPS.RecptclId = e.Element(xn + "FromIPS")?.Element(xn + "RecptclId")?.Value;
                record.FromIPS.ItemWeight = decimal.Parse(e.Element(xn + "FromIPS")?.Element(xn + "ItemWeight")?.Value ?? "");
                record.FromIPS.Value = e.Element(xn + "FromIPS")?.Element(xn + "Value")?.Value;
                record.FromIPS.CurrencyCd = e.Element(xn + "FromIPS")?.Element(xn + "CurrencyCd")?.Value;
                record.FromIPS.DutiableInd = e.Element(xn + "FromIPS")?.Element(xn + "DutiableInd")?.Value;
                record.FromIPS.DutiableValue = e.Element(xn + "FromIPS")?.Element(xn + "DutiableValue")?.Value;
                record.FromIPS.CustomNo = e.Element(xn + "FromIPS")?.Element(xn + "CustomNo")?.Value;
                record.FromIPS.ClassCd = e.Element(xn + "FromIPS")?.Element(xn + "ClassCd")?.Value;
                record.FromIPS.SubclassCd = e.Element(xn + "FromIPS")?.Element(xn + "SubclassCd")?.Value;
                record.FromIPS.Content = e.Element(xn + "FromIPS")?.Element(xn + "Content")?.Value;
                record.FromIPS.OperatorCd = e.Element(xn + "FromIPS")?.Element(xn + "OperatorCd")?.Value;
                record.FromIPS.OrigCountryCd = e.Element(xn + "FromIPS")?.Element(xn + "OrigCountryCd")?.Value;
                record.FromIPS.DestCountryCd = e.Element(xn + "FromIPS")?.Element(xn + "DestCountryCd")?.Value;
                record.FromIPS.Misc4 = e.Element(xn + "FromIPS")?.Element(xn + "Misc4")?.Value;
                record.FromIPS.PostalStatusFcd = e.Element(xn + "FromIPS")?.Element(xn + "PostalStatusFcd")?.Value;
                record.FromIPS.PostagePaidValue = e.Element(xn + "FromIPS")?.Element(xn + "PostagePaidValue")?.Value;
                record.FromIPS.PostagePaidCurrencyCd = e.Element(xn + "FromIPS")?.Element(xn + "PostagePaidCurrencyCd")?.Value;
                record.FromIPS.AdditionalFeesValue = e.Element(xn + "FromIPS")?.Element(xn + "AdditionalFeesValue")?.Value;
                record.FromIPS.AdditionalFeesCurrencyCd = e.Element(xn + "FromIPS")?.Element(xn + "AdditionalFeesCurrencyCd")?.Value;
                record.FromIPS.CustomsTaxPId = e.Element(xn + "FromIPS")?.Element(xn + "CustomsTaxPId")?.Value;
                record.FromIPS.NetworkEntryLocationTypeCd = e.Element(xn + "FromIPS")?.Element(xn + "NetworkEntryLocationTypeCd")?.Value;
                record.FromIPS.MrsStatus = e.Element(xn + "FromIPS")?.Element(xn + "MrsStatus")?.Value;
                record.FromIPS.MrsExpirationDate = DateTime.Parse(e.Element(xn + "FromIPS")?.Element(xn + "MrsExpirationDate")?.Value ?? "");
                record.FromIPS.MrsOriginalId = e.Element(xn + "FromIPS")?.Element(xn + "MrsOriginalId")?.Value;

                foreach (var ce in e.Element("{http://upu.int/ips}FromIPS").Elements(xn + "IPSEvent"))
                {
                    var item = new IPSEvent();
                    item.TNCd = ce.Element(xn + "TNCd")?.Value;
                    item.Date = DateTime.Parse(ce.Element(xn + "Date")?.Value ?? "");
                    item.OfficeCd = ce.Element(xn + "OfficeCd")?.Value;
                    item.UserFid = ce.Element(xn + "UserFid")?.Value;
                    item.WorkstationFid = ce.Element(xn + "WorkstationFid")?.Value;
                    item.ConditionCd = ce.Element(xn + "ConditionCd")?.Value.ParseNullableInt32();
                    item.RetentionReasonCd = ce.Element(xn + "RetentionReasonCd")?.Value.ParseNullableInt32();


                    record.FromIPS.IPSEvent.Add(item);

                }

                record.FromIPS.Dispatch = new Dispatch();

                record.FromIPS.Dispatch.OrigOfficeCd = e.Element(xn + "FromIPS")?.Element(xn + "Dispatch")?.Element(xn + "OrigOfficeCd")?.Value;
                record.FromIPS.Dispatch.DestOfficeCd = e.Element(xn + "FromIPS")?.Element(xn + "Dispatch")?.Element(xn + "DestOfficeCd")?.Value;


                ;
                record.FromIPS.Letter = new Letter();

                record.FromIPS.Letter.ConveyanceTypeCd = e.Element(xn + "FromIPS")?.Element(xn + "Letter")?.Element(xn + "ConveyanceTypeCd")?.Value;
                record.FromIPS.Letter.MailCategoryCd = e.Element(xn + "FromIPS")?.Element(xn + "Letter")?.Element(xn + "MailCategoryCd")?.Value;
                record.FromIPS.Letter.MailItemCategoryCd = e.Element(xn + "FromIPS")?.Element(xn + "Letter")?.Element(xn + "MailItemCategoryCd")?.Value;
                record.FromIPS.Letter.ExpressInd = int.Parse(e.Element(xn + "FromIPS")?.Element(xn + "Letter")?.Element(xn + "ExpressInd")?.Value ?? "");
                record.FromIPS.Letter.CoDInd = int.Parse(e.Element(xn + "FromIPS")?.Element(xn + "Letter")?.Element(xn + "CoDInd")?.Value ?? "");
                record.FromIPS.Letter.LetterCharacteristicCd = e.Element(xn + "FromIPS")?.Element(xn + "Letter")?.Element(xn + "LetterCharacteristicCd")?.Value;


                ;
                record.FromIPS.Parcel = new Parcel();

                record.FromIPS.Parcel.ConveyanceTypeCd = e.Element(xn + "FromIPS")?.Element(xn + "Parcel")?.Element(xn + "ConveyanceTypeCd")?.Value;
                record.FromIPS.Parcel.MailCategoryCd = e.Element(xn + "FromIPS")?.Element(xn + "Parcel")?.Element(xn + "MailCategoryCd")?.Value;
                record.FromIPS.Parcel.MailItemCategoryCd = e.Element(xn + "FromIPS")?.Element(xn + "Parcel")?.Element(xn + "MailItemCategoryCd")?.Value;
                record.FromIPS.Parcel.ExpressInd = int.Parse(e.Element(xn + "FromIPS")?.Element(xn + "Parcel")?.Element(xn + "ExpressInd")?.Value ?? "");
                record.FromIPS.Parcel.CoDInd = int.Parse(e.Element(xn + "FromIPS")?.Element(xn + "Parcel")?.Element(xn + "CoDInd")?.Value ?? "");


                ;
                record.FromIPS.Addressee = new Addressee();

                record.FromIPS.Addressee.Name = e.Element(xn + "FromIPS")?.Element(xn + "Addressee")?.Element(xn + "Name")?.Value;
                record.FromIPS.Addressee.Address = e.Element(xn + "FromIPS")?.Element(xn + "Addressee")?.Element(xn + "Address")?.Value;
                record.FromIPS.Addressee.City = e.Element(xn + "FromIPS")?.Element(xn + "Addressee")?.Element(xn + "City")?.Value;
                record.FromIPS.Addressee.Postcode = int.Parse(e.Element(xn + "FromIPS")?.Element(xn + "Addressee")?.Element(xn + "Postcode")?.Value ?? "");
                record.FromIPS.Addressee.CountryCode = e.Element(xn + "FromIPS")?.Element(xn + "Addressee")?.Element(xn + "CountryCode")?.Value;


                ;
                record.FromIPS.Sender = new Sender();

                record.FromIPS.Sender.Name = e.Element(xn + "FromIPS")?.Element(xn + "Sender")?.Element(xn + "Name")?.Value;
                record.FromIPS.Sender.Address = e.Element(xn + "FromIPS")?.Element(xn + "Sender")?.Element(xn + "Address")?.Value;
                record.FromIPS.Sender.City = e.Element(xn + "FromIPS")?.Element(xn + "Sender")?.Element(xn + "City")?.Value;
                record.FromIPS.Sender.Postcode = int.Parse(e.Element(xn + "FromIPS")?.Element(xn + "Sender")?.Element(xn + "Postcode")?.Value ?? "");
                record.FromIPS.Sender.CountryCode = e.Element(xn + "FromIPS")?.Element(xn + "Sender")?.Element(xn + "CountryCode")?.Value;


                record.FromEDI = e.Element(xn + "FromEDI") == null ? default(FromEDI) : new FromEDI();
                if (null != record.FromEDI)
                {

                    record.FromEDI.RecptclId = e.Element(xn + "FromEDI")?.Element(xn + "RecptclId")?.Value;
                    record.FromEDI.ItemWeight = decimal.Parse(e.Element(xn + "FromEDI")?.Element(xn + "ItemWeight")?.Value ?? "");
                    record.FromEDI.DestCountryCd = e.Element(xn + "FromEDI")?.Element(xn + "DestCountryCd")?.Value;

                }
                record.ItemId = e.Attribute("ItemId")?.Value;
                record.InterfaceCode = doc.Attribute("InterfaceCode")?.Value;

                // AllowMultiple properties

                this.ProcessHeader(record);

                yield return record;
            }


        }

    }
}
