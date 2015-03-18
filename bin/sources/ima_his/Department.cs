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

namespace DevV1.Adapters.dbo.ima_his
{
   public class Department : DomainObject
   {
     
        public override string ToString()
        {
            return "Department:" + Id;
        }       //member:Id
      [XmlAttribute]
      public int Id{get;set;}

       //member:Name
      [XmlAttribute]
      public string Name{get;set;}

   }
}
