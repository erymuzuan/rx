using System;
using System.IO;
using Newtonsoft.Json;
using Bespoke.Sph.Domain;
using System.Collections.Generic;
using System.Globalization;

namespace Bespoke.MailItems.ReceivePorts
{

    public class Letter
    {
        public string ConveyanceTypeCd { get; set; }

        public string MailCategoryCd { get; set; }

        public string MailItemCategoryCd { get; set; }

        public int ExpressInd { get; set; }

        public int CoDInd { get; set; }

        public string LetterCharacteristicCd { get; set; }



    }
}
