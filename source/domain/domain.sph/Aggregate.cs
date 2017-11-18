using System;

namespace Bespoke.Sph.Domain
{
    public class Aggregate
    {
        public Aggregate(string name, string path)
        {
            Name = name;
            Path = path;
        }
        public string Name { get; set; }
        public string Path { get; set; }

        public virtual TResult GetValue<TResult>()
        {
            if (null == m_result)
                return default;

            if (typeof(TResult) == typeof(DateTime))
            {
                if (DateTime.TryParse(m_stringResult, out var dt))
                    return (TResult)(object)dt;
            }
            return (TResult)m_result;
        }

        private object m_result;
        private string m_stringResult;

        public virtual void SetValue<TResult>(TResult value)
        {
            m_result = value;
        }

        public virtual void SetStringValue(string stringValue)
        {
            m_stringResult = stringValue;
        }

        public override string ToString()
        {
            return $"({this.GetType().Name}){Name}:{Path}";
        }
    }
}