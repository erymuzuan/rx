using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MyApp.ReceivePorts
{

    public class SenderObject
    {
        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int Postcode { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string ReferenceNumber { get; set; }



    }
}
