using System;
using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public class LabelItemScriptHost : Entity, ICustomScript
    {
        private readonly IEnumerable<ReportRow> m_rows;
        private readonly ReportDefinition m_rdl;

        public LabelItemScriptHost(IEnumerable<ReportRow> rows, ReportDefinition rdl)
        {
            m_rows = rows;
            m_rdl = rdl;
        }

        public object Sum(string column)
        {
            if (!m_rows.Any()) return decimal.Zero;
            if (m_rows.All(r => null == r[column])) return decimal.Zero;
            var type = m_rows.First()[column].Type;
            if (type == typeof(int))
                return m_rows.Where(r => null != r[column]).Sum(r => Convert.ToInt32(r[column].Value));
            if (type == typeof(decimal))
                return m_rows.Where(r => null != r[column]).Sum(r => Convert.ToDecimal(r[column].Value));
            if (type == typeof(double))
                return m_rows.Where(r => null != r[column]).Sum(r => Convert.ToDouble(r[column].Value));
            if (type == typeof(float))
                return m_rows.Where(r => null != r[column]).Sum(r => Convert.ToSingle(r[column].Value));

            throw new Exception(type.FullName + " is not supported for Sum");
        }

        public object Average(string column)
        {
            if (!m_rows.Any()) return null;
            if (m_rows.All(r => null == r[column])) return null;

            var type = m_rows.First()[column].Type;
            if (type == typeof(int))
                return m_rows.Where(r => null != r[column]).Average(r => Convert.ToInt32(r[column].Value));
            if (type == typeof(decimal))
                return m_rows.Where(r => null != r[column]).Average(r => Convert.ToDecimal(r[column].Value));
            if (type == typeof(double))
                return m_rows.Where(r => null != r[column]).Average(r => Convert.ToDouble(r[column].Value));
            throw new Exception(type.FullName + " is not supported for Average");
        }

        public object Min(string column)
        {
            if (!m_rows.Any()) return null;
            if (m_rows.All(r => null == r[column])) return null;

            var rows2 = m_rows.Where(r => null != r[column])
                .Select(r => r[column].Value);
            return rows2.Min();


        }

        public object Max(string column)
        {
            if (!m_rows.Any()) return null;
            if (m_rows.All(r => null == r[column])) return null;

            var rows2 = m_rows.Where(r => null != r[column])
                .Select(r => r[column].Value);
            return rows2.Max();
        }

        public object Param(string parameterName)
        {
            var parm = this.m_rdl.DataSource.ParameterCollection.SingleOrDefault(p => p.Name == parameterName);
            if (null == parm) return null;
            return parm.Value;
        }

        public string Script { get { return string.Empty; } }
    }
}