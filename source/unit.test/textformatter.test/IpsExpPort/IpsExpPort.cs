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
                record.Dispatch = new Dispatch();

                record.Dispatch.OrigOfficeCd = e.Element(xn + "Dispatch")?.Element(xn + "OrigOfficeCd")?.Value;
                record.Dispatch.DestOfficeCd = e.Element(xn + "Dispatch")?.Element(xn + "DestOfficeCd")?.Value;


                ;
                record.RecptclId = e.Element(xn + "RecptclId")?.Value;
                record.ItemWeight = decimal.Parse(e.Element(xn + "ItemWeight")?.Value ?? "");
                record.ClassCd = e.Element(xn + "ClassCd")?.Value;
                record.SubclassCd = e.Element(xn + "SubclassCd")?.Value;
                record.OrigCountryCd = e.Element(xn + "OrigCountryCd")?.Value;
                record.DestCountryCd = e.Element(xn + "DestCountryCd")?.Value;
                record.PostalStatusFcd = e.Element(xn + "PostalStatusFcd")?.Value;
                record.Parcel = new Parcel();

                record.Parcel.ConveyanceTypeCd = e.Element(xn + "Parcel")?.Element(xn + "ConveyanceTypeCd")?.Value;
                record.Parcel.MailCategoryCd = e.Element(xn + "Parcel")?.Element(xn + "MailCategoryCd")?.Value;
                record.Parcel.MailItemCategoryCd = e.Element(xn + "Parcel")?.Element(xn + "MailItemCategoryCd")?.Value;
                record.Parcel.ExpressInd = e.Element(xn + "Parcel")?.Element(xn + "ExpressInd")?.Value.ParseNullableInt32();
                record.Parcel.CoDInd = e.Element(xn + "Parcel")?.Element(xn + "CoDInd")?.Value.ParseNullableInt32();


                ;
                record.ItemId = e.Parent.Attribute("ItemId")?.Value;
                record.InterfaceCode = doc.Attribute("InterfaceCode")?.Value;

                // AllowMultiple properties
                foreach (var ce in e.Elements("{http://upu.int/ips}IPSEvent"))
                {
                    var item = new IPSEvent();
                    item.TNCd = ce.Element(xn + "TNCd")?.Value;
                    item.Date = ce.Element(xn + "Date")?.Value.ParseNullableDateTime();
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
