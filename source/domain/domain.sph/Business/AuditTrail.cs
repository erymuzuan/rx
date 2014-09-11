using System.Collections.Generic;
using System.Text;

namespace Bespoke.Sph.Domain
{

    public partial class AuditTrail : Entity
    {
        public AuditTrail()
        {
        }

        public AuditTrail(IEnumerable<Change> changes)
        {
            this.ChangeCollection.AddRange(changes);
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