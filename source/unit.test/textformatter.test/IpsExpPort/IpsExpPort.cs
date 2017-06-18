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
                var record = new IpsExpc
                {
                    Dispatch = new Dispatch
                    {

                        OrigOfficeCd = e.Element(xn + "Dispatch")?.Element(xn + "OrigOfficeCd")?.Value,
                        DestOfficeCd = e.Element(xn + "Dispatch")?.Element(xn + "DestOfficeCd")?.Value
                    },
                    RecptclId = e.Element(xn + "RecptclId")?.Value,
                    ItemWeight = decimal.Parse(e.Element(xn + "ItemWeight")?.Value ?? "3.460"),
                    ClassCd = e.Element(xn + "ClassCd")?.Value,
                    SubclassCd = e.Element(xn + "SubclassCd")?.Value,
                    OrigCountryCd = e.Element(xn + "OrigCountryCd")?.Value,
                    DestCountryCd = e.Element(xn + "DestCountryCd")?.Value,
                    PostalStatusFcd = e.Element(xn + "PostalStatusFcd")?.Value,
                    Parcel = new Parcel
                    {

                        ConveyanceTypeCd = e.Element(xn + "Parcel")?.Element(xn + "ConveyanceTypeCd")?.Value,
                        MailCategoryCd = e.Element(xn + "Parcel")?.Element(xn + "MailCategoryCd")?.Value,
                        MailItemCategoryCd = e.Element(xn + "Parcel")?.Element(xn + "MailItemCategoryCd")?.Value,
                        ExpressInd = int.Parse(e.Element(xn + "Parcel")?.Element(xn + "ExpressInd")?.Value ?? "0"),
                        CoDInd = int.Parse(e.Element(xn + "Parcel")?.Element(xn + "CoDInd")?.Value ?? "0")
                    },
                    ItemId = e.Parent.Attribute("ItemId")?.Value,
                    InterfaceCode = doc.Attribute("InterfaceCode")?.Value

                };
                //TODO : AllowMultiple properties
                foreach (var ce in e.Elements(xn + "IPSEvent"))
                {
                    record.IPSEvent.Add(new IPSEvent
                    {
                        TNCd = ce.Element(xn + "TNCd")?.Value,
                        Date = DateTime.Parse(ce.Element(xn + "Date")?.Value ?? "2017-06-06T01:05:43.483"),
                        OfficeCd = ce.Element(xn + "OfficeCd")?.Value,
                        UserFid = ce.Element(xn + "UserFid")?.Value,
                        WorkstationFid = ce.Element(xn + "WorkstationFid")?.Value,
                        ConditionCd = int.Parse(ce.Element(xn + "ConditionCd")?.Value ?? "30"),
                        RetentionReasonCd = int.Parse(ce.Element(xn + "RetentionReasonCd")?.Value ?? "58")
                    });
                }

                yield return record;
            }


        }

    }
}
