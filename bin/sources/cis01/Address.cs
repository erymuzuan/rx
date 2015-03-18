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
   public class Address : DomainObject
   {
     
        public override string ToString()
        {
            return "Address:" + AddressId;
        }       //member:AddressId
      [XmlAttribute]
      public int AddressId{get;set;}

       //member:AccountId
      [XmlAttribute]
      public int AccountId{get;set;}

       //member:Type
      [XmlAttribute]
      public string Type{get;set;}

       //member:Street1
      [XmlAttribute]
      public string Street1{get;set;}

       //member:Street2
      [XmlAttribute]
      public string Street2{get;set;}

       //member:Postcode
      [XmlAttribute]
      public string Postcode{get;set;}

       //member:City
      [XmlAttribute]
      public string City{get;set;}

       //member:State
      [XmlAttribute]
      public string State{get;set;}

       //member:Country
      [XmlAttribute]
      public string Country{get;set;}

   }
}
