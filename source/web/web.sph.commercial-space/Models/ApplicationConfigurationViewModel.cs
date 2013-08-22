﻿using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Models
{
    public class ApplicationConfigurationViewModel
    {
        public string StateOptions { get; set; }
        public string SpaceUsageOptions { get; set; }
        public string DepartmentOptions { get; set; }
        private readonly ObjectCollection<JsRoute> m_routesCollection = new ObjectCollection<JsRoute>();

        public ObjectCollection<JsRoute> Routes
        {
            get { return m_routesCollection; }
        }

        public string StartModule { get; set; }
    }
}