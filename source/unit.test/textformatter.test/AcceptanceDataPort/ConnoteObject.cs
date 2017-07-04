using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MyApp.ReceivePorts
{

    public class ConnoteObject
    {
        public string ProductCode { get; set; }

        public string ConnoteNumber { get; set; }

        public string DestinationCountry { get; set; }

        public decimal? Weight { get; set; }

        public string Width { get; set; }

        public string Length { get; set; }

        public string Height { get; set; }

        public int DimWeight { get; set; }

        public int ItemCategory { get; set; }

        public string RoutingCode { get; set; }

        public string StandardOfService { get; set; }



    }
}
