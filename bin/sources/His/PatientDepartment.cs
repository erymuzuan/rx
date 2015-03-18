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
   public class PatientDepartment : DomainObject
   {
     
        public override string ToString()
        {
            return "PatientDepartment:" + "";
        }       //member:PatientId
      [XmlAttribute]
      public int PatientId{get;set;}

       //member:DepartmentId
      [XmlAttribute]
      public int DepartmentId{get;set;}

   }
}
