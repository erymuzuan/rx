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
   public class Account : DomainObject
   {
     
        public override string ToString()
        {
            return "Account:" + AccountId;
        }       //member:AccountId
      [XmlAttribute]
      public int AccountId{get;set;}

       //member:Dob
      [XmlAttribute]
      public DateTime Dob{get;set;}

       //member:AccountNo
      [XmlAttribute]
      public string AccountNo{get;set;}

       //member:FirstName
      [XmlAttribute]
      public string FirstName{get;set;}

       //member:LastName
      [XmlAttribute]
      public string LastName{get;set;}

       //member:Status
      [XmlAttribute]
      public string Status{get;set;}

   }
}
