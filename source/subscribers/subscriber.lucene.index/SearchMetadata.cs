using System;
using System.Collections.Generic;

namespace Bespoke.Sph.Messaging
{
    class SearchMetadata : ISearchable
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Summary { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public string Title { get; set; }
        public Dictionary<string, object> CustomFields { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string OwnerCode { get; set; }
    }
}