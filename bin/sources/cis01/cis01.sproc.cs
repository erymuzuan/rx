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
using Dev.Adapters.dbo.cis01;

namespace Dev.Adapters.dbo.cis01
{
    public partial class cis01
    {
        public string ConnectionString { set; get; }
    }
}
