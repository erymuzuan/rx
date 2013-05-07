﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Station.Windows.Infrastructure;
using Bespoke.Station.Windows.Models;
using Bespoke.Station.Windows.ViewModels.Utils;
using Bespoke.Station.Windows.Views.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Bespoke.Station.Windows.ViewModels
{
    [Export]
    public class MainWindowViewModel : ViewModelBase, IPartImportsSatisfiedNotification
    {
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ShowGeneralOptionCommand { get; set; }
        [Import]
        public SettingAndOptionsViewModel SettingAndOptionsViewModel { get; set; }
        [ImportMany("ViewContract", typeof(UserControl), AllowRecomposition = true)]
        public Lazy<UserControl, IViewMetadata>[] Views { get; set; }

        [ImportMany(AllowRecomposition = true)]
        public ICommandViewModel[] Commands { get; set; }

        public MainWindowViewModel()
        {
            this.ExitCommand = new RelayCommand(() => Application.Current.Shutdown(0));
            this.ShowGeneralOptionCommand = new RelayCommand(ShowGeneralOption);
            Messenger.Default.Register<ChangeViewArgs>(this, ChangeView);
        }

        private void ShowGeneralOption()
        {
            var dg = new SettingAndOptionsWindow {DataContext = this.SettingAndOptionsViewModel};
            dg.ShowDialog();
        }

        private void ChangeView(ChangeViewArgs view)
        {
            var metadata = this.Views.SingleOrDefault(v => v.Metadata.Name == view.ViewName);
            if (null != metadata)
                this.SelectedView = metadata.Value;
        }

        public async void OnImportsSatisfied()
        {
           
            var home = this.Views.SingleOrDefault(v => v.Metadata.IsHome);
            if (null == home)
            {
                throw new InvalidOperationException("No Home page is not defined");
            }
            //this.SelectedView = home.Value;
            ReloadAllViews();
            var context = new CyclingDataContext();
            var station = await context.LoadOneAsync<Setting>(s => s.Key == "Station.Name") ?? new Setting { Key = "Station.Name", Value = "Stesen Petronas..."};
            Application.Current.MainWindow.Title = station.Value;
            await Task.Delay(100);
            Messenger.Default.Send(new ChangeViewArgs(home.Metadata.Name));
         
        }

        private void ReloadAllViews()
        {
            var groups =
                this.Views
                .OrderBy(v => v.Metadata.SubGroup)
                .Select(v => v.Metadata.Group)
                .Distinct()
                .Select(g => new SsViewModelGroup { Caption = g })
                .ToList();
            foreach (var view in this.Views.OrderBy(v => v.Metadata.SubGroup).Select(v => v.Metadata))
            {
                var subGroupCaption = view.SubGroup;
                if (string.IsNullOrWhiteSpace(subGroupCaption)) subGroupCaption = "Biasa";

                var group = groups.Single(g => g.Caption == view.Group);
                var sub = group.ItemCollection.SingleOrDefault(sg => sg.Caption == subGroupCaption);
                if (null == sub)
                {
                    sub = new SsViewModelSubGroup { Caption = subGroupCaption };
                    group.ItemCollection.Add(sub);
                }
                var vm = new SsViewModel
                             {
                                 ViewName = view.Name,
                                 Caption = view.Caption,
                                 Tooltip = view.Description,
                                 Image = view.Image,
                                 IsHidden = view.IsHidden
                             };
                sub.ItemCollection.Add(vm);
            }

            this.ViewGroupCollection.AddRange(groups);

            foreach (var vcs in this.Commands)
            {
                foreach (var c in vcs.Commands)
                {
                    var group = groups.SingleOrDefault(g => g.Caption == c.Group);
                    if (null == group)
                    {
                        group = this.ViewGroupCollection.SingleOrDefault(g => g.Caption == c.Group);
                        if (null == group)
                        {
                            group = new SsViewModelGroup { Caption = c.Group };
                            this.ViewGroupCollection.Add(group);
                        }
                    }
                    var sub = group.ItemCollection.SingleOrDefault(s => s.Caption == c.Subgroup);
                    if (null == sub)
                    {
                        sub = new SsViewModelSubGroup { Caption = c.Subgroup };
                        group.ItemCollection.Add(sub);
                    }

                    var vm = new SsViewModel
                    {
                        CommandVmCommand = c.Command,
                        CommandParameter = c.CommandParameter,
                        Caption = c.Caption,
                        Tooltip = c.Tooltip,
                        Image = c.Image,
                        Order = c.Order
                    };
                    sub.ItemCollection.Add(vm);

                }

            }
        }

        private UserControl m_selectedView;
        private readonly ObjectCollection<SsViewModelGroup> m_viewGroupCollection = new ObjectCollection<SsViewModelGroup>();
    
        public ObjectCollection<SsViewModelGroup> ViewGroupCollection
        {
            get { return m_viewGroupCollection; }
        }

        public UserControl SelectedView
        {
            get { return m_selectedView; }
            set
            {
                m_selectedView = value;
                RaisePropertyChanged("SelectedView");
            }
        }

    }
}
