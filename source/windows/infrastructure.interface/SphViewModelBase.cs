using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.CommercialSpace.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using GalaSoft.MvvmLight;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Composition;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows;

namespace Bespoke.Sph.Windows.Infrastructure
{
    public abstract class SphViewModelBase<T> : ViewModelBase, INotifyDataErrorInfo, IPartImportsSatisfiedNotification where T : DomainObject
    {
        public RelayCommand<string> ChangeViewCommand { get; set; }
        public RelayCommand GotoDashboardViewCommand { get; set; }
        public RelayCommand<T> PrintCommand { get; set; }
        public RelayCommand<T> OpenCommand { get; set; }

        #region "Data error info"

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return (m_currentErrors.Values);
            }
            MakeOrCreatePropertyErrorList(propertyName);
            return (m_currentErrors[propertyName]);
        }
        public bool HasErrors
        {
            get
            {
                return (m_currentErrors.Count > 0);
            }
        }
        void MakeOrCreatePropertyErrorList(string propertyName)
        {
            if (!m_currentErrors.ContainsKey(propertyName))
            {
                m_currentErrors[propertyName] = new List<ErrorInfo>();
            }
        }
        void FireErrorsChanged(string property)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(this, new DataErrorsChangedEventArgs(property));
            }
        }
        protected void ClearErrorFromProperty(string property, int errorCode)
        {
            MakeOrCreatePropertyErrorList(property);

            ErrorInfo error =
              m_currentErrors[property].SingleOrDefault(e => e.ErrorCode == errorCode);

            if (error != null)
            {
                m_currentErrors[property].Remove(error);
                FireErrorsChanged(property);
            }
        }
        protected void AddErrorForProperty(string property, ErrorInfo error)
        {
            MakeOrCreatePropertyErrorList(property);

            if (m_currentErrors[property].SingleOrDefault(e => e.ErrorCode == error.ErrorCode) == null)
            {
                m_currentErrors[property].Add(error);
                FireErrorsChanged(property);
            }
        }

        readonly Dictionary<string, List<ErrorInfo>> m_currentErrors;
        #endregion

   
        [Import]
        public IView View { get; set; }

        protected bool Confirm(string message)
        {
            return this.View.Confirm(message);
        }

        protected void Alert(string message, AlertImage image = AlertImage.Information)
        {
            this.View.Alert(message, image);
        }


        protected SphViewModelBase()
        {

            if (this.IsInDesignMode) return;
            this.GotoDashboardViewCommand = new RelayCommand(() => ChangeView("DashboardView"));
            this.ChangeViewCommand = new RelayCommand<string>(ChangeView, v => !string.IsNullOrWhiteSpace(v));
            this.PrintCommand = new RelayCommand<T>(Print, t => null != t);
            this.OpenCommand = new RelayCommand<T>(Open, t => null != t);

            m_currentErrors = new Dictionary<string, List<ErrorInfo>>();
        }

        protected virtual void Open(T item)
        {
            MessageBox.Show("No open command is implemented");
        }

        protected virtual void Print(T item)
        {
            MessageBox.Show("No print command is implemented");
        }

        protected void ChangeView(string viewName)
        {
            Messenger.Default.Send(new ChangeViewArgs(viewName));
        }

        [Import]
        public SphDataContext   Context { get; set; }

        private bool m_isBusy;
        private string m_loadingMessage;
        protected virtual void OnIsBusyChanged(bool busy)
        {

        }
        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                OnIsBusyChanged(value);
                RaisePropertyChanged("IsBusy");
                if (value == false)
                {
                    LoadingMessage = String.Empty;
                    Messenger.Default.Send(new StatusBarMessage
                    {
                        IsBusy = false,
                        Text = "Ready"
                    });
                }
            }
        }

        public string LoadingMessage
        {
            get { return m_loadingMessage; }
            set
            {
                m_loadingMessage = value;
                RaisePropertyChanged("LoadingMessage");
            }
        }


        protected void ShowBusy(string message, params object[] args)
        {
            this.View.Post((m, a) =>
            {
                this.IsBusy = true;
                this.LoadingMessage = string.Format(m, a);
                Messenger.Default.Send(new StatusBarMessage
                                           {
                                               IsBusy = true,
                                               Text = m
                                           });
            }, message, args);
        }

        protected void HideBusy()
        {

            this.View.Post(() =>
            {
                this.IsBusy = false;
                this.LoadingMessage = string.Empty;
                Messenger.Default.Send(new StatusBarMessage
                {
                    IsBusy = false
                });
            });

        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            OnViewReady();
        }

        protected virtual void OnViewReady()
        {
        }



    }
}
