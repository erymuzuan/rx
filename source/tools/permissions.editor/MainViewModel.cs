using System;
using System.Collections.ObjectModel;
using System.Web.Security;
using System.Windows;
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
        public RelayCommand CreateUserAdminCommand { get; set; }

        public MainViewModel()
        {
            this.OpenCommand = new RelayCommand(Open);
            this.SaveCommand = new RelayCommand(Save);
            this.CreateUserAdminCommand = new RelayCommand(CreateUserAdmin);
        }

        private static void CreateUserAdmin()
        {
            var em = Membership.GetUser("useradmin");

            if (null != em)
            {
                var userroles = Roles.GetRolesForUser("useradmin");
                if (userroles.Any())
                    Roles.RemoveUserFromRoles("useradmin", new[] { "admin_user" });

                Roles.AddUserToRoles("useradmin", new[] { "admin_user" });
                em.Email = "useradmin@gmail.com";

                Membership.UpdateUser(em);
                MessageBox.Show("useradmin already created..please login.");
            }

            else
            {
                Membership.CreateUser("useradmin", "123456", "useradmin@gmail.com");
                Roles.AddUserToRoles("useradmin", new[] { "admin_user" });
                MessageBox.Show("Username : useradmin , Password : 123456");
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
            var json = System.IO.File.ReadAllText(this.FileName);
            var routes = JsonConvert.DeserializeObject<RoleModel[]>(json);

            this.RoleCollection.Clear();
            routes.ToList().ForEach(this.RoleCollection.Add);
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(this.RoleCollection, Formatting.Indented);
            System.IO.File.WriteAllText(this.FileName, json);
            foreach (var r in this.RoleCollection)
            {
                var existing = Roles.RoleExists(r.Role);
                if (existing == false)
                    Roles.CreateRole(r.Role);
            }
            Properties.Settings.Default.LastFile = this.FileName;
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
