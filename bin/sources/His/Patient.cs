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

namespace DevV1.Adapters.dbo.His
{
   public class Patient : DomainObject
   {
     
        public override string ToString()
        {
            return "Patient:" + Id;
        }       //member:Id
      [XmlAttribute]
      public int Id{get;set;}

       //member:Dob
      [XmlAttribute]
      public DateTime Dob{get;set;}

       //member:Income
      [XmlAttribute]
      public decimal Income{get;set;}

       //member:Name
      [XmlAttribute]
      public string Name{get;set;}

       //member:Mrn
      [XmlAttribute]
      public string Mrn{get;set;}

       //member:Gender
      [XmlAttribute]
      public string Gender{get;set;}

   }
}
