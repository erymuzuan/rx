using System.ComponentModel.Composition;
using Bespoke.Cycling.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public class InvoiceViewModel : ViewModelBase
    {
        private Registration m_registration;
        public RelayCommand AddPaymentCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public InvoiceViewModel()
        {
            //default

            this.AddPaymentCommand = new RelayCommand(AddPayment, () => null != this.Invoice && this.Invoice.Status != "Paid");
            //TODO : repos
            // this.SaveCommand = new RelayCommand(() =>
            //                                        {
            //                                            this.IsBusy = true;
            //                                            ObjectBuilder.GetObject<IInvoiceRepository>().SaveChange(
            //                                                t => this.IsBusy = false);
            //                                        });
        }


        public void LoadRegistrationInfo(int registrationId)
        {
            this.IsBusy = true;
            //TODO : repoas
            //var repos = ObjectBuilder.GetObject<IRegistrationRepository>();
            //repos.Load(registrationId, r =>
            //{
            //    this.Registration = r;
            //    // now load the invoices
            //    var ir = ObjectBuilder.GetObject<IInvoiceRepository>();
            //    ir.Load(r.InvoiceNo, v =>
            //    {
            //        this.Invoice = v;
            //        this.IsBusy = false;
            //    });
            //});

        }

        private void AddPayment()
        {
            // TODO : repos
            //var payment = new Payment
            //                  {
            //                      Date = DateTime.Today,
            //                      VerifyBy = WebContext.Current.User.Name,
            //                      InvoiceId = this.Invoice.InvoiceId
            //                  };

            //var repos = ObjectBuilder.GetObject<IPaymentRepository>();
            //repos.Add(payment);
            //this.SelectedPayment = payment;
        }

        public Registration Registration
        {
            get { return m_registration; }
            set
            {
                m_registration = value;
                RaisePropertyChanged("Registration");
            }
        }


        private Payment m_selectedPayment;
        private readonly ObjectCollection<Payment> m_paymentCollection = new ObjectCollection<Payment>();
        private bool m_isBusy;
        private Invoice m_invoice;

        public ObjectCollection<Payment> PaymentCollection
        {
            get { return m_paymentCollection; }
        }

        public Payment SelectedPayment
        {
            get { return m_selectedPayment; }
            set
            {
                m_selectedPayment = value;
                RaisePropertyChanged("SelectedPayment");
            }
        }
        
        public Invoice Invoice
        {
            get { return m_invoice; }
            set
            {
                m_invoice = value;
                RaisePropertyChanged("Invoice");
            }
        }
        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
    }
}