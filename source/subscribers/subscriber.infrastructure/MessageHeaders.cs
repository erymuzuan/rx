using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class MessageHeaders : DynamicObject
    {
        private readonly ReceivedMessageArgs m_args;
        public const string SPH_TRYCOUNT = "sph.trycount";
        public const string SPH_DELAY= "sph.delay";

        public MessageHeaders(ReceivedMessageArgs args)
        {
            m_args = args;
        }

        private string ByteToString(byte[] content)
        {
            using (var orginalStream = new MemoryStream(content))
            {
                using (var sr = new StreamReader(orginalStream))
                {
                    var text = sr.ReadToEnd();
                    return text;
                }
            }
        }

        public AuditTrail Log
        {
            get
            {
                var operationBytes = m_args.Properties.Headers["log"] as byte[];
                if (null != operationBytes)
                {
                    var json = ByteToString(operationBytes);
                    if (string.IsNullOrWhiteSpace(json)) return null;
                    return json.DeserializeFromJson<AuditTrail>();
                }

                return null;
            }
        }

        public string Operation
        {
            get
            {
                var operationBytes = m_args.Properties.Headers["operation"] as byte[];
                if (null != operationBytes)
                    return ByteToString(operationBytes);

                return null;
            }
        }
        public int? TryCount
        {
            get
            {
                if (!m_args.Properties.Headers.ContainsKey(SPH_TRYCOUNT))
                    return null;
                var blob = m_args.Properties.Headers[SPH_TRYCOUNT];
                if (blob is int)
                    return (int) blob;

                var operationBytes =  blob as byte[];
                if (null != operationBytes)
                {
                    var sct = ByteToString(operationBytes);
                    int tryCount;
                    if (int.TryParse(sct, out tryCount))
                        return tryCount;
                }

                return null;
            }
        }
        public long? Delay
        {
            get
            {
                if (!m_args.Properties.Headers.ContainsKey(SPH_DELAY))
                    return null;
                var blob = m_args.Properties.Headers[SPH_DELAY];
                if (blob is int)
                    return (int) blob;
                if (blob is long)
                    return (long) blob;

                var operationBytes =  blob as byte[];
                if (null != operationBytes)
                {
                    var sct = ByteToString(operationBytes);
                    long delayText;
                    if (long.TryParse(sct, out delayText))
                        return delayText;
                }

                return null;
            }
        }

        public string Username
        {
            get
            {
                var operationBytes = m_args.Properties.Headers["username"] as byte[];
                if (null != operationBytes)
                    return ByteToString(operationBytes);

                return null;
            }
        }

        public IDictionary<string, object> GetRawHeaders()
        {
            return m_args.Properties.Headers;
        }
        public CrudOperation Crud
        {
            get
            {
                var crud = CrudOperation.None;
                var operationBytes = m_args.Properties.Headers["crud"] as byte[];
                if (null != operationBytes)
                {
                    var v = ByteToString(operationBytes);
                    if (Enum.TryParse(v, true, out crud))
                        return crud;

                }

                return crud;

            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var operationBytes = m_args.Properties.Headers[binder.Name] as byte[];

            if (null != operationBytes)
            {
                var r = ByteToString(operationBytes);
                if (binder.ReturnType == typeof(string[]))
                {
                    result = r.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return true;

                }
                if (binder.ReturnType == typeof(int))
                {
                    int no;
                    if (int.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(bool))
                {
                    bool no;
                    if (bool.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(decimal))
                {
                    decimal no;
                    if (decimal.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(double))
                {
                    double no;
                    if (double.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(DateTime))
                {
                    DateTime no;
                    if (DateTime.TryParse(r, out no))
                    {
                        result = no;
                        return true;
                    }
                }

                result = r;
                return true;
            }
            result = null;
            return false;
        }

        public override string ToString()
        {
            return this.ToJsonString(true);
        }
    }
}