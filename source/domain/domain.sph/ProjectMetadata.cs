using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class ProjectMetadata
    {
        private readonly ObjectCollection<ProjectChildItem> m_childItemCollection = new ObjectCollection<ProjectChildItem>();
        public string TypeName { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }
        public string Name { get; set; }

        public ObjectCollection<ProjectChildItem> ChildItemCollection
        {
            get { return m_childItemCollection; }
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
    }

    public class ProjectChildItem
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string Id { get; set; }
        public string ParentId { get; set; }
    }
}