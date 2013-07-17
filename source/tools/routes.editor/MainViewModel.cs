using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Serialization;

namespace routes.editor
{
    public class MainViewModel : ViewModelBase
    {
        //
        public RelayCommand OpenCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand ValidateCommand { get; set; }
        public RelayCommand<JsRoute> MoveUpCommand { get; set; }
        public RelayCommand<JsRoute> MoveDownCommand { get; set; }

        //
        public MainViewModel()
        {
            this.OpenCommand = new RelayCommand(Open);
            this.AddCommand = new RelayCommand(Add);
            this.SaveCommand = new RelayCommand(Save);
            this.ValidateCommand = new RelayCommand(Validate);
            this.MoveDownCommand = new RelayCommand<JsRoute>(r => Move(r, 1), r => null != r && this.RouteCollection.IndexOf(r) < this.RouteCollection.Count - 1);
            this.MoveUpCommand = new RelayCommand<JsRoute>(r => Move(r, -1), r => null != r && this.RouteCollection.IndexOf(r) > 0);
        }

        private void Move(JsRoute route, int step)
        {
            var index = this.RouteCollection.IndexOf(route);
            this.RouteCollection.Move(index, index + step);
            this.SelectedRoute = route;
        }

        private void Validate()
        {
            this.RouteCollection.ToList().ForEach(Validate);

        }

        private void Validate(JsRoute route)
        {
            var root = System.IO.Path.GetDirectoryName(this.FileName) ?? string.Empty;
            var vm = System.IO.Path.Combine(root, @"App\" + route.ModuleId + ".js");
            if (System.IO.File.Exists(vm))
            {
                route.RemoveErrors("ModuleId");

            }
            else
            {
                // TODO :look for controllers
                route.AddErrors("ModuleId", "cannot find " + vm);

            }
        }

        public void Load()
        {
            var lastFile = Properties.Settings.Default.LastFile;
            if (string.IsNullOrWhiteSpace(lastFile)) return;
            if (!System.IO.File.Exists(lastFile)) return;

            this.FileName = lastFile;
            this.ReadJson();
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

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = System.IO.File.ReadAllText(this.FileName);
            var routes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);

            this.RouteCollection.Clear();
            routes.ToList().ForEach(this.RouteCollection.Add);
        }
        private void Add()
        {
            var route = new JsRoute();
            this.RouteCollection.Add(route);
        }
        private void Save()
        {
            var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            var json = JsonConvert.SerializeObject(this.RouteCollection, Formatting.Indented, settings);
            System.IO.File.WriteAllText(this.FileName, json);

            Properties.Settings.Default.LastFile = this.FileName;
            Properties.Settings.Default.Save();
        }

        private readonly ObservableCollection<JsRoute> m_routeCollection = new ObservableCollection<JsRoute>();
        private string m_fileName;
        private JsRoute m_selectedRoute;

        public JsRoute SelectedRoute
        {
            get { return m_selectedRoute; }
            set
            {
                m_selectedRoute = value;
                RaisePropertyChanged("SelectedRoute");
            }
        }
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
