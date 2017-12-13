using System;
using System.Collections.Generic;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.Domain.Messaging
{
    public class BrokeredMessage 
    {
        public string RoutingKey { get; set; }
        public string Id { get; set; }
        public Dictionary<string, object> Headers { get; } = new Dictionary<string, object>();
        public byte[] Body { get; set; }
        public string Operation { get; set; }
        public CrudOperation Crud { get; set; }
        public int? TryCount { get; set; }
        public string Username { get; set; }
        public string ReplyTo { get; set; }

        public bool IsDataImport => HasDataImport(this);
        public TimeSpan RetryDelay { get; set; }

        private static bool HasDataImport(BrokeredMessage message)
        {
            var headers = message.Headers;
            if (headers.ContainsKey("data-import"))
            {
                if (headers["data-import"] is bool dv)
                {
                    if (!dv)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
            return false;
        }
        
        
        public T GetValue<T>(string key)
        {
            if (!this.Headers.ContainsKey(key))
                return default;
            var blob = this.Headers[key];
            if (null != blob)
            {
                if (blob.GetType() == typeof(T))
                    return (T) blob;
            }

            if (!(blob is byte[] operationBytes)) return default;
            var sct = operationBytes.ReadString();
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
            if (!this.Headers.ContainsKey(key))
                return new T?();
            var blob = this.Headers[key];
            if (null != blob)
            {
                if (blob.GetType() == typeof(T))
                    return (T) blob;
            }

            if (!(blob is byte[] bytes)) return default(T);
            var sct = bytes.ReadString();
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
    }
}