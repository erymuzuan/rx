using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MailItems.ReceivePorts
{

    public class FromEDI
    {
        public string RecptclId { get; set; }

        public decimal ItemWeight { get; set; }

        public string DestCountryCd { get; set; }

        public EDIEvent EDIEvent { get; set; }



    }
}
