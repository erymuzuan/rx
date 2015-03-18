using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace Dev.Adapters.dbo.cis01
{
   public class Invoice : DomainObject
   {
     
        public override string ToString()
        {
            return "Invoice:" + InvoiceId;
        }       //member:InvoiceId
      [XmlAttribute]
      public int InvoiceId{get;set;}

       //member:AccountId
      [XmlAttribute]
      public int AccountId{get;set;}

       //member:Date
      [XmlAttribute]
      public DateTime Date{get;set;}

       //member:Amount
      [XmlAttribute]
      public decimal Amount{get;set;}

       //member:No
      [XmlAttribute]
      public string No{get;set;}

   }
}
