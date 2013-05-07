using Bespoke.Cycling.Domain;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    public partial class RideViewModel
    {
        private Faq m_selectedFaqSection;
        private bool m_isBusyUploadDocument;

        public bool IsBusyUploadDocument
        {
            get { return m_isBusyUploadDocument; }
            set
            {
                m_isBusyUploadDocument = value;
                RaisePropertyChanged("IsBusyUploadDocument");
            }
        }

        public Faq SelectedFaqSection
        {
            get { return m_selectedFaqSection; }
            set
            {
                m_selectedFaqSection = value;
                RaisePropertyChanged("SelectedFaqSection");
            }
        }

        private Itinerary m_selectedItinerarySection;
        public Itinerary SelectedItinerarySection
        {
            get { return m_selectedItinerarySection; }
            set
            {
                m_selectedItinerarySection = value;
                RaisePropertyChanged("SelectedItinerarySection");
            }
        }

    }
}