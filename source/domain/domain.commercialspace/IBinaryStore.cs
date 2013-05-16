﻿using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IBinaryStore
    {
       void Add(BinaryStore document);
       Task<BinaryStore> GetContent(string stroreid);
    }
}