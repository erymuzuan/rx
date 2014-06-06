using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition : Entity
    {
        private int m_transformDefinitionId;

        [XmlAttribute]
        public int TransformDefinitionId
        {
            get { return m_transformDefinitionId; }
            set
            {
                m_transformDefinitionId = value;
                OnPropertyChanged();
            }
        }

        public string Name { get; set; }
        public string Description { get; set; }

        private readonly ObjectCollection<Map> m_mapCollection = new ObjectCollection<Map>();

        public ObjectCollection<Map> MapCollection
        {
            get { return m_mapCollection; }
        }


        public async Task<object> TransformAsync(object source)
        {
            var sb = new StringBuilder("{");
            sb.AppendLine();
            var tasks = from m in this.MapCollection
                select m.ConvertAsync(source);
            var maps = await Task.WhenAll(tasks);
            sb.AppendLine(string.Join(",\r\n    ", maps.ToArray()));
            sb.AppendLine();
            sb.Append("}");
            return sb.ToString();
        }
    }
}