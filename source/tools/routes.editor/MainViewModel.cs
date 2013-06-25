using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Linq;

namespace routes.editor
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        public MainViewModel()
        {
            this.OpenCommand = new RelayCommand(Open);
            this.SaveCommand = new RelayCommand(Save);
        }

        public void Load()
        {
            var lastFile = Properties.Settings.Default.LastFile;
            if (string.IsNullOrWhiteSpace(lastFile)) return;
            if (!System.IO.File.Exists(lastFile)) return;

            this.FileName = lastFile;
            var json = System.IO.File.ReadAllText(this.FileName);
            var routes = JsonConvert.DeserializeObject<JsRoute[]>(json);

            this.RouteCollection.Clear();
            routes.ToList().ForEach(this.RouteCollection.Add);
        }

        private void Open()
        {
            var dlg = new OpenFileDialog
                {
                    RestoreDirectory = true,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Filter = "Json|*.js;*.json|All Files|*.*",
                    Title = "Select routes json file"
                };

            if (dlg.ShowDialog() ?? false)
            {
                this.FileName = dlg.FileName;
                var json = System.IO.File.ReadAllText(this.FileName);
                var routes = JsonConvert.DeserializeObject<JsRoute[]>(json);

                this.RouteCollection.Clear();
                routes.ToList().ForEach(this.RouteCollection.Add);
            }

        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(this.RouteCollection);
            System.IO.File.WriteAllText(this.FileName, json);

            Properties.Settings.Default.LastFile = this.FileName;
            Properties.Settings.Default.Save();
        }

        private readonly ObservableCollection<JsRoute> m_routeCollection = new ObservableCollection<JsRoute>();
        private string m_fileName;

        public string FileName
        {
            get { return m_fileName; }
            set
            {
                m_fileName = value;
                RaisePropertyChanged("FileName");
            }
        }
        public ObservableCollection<JsRoute> RouteCollection
        {
            get { return m_routeCollection; }
        }
    }
}
