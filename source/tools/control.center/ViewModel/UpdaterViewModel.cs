using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public class UpdaterViewModel : ViewModelBase, IView
    {
        public RelayCommand CheckUpdateCommand { get; set; }
        public RelayCommand LoadUpdateCommand { get; set; }
        private bool m_isBusy;
        private bool m_isUpdating;

        public bool IsUpdating
        {
            get { return m_isUpdating; }
            set
            {
                m_isUpdating = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                OnPropertyChanged();
            }
        }
        public UpdaterViewModel()
        {
            this.CheckUpdateCommand = new RelayCommand(CheckUpdate);
            this.LoadUpdateCommand = new RelayCommand(LoadUpdate);
        }

        private void LoadUpdate()
        {
            var d = new OpenFileDialog { Filter = "7zip archived (.7z)|*.7z", DefaultExt = ".7z" };
            if (d.ShowDialog() ?? false)
            {
                var path = Directory.GetCurrentDirectory() + "\\temp";
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                Directory.CreateDirectory(path);

                var sz = new ProcessStartInfo("7za", $"x \"{d.FileName}\" -o\"{path}\"");
                var pz = Process.Start(sz);
                pz?.WaitForExit();
                this.QueueUserWorkItem(RunUpdatePackage, "", this.Settings);
            }
        }

        private async void CheckUpdate()
        {
            this.Settings = SphSettings.Load();
            var file = @".\version.json".TranslatePath();
            if (!File.Exists(file))
            {
                MessageBox.Show("Can't find " + file + " in your root", "Rx Developer", MessageBoxButton.OK,
                    MessageBoxImage.Stop);
                return;
            }
            this.IsBusy = true;
            var text = File.ReadAllText(file);
            var json = JObject.Parse(text);
            var build = json.SelectToken("$.build").Value<int>();
            using (var client = new HttpClient { BaseAddress = new Uri(this.Settings.UpdateUri) })
            {
                var url = "binaries/" + build + ".json";
                var responseJson = "";
                try
                {
                    var response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        MessageBox.Show("Now new update is found, Please check again in the future", "Rx Developer", MessageBoxButton.OK,
                                MessageBoxImage.Information);


                        return;

                    }
                    var content = response.Content as StreamContent;
                    if (null == content) return;
                    responseJson = await content.ReadAsStringAsync();
                    if (string.IsNullOrWhiteSpace(responseJson))
                    {
                        MessageBox.Show("Too bad, not getting any update info", Strings.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show($"Cannot download {client.BaseAddress}/{url} \r\n{e}", Strings.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                finally
                {
                    this.IsBusy = false;
                }

                var jo = JObject.Parse(responseJson);
                var vnext = jo.SelectToken("$.vnext").Value<int>();
                if (vnext > build)
                {
                    var updateScript = jo.SelectToken("$.update-script").Value<string>();
                    var ps1 = await client.GetByteArrayAsync("binaries/" + updateScript);
                    File.WriteAllBytes(updateScript.TranslatePath(), ps1);

                    var result = MessageBox.Show(updateScript + " has been downloaded to your working directory, Do you want to apply this update? Please make sure you have saved and commit all changes, updating may cause your work to be overwriten", Strings.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        this.IsUpdating = true;
                        this.IsBusy = true;
                        var updatePackage = $"binaries/{vnext}/{vnext}.7z";
                        Console.WriteLine($"Downloading {updatePackage} ...");
                        Console.WriteLine("Please wait...");
                        var package = await client.GetByteArrayAsync(updatePackage);

                        if (Directory.Exists(vnext.ToString()))
                            Directory.Delete(vnext.ToString(), true);

                        Directory.CreateDirectory(vnext.ToString());

                        File.WriteAllBytes($"{vnext}\\{vnext}.7z", package);
                        Console.WriteLine($"Update package was succesfully downloaded and saved to {vnext}.7z");

                        this.QueueUserWorkItem(RunUpdatePackage, updateScript.TranslatePath(), this.Settings);

                    }

                }
                else
                {
                    MessageBox.Show("No update is available", Strings.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }

        private void RunUpdatePackage(string path, SphSettings settings)
        {

            if (!File.Exists(path))
            {
                this.Post(() =>
                {
                    this.IsBusy = false;
                    this.Status = "fail";
                    MessageBox.Show($"Cannot find {path}", Strings.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                });
                return;
            }


            using (var ps = PowerShell.Create())
            {
                var ps1 = File.ReadAllText(path);
                ps.AddScript(ps1);

                var outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += outputCollection_DataAdded;
                ps.Streams.Error.DataAdded += Error_DataAdded;
                ps.Streams.Verbose.DataAdded += Verbose_DataAdded;
                ps.Streams.Warning.DataAdded += Warning_DataAdded;
                ps.Streams.Debug.DataAdded += Debug_DataAdded;
                ps.Streams.Progress.DataAdded += Progress_DataAdded;
                ps.InvocationStateChanged += Ps_InvocationStateChanged;


                var result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);


                while (result.IsCompleted == false)
                {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    UpdateProgress();
                    Thread.Sleep(100);
                }

                this.Log("Execution has stopped. The pipeline state " + ps.InvocationStateInfo.State);


                var status = ps.HadErrors ? "fail" : "success";
                this.Post(() =>
                {
                    this.Progress = 100;
                    this.Status = status;
                    this.IsBusy = false;
                    this.IsUpdating = false;
                });
            }
        }

        void UpdateProgress(double step = 0.2d, string message = ". ")
        {
            this.Post(() =>
            {
                if (this.Progress < 100)
                    this.Progress += step;
                this.Log(message);
            });
        }
        private void Progress_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var errors = sender as PSDataCollection<ProgressRecord>;
            if (null != errors)
            {
                var er = errors[e.Index];
                message = er.ToString();
            }
            this.Post(a =>
            {
                this.Log(message);
            }, e);
        }

        private void Debug_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var errors = sender as PSDataCollection<DebugRecord>;
            if (null != errors)
            {
                var er = errors[e.Index];
                message = er + "\r\n";
            }
            this.Post(a =>
            {
                this.Log(message);
            }, e);
        }

        private void Warning_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var errors = sender as PSDataCollection<WarningRecord>;
            if (null != errors)
            {
                var er = errors[e.Index];
                message = er.Message;
            }
            this.Post(a =>
            {
                this.Log(message);
            }, e);
        }

        private void Verbose_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var streams = sender as PSDataCollection<VerboseRecord>;
            if (null != streams)
            {
                var vb = streams[e.Index];
                message = vb.Message;
            }
            this.Post(a =>
            {
                this.Log(message);
            }, e);
        }

        private void Ps_InvocationStateChanged(object sender, PSInvocationStateChangedEventArgs e)
        {
            this.Log("InvocationStateChanged : reason -> " + e.InvocationStateInfo.Reason + ", state -> " + e.InvocationStateInfo.State);
        }

        private void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            var psobjects = sender as PSDataCollection<PSObject>;
            var message = "";
            if (null != psobjects)
            {
                var ps = psobjects[e.Index];
                message = ps.ToString();
            }
            this.Post(a =>
            {
                this.Log(a);
            }, message);
        }

        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var errors = sender as PSDataCollection<ErrorRecord>;
            if (null != errors)
            {
                var er = errors[e.Index];
                message = er + "";
            }
            this.Post(a =>
            {
                this.Log(message);
            }, e);
        }

        public void Log(string message, Severity severity = Severity.Verbose)
        {
            this.Post((m, s) =>
            {
                this.LogCollection.Add(new LogEntry { Severity = s, Message = m, Time = DateTime.Now });
                Console.WriteLine(message);

            }, message, severity);
        }




        public ObservableCollection<LogEntry> LogCollection { get; } = new ObservableCollection<LogEntry>();
        public DispatcherObject View
        {
            get; set;
        }

        private double m_progress;
        private SphSettings m_settings;

        public SphSettings Settings
        {
            get { return m_settings; }
            set
            {
                m_settings = value;
                RaisePropertyChanged("Settings");
            }
        }

        public double Progress
        {
            get { return m_progress; }
            set
            {
                m_progress = value;
                RaisePropertyChanged("Progress");
            }
        }

        private MainViewModel m_mainViewModel;
        private string m_status;

        public string Status
        {
            get { return m_status; }
            set
            {
                m_status = value;
                RaisePropertyChanged("Status");
            }
        }

        public MainViewModel MainViewModel
        {
            get { return m_mainViewModel; }
            set
            {
                m_mainViewModel = value;
                RaisePropertyChanged("MainViewModel");
            }
        }
    }
}
