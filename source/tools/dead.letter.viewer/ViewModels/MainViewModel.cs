using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Bespoke.Station.Windows.RabbitMqDeadLetter.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Linq;
using System.Text;
using System.Windows;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter.ViewModels
{
    [Export]
    public partial class MainViewModel : ViewModelBase, IView
    {
        public RelayCommand<object> DeleteCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand SendCommand { get; set; }
        public RelayCommand RequeueCommand { get; set; }
        public RelayCommand DiscardMessageCommand { get; set; }
        public RelayCommand<BasicGetResult> DecompressMessageCommand { get; set; }
        public RelayCommand<string> FormatMessageCommand { get; set; }
        public RelayCommand<XDeathHeader> AutomaticallyRequeCommand { set; get; }

        [Import]
        public ConnectionViewModel Connection { set; get; }

        public MainViewModel()
        {
            this.LoadCommand = new RelayCommand(Load, () => Connection.IsConnected && null != Connection.SelectedQueue);
            this.SendCommand = new RelayCommand(Send, CanSend);
            this.RequeueCommand = new RelayCommand(Requeue, () => Connection.IsConnected && null != this.Result);
            this.DiscardMessageCommand = new RelayCommand(DiscardMessage, () => Connection.IsConnected && null != this.Result);
            this.DecompressMessageCommand = new RelayCommand<BasicGetResult>(Decompress, b => null != b);
            this.FormatMessageCommand = new RelayCommand<string>(FormatJson, b => !string.IsNullOrWhiteSpace(b));
            this.AutomaticallyRequeCommand = new RelayCommand<XDeathHeader>(AutomaticallyReque, xd => null != xd);
        }
        private readonly List<XDeathHeader> m_autoRequeList = new List<XDeathHeader>();
        private void AutomaticallyReque(XDeathHeader obj)
        {
            m_autoRequeList.Add(obj);
        }


        private bool CanSend()
        {
            if (null == this.Connection) return false;

            return Connection.IsConnected
                && null != this.Result
                && null != Connection.Exchange
                && !string.IsNullOrWhiteSpace(this.RoutingKey)
                ;
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            if (propertyName == "IsConnected" && null != this.LoadCommand)
                this.LoadCommand.RaiseCanExecuteChanged();
            if (propertyName == "SelectedQueue" && null != this.LoadCommand)
                this.LoadCommand.RaiseCanExecuteChanged();
            if (propertyName == "Result" && null != this.DiscardMessageCommand)
                this.DiscardMessageCommand.RaiseCanExecuteChanged();
            if (propertyName == "Result" && null != this.SendCommand)
                this.SendCommand.RaiseCanExecuteChanged();
            if (propertyName == "Exchange" && null != this.SendCommand)
                this.SendCommand.RaiseCanExecuteChanged();

            base.RaisePropertyChanged(propertyName);

        }

        private async void Requeue()
        {
            this.Connection.Channel.BasicAck(this.Result.DeliveryTag, false);
            byte[] body;
            if (this.IsCompress)
                body = await CompressAsync(this.Message);
            else
                body = System.Text.Encoding.UTF8.GetBytes(this.Message);

            var props = this.Connection.Channel.CreateBasicProperties();
            props.DeliveryMode = 2;
            props.ContentType = "application/json";
            props.Headers = this.Result.BasicProperties.Headers;

            this.Connection.Channel.BasicPublish(this.Connection.Exchange.Name, this.RoutingKey, props, body);

            this.Result = null;
            this.Message = string.Empty;
            this.DeathHeader = null;


            if (this.IsAfterProcessGetNextMessage)
            {

                this.Load();
                if (null == this.Result) return;
                if (this.IsNextMessageDecompress)
                    this.Message = await DecompressAsync(this.Result.Body);
                if (this.IsNextMessageReformat)
                    this.FormatJson(this.Message);
                if (m_autoRequeList.Any(d => d.Equals(this.DeathHeader)))
                {
                    this.Requeue();

                }


            }
        }

        private void DiscardMessage()
        {
            Connection.Channel.BasicAck(this.Result.DeliveryTag, false);
            this.Result = null;
            this.Message = string.Empty;
        }

        private void Send()
        {
            Connection.Channel.BasicAck(this.Result.DeliveryTag, false);
            this.Result = null;

            // send
            var message = System.Text.Encoding.UTF8.GetBytes(this.Message);
            Connection.Channel.BasicPublish(Connection.Exchange.Name, this.RoutingKey, null, message);
        }

        public void OnViewReady()
        {
            this.IsBusy = true;
            this.QueueUserWorkItem(Load);
        }


        private void Load()
        {
            var result = Connection.Channel.BasicGet(Connection.SelectedQueue.Name, false);
            if (null == result)
                return;

            if (string.IsNullOrWhiteSpace(result.BasicProperties.ReplyTo))
                result.BasicProperties.ReplyTo = "empty";


            this.DeathHeader = new XDeathHeader(result.BasicProperties.Headers);
            this.RoutingKey = this.DeathHeader.RoutingValuesKeys;
            this.Connection.Exchange =
                this.Connection.ExchangeCollection.SingleOrDefault(e => e.Name == this.DeathHeader.Exchange);
            this.Result = result;
            this.Message = System.Text.Encoding.UTF8.GetString(this.Result.Body);
            this.Post(() => this.IsBusy = false);

        }


        private async void Decompress(BasicGetResult result)
        {
            try
            {
                this.Message = await DecompressAsync(result.Body);
            }
            catch (Exception e)
            {
                var text = new StringBuilder();
                while (null != e)
                {
                    text.AppendLine(e.ToString());
                    e = e.InnerException;
                }
                MessageBox.Show(text.ToString(), "There's an exception trying to decompress the result",
                    MessageBoxButton.OK);
            }
        }

        private void FormatJson(string message)
        {
            var jo = JsonConvert.DeserializeObject(message);
            this.Message = JsonConvert.SerializeObject(jo, Formatting.Indented);
        }

        private static async Task<string> DecompressAsync(byte[] content)
        {

            using (var orginalStream = new MemoryStream(content))
            using (var destinationStream = new MemoryStream())
            using (var gzip = new GZipStream(orginalStream, CompressionMode.Decompress))
            {
                try
                {
                    await gzip.CopyToAsync(destinationStream);
                }
                catch (InvalidDataException)
                {
                    orginalStream.CopyTo(destinationStream);
                }

                destinationStream.Position = 0;
                using (var sr = new StreamReader(destinationStream))
                {
                    var json = await sr.ReadToEndAsync();
                    return json;
                }
            }

        }


        private async Task<byte[]> CompressAsync(string value)
        {

            var content = new byte[value.Length];
            int index = 0;
            foreach (char item in value)
            {
                content[index++] = (byte)item;
            }


            var ms = new MemoryStream();
            var sw = new GZipStream(ms, CompressionMode.Compress);

            await sw.WriteAsync(content, 0, content.Length);
            //NOTE : DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            content = ms.ToArray();

            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return content;
        }



    }
}
