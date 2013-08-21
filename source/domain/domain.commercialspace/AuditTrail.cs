using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{

    public partial class AuditTrail : Entity
    {
        private int m_auditTrailId;

        public AuditTrail()
        {
            
        }

        public AuditTrail(IEnumerable<Change> changes)
        {
            this.ChangeCollection.AddRange(changes);
        }

        [XmlAttribute]
        public int AuditTrailId
        {
            get { return m_auditTrailId; }
            set
            {
                m_auditTrailId = value;
                RaisePropertyChanged();
            }
        }

        private string m_note;

        public string Note
        {
            get { return m_note; }
            set
            {
                m_note = value;
                RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Changed by : {0}", this.ChangedBy);
            sb.AppendLine("<br/>");
            sb.AppendFormat("Changed datetime : {0}", this.DateTime);
            sb.AppendLine("<br/>");

            sb.AppendLine("<ul>");
            foreach (var change in this.ChangeCollection)
            {
                sb.AppendFormat("<li>{0}</li>", change);
            }
            sb.AppendLine("</ul>");

            return sb.ToString();
        }
    }
}