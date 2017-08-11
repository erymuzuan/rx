using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements.Models
{
    public class DeploymentHistory : DomainObject
    {
        private string m_name;
        private DateTime m_dateTime;
        private string m_tag;
        private string m_revision;

        public string Revision
        {
            get => m_revision;
            set
            {
                m_revision = value;
                RaisePropertyChanged();
            }
        }

        public string Tag
        {
            get => m_tag;
            set
            {
                m_tag = value;
                RaisePropertyChanged();
            }
        }

        public DateTime DateTime
        {
            get => m_dateTime;
            set
            {
                m_dateTime = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => m_name;
            set
            {
                m_name = value;
                RaisePropertyChanged();
            }
        }
    }
}