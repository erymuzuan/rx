using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MailItems.ReceivePorts
{

    public class MailItem
    {
        public string ItemId { get; set; }

        public FromIPS FromIPS { get; set; }

        public FromEDI FromEDI { get; set; }

        public string InterfaceCode { get; set; }



        //metadata
    }
}
