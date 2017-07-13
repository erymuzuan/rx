using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MailItems.ReceivePorts
{

    public class Addressee
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int Postcode { get; set; }

        public string CountryCode { get; set; }



    }
}
