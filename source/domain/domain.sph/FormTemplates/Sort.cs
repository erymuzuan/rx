﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class Sort : DomainObject
    {
        public override string ToString()
        {
            return $"{Path} {Direction}";
        }
    }
}