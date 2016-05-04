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
        public const string SPH_DELAY = "sph.delay";

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

        public T GetValue<T>(string key)
        {
            if (!m_args.Properties.Headers.ContainsKey(key))
                return default(T);
            var blob = m_args.Properties.Headers[key];
            if (null != blob)
            {
                if (blob.GetType() == typeof(T))
                    return (T) blob;
            }

            var operationBytes = blob as byte[];
            if (null == operationBytes) return default(T);
            var sct = ByteToString(operationBytes);
            if (typeof(T) == typeof(bool))
            {
                bool boolValue;
                if (bool.TryParse(sct, out boolValue))
                {
                    object f = boolValue;
                    return (T) f;
                }
            }

            if (typeof(T) == typeof(int))
            {
                int val;
                if (int.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(double))
            {
                double val;
                if (double.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(decimal))
            {
                decimal val;
                if (decimal.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(float))
            {
                float val;
                if (float.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(DateTime))
            {
                DateTime val;
                if (DateTime.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }

            return default(T);

        }

        public T? GetNullableValue<T>(string key) where T: struct 
        {
            if (!m_args.Properties.Headers.ContainsKey(key))
                return new T?();
            var blob = m_args.Properties.Headers[key];
            if (null != blob)
            {
                if (blob.GetType() == typeof(T))
                    return (T) blob;
            }

            var bytes = blob as byte[];
            if (null == bytes) return default(T);
            var sct = ByteToString(bytes);
            if (typeof(T) == typeof(bool))
            {
                bool boolValue;
                if (bool.TryParse(sct, out boolValue))
                {
                    object f = boolValue;
                    return (T) f;
                }
            }

            if (typeof(T) == typeof(int))
            {
                int val;
                if (int.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(double))
            {
                double val;
                if (double.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(decimal))
            {
                decimal val;
                if (decimal.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(float))
            {
                float val;
                if (float.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(DateTime))
            {
                DateTime val;
                if (DateTime.TryParse(sct, out val))
                {
                    object f = val;
                    return (T) f;
                }
            }

            return new T?(); 

        }
        public bool? LogAuditTrail
        {
            get
            {
                const string KEY = "log.audit.trail";
                if (!m_args.Properties.Headers.ContainsKey(KEY))
                    return null;
                var blob = m_args.Properties.Headers[KEY];
                if (blob is bool)
                    return (bool)blob;

                var operationBytes = blob as byte[];
                if (null != operationBytes)
                {
                    var sct = ByteToString(operationBytes);
                    bool tryCount;
                    if (bool.TryParse(sct, out tryCount))
                        return tryCount;
                }

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
                    return (int)blob;

                var operationBytes = blob as byte[];
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
                    return (int)blob;
                if (blob is long)
                    return (long)blob;

                var operationBytes = blob as byte[];
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
                var user = m_args.Properties.Headers["username"] as string;
                if (!string.IsNullOrWhiteSpace(user))
                    return user;

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
            result = null;
            if (!m_args.Properties.Headers.ContainsKey(binder.Name))
                return false;
            var value = m_args.Properties.Headers[binder.Name];
            if (null == value) return true;

            if (value is string)
            {
                result = value as string;
                return true;
            }
            if (value is DateTime)
            {
                result = (DateTime)value;
                return true;
            }

            if (value is int)
            {
                result = (int)value;
                return true;
            }

            if (value is decimal)
            {
                result = (decimal)value;
                return true;
            }

            var operationBytes = value as byte[];

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
            return false;
        }

        public override string ToString()
        {
            return this.ToJsonString(true);
        }
    }
}