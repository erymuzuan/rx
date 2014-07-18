using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace Dev.Adapters.Sap
{

   public class PostDisorderitemDoRequest : DomainObject
   {
       public string PostData
       {
           get{
               return "salesOrderNo=" + salesOrderNo + "&yearorder=" + yearorder + "&monthsorder=" + monthsorder + "&weekorder=" + weekorder + "&sortsorder=" + sortsorder + "&submitf=" + submitf + "&currentRows=" + currentRows + "&startRows=" + startRows + "&current=" + current;
           }
       }
       //member:salesOrderNo
      [XmlAttribute]
      public string salesOrderNo{get;set;}

       //member:yearorder
      [XmlAttribute]
      public string yearorder{get;set;}

       //member:monthsorder
      [XmlAttribute]
      public string monthsorder{get;set;}

       //member:weekorder
      [XmlAttribute]
      public string weekorder{get;set;}

       //member:sortsorder
      [XmlAttribute]
      public string sortsorder{get;set;}

       //member:submitf
      [XmlAttribute]
      public string submitf{get;set;}

       //member:currentRows
      [XmlAttribute]
      public string currentRows{get;set;}

       //member:startRows
      [XmlAttribute]
      public string startRows{get;set;}

       //member:current
      [XmlAttribute]
      public string current{get;set;}

   }
}
