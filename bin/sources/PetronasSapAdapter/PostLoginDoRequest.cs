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

   public class PostLoginDoRequest : DomainObject
   {
       public string PostData
       {
           get{
               return "inUserName=" + inUserName + "&inPassword=" + inPassword + "&submitf=" + submitf;
           }
       }
       //member:inUserName
      [XmlAttribute]
      public string inUserName{get;set;}

       //member:inPassword
      [XmlAttribute]
      public string inPassword{get;set;}

       //member:submitf
      [XmlAttribute]
      public string submitf{get;set;}

   }
}
