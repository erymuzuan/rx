using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter.Models
{
    public class MessageHeader : ModelBase
    {
        public string Header { get; set; }
        public string Value { get; set; }
    }
    public class XDeathHeader : ModelBase
    {
        protected bool Equals(XDeathHeader other)
        {
            return string.Equals(m_reason, other.m_reason)
                && string.Equals(m_queue, other.m_queue)
                && string.Equals(m_exchange, other.m_exchange)
                && m_routingKeys.Length == other.m_routingKeys.Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
// ReSharper disable NonReadonlyFieldInGetHashCode
                int hashCode = (m_reason != null ? m_reason.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (m_queue != null ? m_queue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (m_exchange != null ? m_exchange.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (m_routingKeys != null ? m_routingKeys.GetHashCode() : 0);
                return hashCode;
// ReSharper restore NonReadonlyFieldInGetHashCode
            }
        }

        private string m_reason;
        private string m_queue;
        private DateTime? m_time;

        static string GetStringValue(object vals)
        {
            try
            {
                if (null != vals)
                    return Encoding.UTF8.GetString((byte[])vals);
                return null;
            }
            catch (Exception e)
            {
                return "Exception: " + e.Message;
            }

        }

        public XDeathHeader(IDictionary<string, object> entries)
        {
            if (null == entries) return;

            var temp = entries.Keys
                .Where(key => key != "x-death")
                .ToDictionary(key => key, key => GetStringValue(entries[key]));
            this.OtherHeaders = JsonConvert.SerializeObject(temp, Formatting.Indented);
            if (entries.ContainsKey("log"))
                this.Log = GetStringValue(entries["log"]);


            if (!entries.ContainsKey("x-death")) return;
            //entries["x-death"].GetType().FullName
          //  "System.Collections.Generic.List`1[[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"//
            var vals = entries["x-death"] as IList<object>;
            if (null == vals) return;
            if (vals.Count == 0) return;
            //vals[0].GetType().FullName
            // "System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"
            var hash = vals[0] as IDictionary<string,object>;
            if (null == hash) return;

            this.Reason = hash.GetStringValue("reason");
            this.Queue = hash.GetStringValue("queue");
            this.Exchange = hash.GetStringValue("exchange");
            this.Time = hash.GetDateTimeValue("time");

            var rks = hash["routing-keys"] as IList<object>;
            if (null == rks) return;
            Debug.WriteLine(rks);

            this.RoutingKeys = rks.GetStringValues();
        }


        private string m_exchange;
        private string[] m_routingKeys;
        private string m_otherHeaders;
        private string m_log;

        public string Log
        {
            get { return m_log; }
            set
            {
                m_log = value;
                RaisePropertyChanged();
            }
        }


        public string OtherHeaders
        {
            get { return m_otherHeaders; }
            set
            {
                m_otherHeaders = value;
                RaisePropertyChanged();
            }
        }

        [Browsable(false)]
        public string[] RoutingKeys
        {
            get { return m_routingKeys; }
            set
            {
                m_routingKeys = value;
                RaisePropertyChanged();
            }
        }

        [Description("Roting keys comma seperated")]
        public string RoutingValuesKeys
        {
            get
            {
                return null == this.RoutingKeys ? string.Empty :
                    string.Join(",", this.RoutingKeys);
            }
        }

        public string Exchange
        {
            get { return m_exchange; }
            set
            {
                m_exchange = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? Time
        {
            get { return m_time; }
            set
            {
                m_time = value;
                RaisePropertyChanged();
            }
        }
        public string Queue
        {
            get { return m_queue; }
            set
            {
                m_queue = value;
                RaisePropertyChanged();
            }
        }
        public string Reason
        {
            get { return m_reason; }
            set
            {
                m_reason = value;
                RaisePropertyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((XDeathHeader)obj);
        }
    }
}