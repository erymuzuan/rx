using System.ComponentModel.Composition;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Station.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace Bespoke.Station.Windows.ViewModels
{
    [Export]
    public class GeneralOptionViewModel : StationViewModelBase<Ride>
    {
        public RelayCommand SaveCommmand { get; set; }
        public RelayCommand<TextBox > BrowseFileCommand { get; set; }


        public GeneralOptionViewModel()
        {
            if (this.IsInDesignMode) return;
            this.SaveCommmand = new RelayCommand(Save);
            this.BrowseFileCommand = new RelayCommand<TextBox>(BrowseFile);


        //    this.SevenZipPath = Properties.Settings.Default.SevenZipExe;
        //    this.ImagesUnpackPath = Properties.Settings.Default.UnpackVideoImagesDirectory = this.ImagesUnpackPath;
        //    this.GmailPassword = Properties.Settings.Default.GmailPassword = this.GmailPassword;
        //    this.GmailUserName = Properties.Settings.Default.GmailUserName;
        //    this.MyStationPassword = Properties.Settings.Default.MyStationPassword;
        //    this.MyStationUserName = Properties.Settings.Default.MyStationUserName;
        //    this.eServicePassword = Properties.Settings.Default.eServicePassword;
        //    this.eServiceUserName = Properties.Settings.Default.eServiveUserName;


        //    this.Camera4 = Properties.Settings.Default.Camera4;
        //    this.Camera7 = Properties.Settings.Default.Camera7;
        //    this.SsPath = Properties.Settings.Default.SsPath;
        //    this.SvnCommitbat = Properties.Settings.Default.SvnCommitbat;
        //    this.PicasaPath = Properties.Settings.Default.PicasaPath;
        }

        private void BrowseFile(TextBox value)
        {
            var dialog = new OpenFileDialog();
            if(dialog.ShowDialog() ?? false)
            {
                value.Text = dialog.FileName;
            }
        }

        private void Save()
        {
            this.IsBusy = true;
            //Properties.Settings.Default.SevenZipExe = this.SevenZipPath;
            //Properties.Settings.Default.UnpackVideoImagesDirectory = this.ImagesUnpackPath;
            //Properties.Settings.Default.GmailPassword = this.GmailPassword;
            //Properties.Settings.Default.GmailUserName = this.GmailUserName;
            //Properties.Settings.Default.MyStationPassword = this.MyStationPassword;
            //Properties.Settings.Default.MyStationUserName = this.MyStationUserName;
            //Properties.Settings.Default.eServicePassword = this.eServicePassword;
            //Properties.Settings.Default.eServiveUserName = this.eServiceUserName;

            //Properties.Settings.Default.Camera4 = this.Camera4;
            //Properties.Settings.Default.Camera7 = this.Camera7;
            //Properties.Settings.Default.SsPath = this.SsPath;
            //Properties.Settings.Default.SvnCommitbat = this.SvnCommitbat;
            //Properties.Settings.Default.PicasaPath = this.PicasaPath;


            this.QueueUserWorkItem(SaveCallback);
        }

        private void SaveCallback()
        {
            Properties.Settings.Default.Save();
            this.IsBusy = false;
        }



        private string m_gmailUserName;
        private string m_gmailPassword;
        private string m_myStationUserName;
        private string m_myStationPassword;
        private string m_eServiceUserName;
        private string m_eServicePassword;
        private string m_sevenZipPath;
        private string m_imagesUnpackPath;
        private string m_ssPath;
        private string m_camera4;
        private string m_camera7;
        private string m_svnCommitbat;

        private string m_picasaPath;


        public string PicasaPath
        {
            get { return m_picasaPath; }
            set
            {
                m_picasaPath = value;
                RaisePropertyChanged("PicasaPath");
            }
        }

        public string SvnCommitbat
        {
            get { return m_svnCommitbat; }
            set
            {
                m_svnCommitbat = value;
                RaisePropertyChanged("SvnCommitbat");
            }
        }


        public string Camera7
        {
            get { return m_camera7; }
            set
            {
                m_camera7 = value;
                RaisePropertyChanged("Camera7");
            }
        }


        public string Camera4
        {
            get { return m_camera4; }
            set
            {
                m_camera4 = value;
                RaisePropertyChanged("Camera4");
            }
        }


        public string SsPath
        {
            get { return m_ssPath; }
            set
            {
                m_ssPath = value;
                RaisePropertyChanged("SsPath");
            }
        }



        public string ImagesUnpackPath
        {
            get { return m_imagesUnpackPath; }
            set
            {
                m_imagesUnpackPath = value;
                RaisePropertyChanged("ImagesUnpackPath");
            }
        }

        public string SevenZipPath
        {
            get { return m_sevenZipPath; }
            set
            {
                m_sevenZipPath = value;
                RaisePropertyChanged("SevenZipPath");
            }
        }

        public string eServicePassword
        {
            get { return m_eServicePassword; }
            set
            {
                m_eServicePassword = value;
                RaisePropertyChanged("eServicePassword");
            }
        }

        public string eServiceUserName
        {
            get { return m_eServiceUserName; }
            set
            {
                m_eServiceUserName = value;
                RaisePropertyChanged("eServiceUserName");
            }
        }

        public string MyStationPassword
        {
            get { return m_myStationPassword; }
            set
            {
                m_myStationPassword = value;
                RaisePropertyChanged("MyStationPassword");
            }
        }

        public string MyStationUserName
        {
            get { return m_myStationUserName; }
            set
            {
                m_myStationUserName = value;
                RaisePropertyChanged("MyStationUserName");
            }
        }

        public string GmailPassword
        {
            get { return m_gmailPassword; }
            set
            {
                m_gmailPassword = value;
                RaisePropertyChanged("GmailPassword");
            }
        }

        public string GmailUserName
        {
            get { return m_gmailUserName; }
            set
            {
                m_gmailUserName = value;
                RaisePropertyChanged("GmailUserName");
            }
        }
    }
}