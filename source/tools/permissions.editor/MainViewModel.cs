using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Linq;

namespace permissions.editor
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand ValidateCommand { get; set; }

        public MainViewModel()
        {
            this.OpenCommand = new RelayCommand(Open);
            this.SaveCommand = new RelayCommand(Save);
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

            // ReSharper disable ConstantNullCoalescingCondition
            if (dlg.ShowDialog() ?? false)
            // ReSharper restore ConstantNullCoalescingCondition
            {
                this.FileName = dlg.FileName;
                this.ReadJson();
            }

        }

        private void ReadJson()
        {
           var json = System.IO.File.ReadAllText(this.FileName);
            var routes = JsonConvert.DeserializeObject<RoleModel[]>(json);

            this.RoleCollection.Clear();
            routes.ToList().ForEach(this.RoleCollection.Add);
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(this.RoleCollection, Formatting.Indented);
            System.IO.File.WriteAllText(this.FileName, json);

          
            Properties.Settings.Default.Save();
        }
        private readonly ObservableCollection<RoleModel> m_roleCollection = new ObservableCollection<RoleModel>();
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

        public ObservableCollection<RoleModel> RoleCollection
        {
            get { return m_roleCollection; }
        }
    }
}
