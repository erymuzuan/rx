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

   public class GetDisorderitemDoResponse : DomainObject
   {
       public string ResponseText{ get; private set;}
       public async Task LoadAsync(HttpResponseMessage response)
       {
           var content = response.Content as StreamContent;
           if(null == content) throw new Exception("Fail to read from response");
           this.ResponseText = await content.ReadAsStringAsync();
       }
   }
}
