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

   public class GetDisorderitemDoRequest : DomainObject
   {
       public string PostData
       {
           get{
return string.Empty;
           }
       }
   }
}
