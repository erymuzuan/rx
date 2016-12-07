using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Station.Windows.RabbitMqDeadLetter.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Linq;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter.ViewModels
{
    [Export]
    public class ConnectionViewModel : ViewModelBase, IView
    {
        public RelayCommand<RabbitMqConnection> ConnectCommand { get; set; }
        public RelayCommand<RabbitMqConnection> EditConnectionCommand { get; set; }
        public RelayCommand DisconnectCommand { get; set; }
        public RelayCommand AddConnectionCommand { get; set; }

        private Exchange m_exchange;
        private bool m_isConnected;
        private bool m_isBusy;
        private IConnection m_conn;
        private RabbitMqConnection m_selectedConnection;
        private RabbitMqConnection m_connectedConnection;

        //TODO : we should set the one connected, for any operation, selected is used for UI only
        public RabbitMqConnection ConnectedConnection
        {
            get { return m_connectedConnection; }
            set
            {
                m_connectedConnection = value;
                RaisePropertyChanged("ConnectedConnection");
            }
        }

        public RabbitMqConnection SelectedConnection
        {
            get { return m_selectedConnection; }
            set
            {
                m_selectedConnection = value;
                RaisePropertyChanged("SelectedConnection");
            }
        }
        public ObservableCollection<RabbitMqConnection> ConnectionCollection { get; } = new ObservableCollection<RabbitMqConnection>();


        public ConnectionViewModel()
        {
            var json = Properties.Settings.Default.Connections;
            if (!string.IsNullOrWhiteSpace(json))
            {
                try
                {
                    var connections = JsonConvert.DeserializeObject<ObservableCollection<RabbitMqConnection>>(json);
                    this.ConnectionCollection.Clear();
                    connections.ToList().ForEach(this.ConnectionCollection.Add);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            this.ConnectCommand = new RelayCommand<RabbitMqConnection>(Connect, CanConnect);
            this.DisconnectCommand = new RelayCommand(Disconnect, CanDisconnect);
            this.AddConnectionCommand = new RelayCommand(AddConnection);
            this.EditConnectionCommand = new RelayCommand<RabbitMqConnection>(EditConnection);

        }

        private void EditConnection(RabbitMqConnection connection)
        {
            var json = JsonConvert.SerializeObject(connection);
            var clone = JsonConvert.DeserializeObject<RabbitMqConnection>(json);
            this.SelectedConnection = clone;
            var dialog = new ConnectionDialog { DataContext = this };
            dialog.ShowDialog();
            if (dialog.DialogResult ?? false)
            {

                var index = this.ConnectionCollection.IndexOf(connection);
                this.ConnectionCollection.Remove(connection);
                this.ConnectionCollection.Insert(index, clone);
                this.SelectedConnection = clone;

                this.SaveConnection();
                this.Connect(clone);
            }
            else
            {
                this.SelectedConnection = null;
            }
        }

        private void AddConnection()
        {
            var connection = new RabbitMqConnection();
            this.ConnectionCollection.Add(connection);
            this.SelectedConnection = connection;
            var dialog = new ConnectionDialog { DataContext = this };
            dialog.ShowDialog();
            if (dialog.DialogResult ?? false)
            {
                this.SaveConnection();
                this.Connect(connection);
            }
            else
            {
                this.ConnectionCollection.Remove(connection);
            }
        }

        public ObservableCollection<Exchange> ExchangeCollection { get; } = new ObservableCollection<Exchange>();

        public ObservableCollection<Queue> QueueCollection { get; } = new ObservableCollection<Queue>();

        public Exchange Exchange
        {
            get { return m_exchange; }
            set
            {
                m_exchange = value;
                RaisePropertyChanged("Exchange");
            }
        }

        public bool IsConnected
        {
            get { return m_isConnected; }
            set
            {
                m_isConnected = value;
                RaisePropertyChanged("IsConnected");
            }
        }


        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }


        public IModel Channel { get; private set; }

        private bool CanDisconnect()
        {
            return this.IsConnected;
        }

        public void Disconnect()
        {

            Channel.Dispose();
            m_conn.Dispose();
            this.IsConnected = false;
        }

        private async void Connect(RabbitMqConnection connection)
        {
            this.IsBusy = true;
            var factory = new ConnectionFactory
                {
                    VirtualHost = connection.VirtualHost,
                    HostName = connection.HostName,
                    UserName = connection.UserName,
                    Password = connection.Password,
                    Port = connection.Port,
                };
            m_conn = factory.CreateConnection();
            Channel = m_conn.CreateModel();
            this.IsConnected = true;


            var cache = new CredentialCache();
            var conn = this.SelectedConnection;
            var prefix = new Uri($"http://{conn.HostName}:{conn.ApiPort}/");
            cache.Add(prefix, "Basic", new NetworkCredential(conn.UserName, conn.Password));

            using (var handler = new HttpClientHandler { Credentials = cache })
            using (var client = new HttpClient(handler))
            {
                this.QueueCollection.Clear();
                this.ExchangeCollection.Clear();
                var queueResponse = await client.GetStringAsync($"http://{conn.HostName}:{conn.ApiPort}/api/queues");
                var queueJson = ( JsonConvert.DeserializeObject(queueResponse)) as Newtonsoft.Json.Linq.JArray;
                if (null != queueJson)
                {
                    foreach (dynamic q in queueJson)
                    {
                        try
                        {
                            var queue = new Queue
                                {
                                    Name = q.name.Value,
                                    IsAutoDelete = q.auto_delete.Value,
                                    IsDurable = q.durable.Value,
                                    VirtualHost = q.vhost.Value,
                                    MessagesCount = q.messages.Value
                                };
                            if (queue.VirtualHost != conn.VirtualHost) continue;
                            this.QueueCollection.Add(queue);


                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Cannot read queue " + q);
                        }
                    }

                }
                var excahngeResponse = await client.GetStringAsync($"http://{conn.HostName}:{conn.ApiPort}/api/exchanges");
                var exchangeJson = (JsonConvert.DeserializeObject(excahngeResponse)) as Newtonsoft.Json.Linq.JArray;
                if (null != exchangeJson)
                {
                    foreach (var q in exchangeJson)
                    {
                        var xch = new Exchange
                            {
                                Name = q.Value<string>("name"),
                                VirtualHost = q.Value<string>("vhost"),
                                Type = q.Value<string>("type")
                            };
                        if (xch.VirtualHost != conn.VirtualHost) continue;
                        this.ExchangeCollection.Add(xch);
                    }

                }

            }

            this.SaveConnection();
            this.IsBusy = false;
            var dlq = this.QueueCollection.FirstOrDefault(x => x.Name == "ms_dead_letter_queue");
            if (null != dlq)
                this.SelectedQueue = dlq;

            this.ConnectedConnection = new RabbitMqConnection(conn);
        }

        private async void PollSelectedQueue()
        {
            if (null == this.SelectedQueue) return;
            if (!this.IsConnected) return;

            var cache = new CredentialCache();
            var conn = this.SelectedConnection;
            var prefix = new Uri($"http://{conn.HostName}:{conn.ApiPort}/");
            cache.Add(prefix, "Basic", new NetworkCredential(conn.UserName, conn.Password));

            using (var handler = new HttpClientHandler { Credentials = cache })
            using (var client = new HttpClient(handler))
            {
                var queueResponse = await client.GetStringAsync($"http://{conn.HostName}:{conn.ApiPort}/api/queues/{HttpUtility.UrlEncode(conn.VirtualHost)}/{this.SelectedQueue.Name}");
                dynamic q = ( JsonConvert.DeserializeObject(queueResponse));
                this.SelectedQueue.MessagesCount = q.messages.Value;
            }
            await Task.Delay(2500);
            PollSelectedQueue();
        }


        protected override void RaisePropertyChanged(string propertyName)
        {
            if (this.SelectedQueue != null && propertyName == "SelectedQueue")
            {
                PollSelectedQueue();
            }

            base.RaisePropertyChanged(propertyName);
        }

        private void SaveConnection()
        {
            var json =  JsonConvert.SerializeObject(this.ConnectionCollection);
            Properties.Settings.Default.Connections = json;
            Properties.Settings.Default.Save();
        }

        private bool CanConnect(RabbitMqConnection connection)
        {
            if (this.IsConnected) return false;
            if (string.IsNullOrWhiteSpace(connection?.HostName)) return false;
            if (string.IsNullOrWhiteSpace(connection.VirtualHost)) return false;
            if (string.IsNullOrWhiteSpace(connection.UserName)) return false;
            if (string.IsNullOrWhiteSpace(connection.Password)) return false;
            if (connection.Port == 0) return false;
            return true;
        }

        private Queue m_selectedQueue;
        public Queue SelectedQueue
        {
            get { return m_selectedQueue; }
            set
            {
                m_selectedQueue = value;
                RaisePropertyChanged("SelectedQueue");
            }
        }
        public DispatcherObject View { get; set; }
    }
}