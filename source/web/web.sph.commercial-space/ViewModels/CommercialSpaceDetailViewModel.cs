﻿using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.ViewModels
{
    public class CommercialSpaceDetailViewModel
    {
        public SpaceTemplate Template { get; set; }

        public ApplicationTemplate[] ApplicationTemplates { get; set; }
    }
}