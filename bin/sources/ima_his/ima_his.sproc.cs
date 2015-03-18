using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using DevV1.Adapters.dbo.ima_his;

namespace DevV1.Adapters.dbo.ima_his
{
    public partial class ima_his
    {
        public string ConnectionString { set; get; }
    }
}
