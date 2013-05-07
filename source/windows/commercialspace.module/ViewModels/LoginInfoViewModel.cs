﻿using System.ComponentModel.Composition;
using GalaSoft.MvvmLight;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public class LoginInfoViewModel : ViewModelBase
    {
        private string m_userName;

        public string UserName
        {
            get { return m_userName; }
            set { m_userName = value; 
            RaisePropertyChanged("UserName");}
        }
    }
}