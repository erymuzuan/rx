using System;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class EmailAction : CustomAction
    {
        private string m_to;
        private string m_subjectTemplate;
        private string m_bodyTemplate;
        private string m_cc;
        private string m_bcc;

        public string Bcc
        {
            get { return m_bcc; }
            set
            {
                m_bcc = value;
                RaisePropertyChanged();
            }
        }

        public string Cc
        {
            get { return m_cc; }
            set
            {
                m_cc = value;
                RaisePropertyChanged();
            }
        }

        [XmlAttribute]
        public string To
        {
            get { return m_to; }
            set
            {
                m_to = value;
                RaisePropertyChanged();
            }
        }
        [XmlAttribute]
        public string SubjectTemplate
        {
            get { return m_subjectTemplate; }
            set
            {
                m_subjectTemplate = value;
                RaisePropertyChanged();
            }
        }


        public string BodyTemplate
        {
            get { return m_bodyTemplate; }
            set
            {
                m_bodyTemplate = value;
                RaisePropertyChanged();
            }
        }

        public override void Execute(Entity item)
        {
            Console.WriteLine("Email...");
        }

        public override Task ExecuteAsync(Entity item)
        {
            throw new NotImplementedException();
        }

        public override bool UseAsync
        {
            get { return false; }
        }
    }
}