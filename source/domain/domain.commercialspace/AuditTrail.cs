using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{

    public partial class AuditTrail : Entity
    {
        private int m_auditTrailId;

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
         
    }
}