using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Models
{
    public class XDeathHeader
    {
        protected bool Equals(XDeathHeader other)
        {
            return string.Equals(Reason, other.Reason)
                && string.Equals(this.Queue, other.Queue)
                && string.Equals(Exchange, other.Exchange)
                && this.RoutingKeys.Length == other.RoutingKeys.Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Reason != null ? Reason.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Queue != null ? Queue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Exchange != null ? this.Exchange.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.RoutingKeys != null ? this.RoutingKeys.GetHashCode() : 0);
                return hashCode;
            }
        }


        public XDeathHeader(IDictionary<string, object> entries)
        {
            if (null == entries) return;
            if (!entries.ContainsKey("x-death")) return;
            var vals = entries["x-death"] as ArrayList;
            if (null == vals) return;
            if (vals.Count == 0) return;
            var hash = vals[0] as Hashtable;
            if (null == hash) return;

            this.Reason = hash.GetStringValue("reason");
            this.Queue = hash.GetStringValue("queue");
            this.Exchange = hash.GetStringValue("exchange");
            this.Time = hash.GetDateTimeValue("time");

            var rks = hash["routing-keys"] as ArrayList;
            if (null == rks) return;
            Debug.WriteLine(rks);

            this.RoutingKeys = rks.GetStringValues();
        }



        public string[] RoutingKeys { get; set; }

        [Description("Roting keys comma seperated")]
        public string RoutingValuesKeys
        {
            get
            {
                return null == this.RoutingKeys ? string.Empty :
                    string.Join(",", this.RoutingKeys);
            }
        }

        public string Exchange { get; set; }
        public DateTime? Time { get; set; }
        public string Queue { get; set; }
        public string Reason { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((XDeathHeader)obj);
        }
    }
}