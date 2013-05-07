using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Bespoke.Station.Windows.ViewModels.Utils
{
    [Export]
    public partial class SettingAndOptionsViewModel : StationViewModelBase<Setting>
    {
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand<string> BrowseFileCommand { get; set; }


        [ImportMany("OptionPanel", typeof(UserControl), AllowRecomposition = true)]
        public Lazy<object, IOptionPanelMetadata>[] OptionPanels { get; set; }


        public SettingAndOptionsViewModel()
        {
            if (this.IsInDesignMode) return;
            this.SaveCommand = new RelayCommand(Save);
            this.BrowseFileCommand = new RelayCommand<string>(BrowseFile, CanBrowseFile);
        }

        private bool CanBrowseFile(string obj)
        {
            return true;
        }

        private void BrowseFile(string obj)
        {
            throw new NotImplementedException();
        }

        private async void Save()
        {

            this.ShowBusy("Saving...");
            using (var session = this.Context.OpenSession())
            {
                session.Attach(this.SettingCollection.Cast<Entity>().ToArray());
                await session.SubmitChanges();


            }
            this.HideBusy();
        }


        private async void CreateAndAddSeting(string key, Action<string> propertySet)
        {
            var setting = await this.Context.LoadOneAsync<Setting>(s => s.Key == key) ?? new Setting { Key = key };
            propertySet(setting.Value);
            this.SettingCollection.Add(setting);
        }


    }
}
