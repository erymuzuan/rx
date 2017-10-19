﻿using System;
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
                if (m_args.Properties.Headers["log"] is byte[] operationBytes)
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
                if (m_args.Properties.Headers["operation"] is byte[] operationBytes)
                    return ByteToString(operationBytes);

                return null;
            }
        }
        public string MessageId
        {
            get
            {
                if (m_args.Properties.Headers["message-id"] is byte[] operationBytes)
                    return ByteToString(operationBytes);

                return null;
            }
        }

        public T GetValue<T>(string key)
        {
            if (!m_args.Properties.Headers.ContainsKey(key))
                return default;
            var blob = m_args.Properties.Headers[key];
            if (null != blob)
            {
                if (blob.GetType() == typeof(T))
                    return (T) blob;
            }

            if (!(blob is byte[] operationBytes)) return default;
            var sct = ByteToString(operationBytes);
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(sct, out var boolValue))
                {
                    object f = boolValue;
                    return (T) f;
                }
            }

            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(double))
            {
                if (double.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(decimal))
            {
                if (decimal.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(float))
            {
                if (float.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(DateTime))
            {
                if (DateTime.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }

            return default;

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

            if (!(blob is byte[] bytes)) return default(T);
            var sct = ByteToString(bytes);
            if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(sct, out var boolValue))
                {
                    object f = boolValue;
                    return (T) f;
                }
            }

            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(double))
            {
                if (double.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(decimal))
            {
                if (decimal.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(float))
            {
                if (float.TryParse(sct, out var val))
                {
                    object f = val;
                    return (T) f;
                }
            }
            if (typeof(T) == typeof(DateTime))
            {
                if (DateTime.TryParse(sct, out var val))
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
                if (blob is bool b)
                    return b;

                if (blob is byte[] operationBytes)
                {
                    var sct = ByteToString(operationBytes);
                    if (bool.TryParse(sct, out var tryCount))
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
                if (blob is int i)
                    return i;

                if (blob is byte[] operationBytes)
                {
                    var sct = ByteToString(operationBytes);
                    if (int.TryParse(sct, out var tryCount))
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
                if (blob is int i)
                    return i;
                if (blob is long l)
                    return l;

                if (blob is byte[] operationBytes)
                {
                    var sct = ByteToString(operationBytes);
                    if (long.TryParse(sct, out var delayText))
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

                if (m_args.Properties.Headers["username"] is byte[] operationBytes)
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
                if (!m_args.Properties.Headers.ContainsKey("crud")) return crud;
                if (m_args.Properties.Headers["crud"] is byte[] operationBytes)
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
            if (value is DateTime time)
            {
                result = time;
                return true;
            }

            if (value is int i)
            {
                result = i;
                return true;
            }

            if (value is decimal @decimal)
            {
                result = @decimal;
                return true;
            }

            if (value is byte[] operationBytes)
            {
                var r = ByteToString(operationBytes);
                if (binder.ReturnType == typeof(string[]))
                {
                    result = r.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    return true;

                }
                if (binder.ReturnType == typeof(int))
                {
                    if (int.TryParse(r, out var no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(bool))
                {
                    if (bool.TryParse(r, out var no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(decimal))
                {
                    if (decimal.TryParse(r, out var no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(double))
                {
                    if (double.TryParse(r, out var no))
                    {
                        result = no;
                        return true;
                    }
                }
                if (binder.ReturnType == typeof(DateTime))
                    if (DateTime.TryParse(r, out var no))
                    {
                        result = no;
                        return true;
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