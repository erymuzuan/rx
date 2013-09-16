using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ReportDefinition : Entity, ICustomScript
    {
        public async Task<ObjectCollection<ReportColumn>> GetAvailableColumnsAsync()
        {

            // ReSharper disable PossibleNullReferenceException
            var type = Type.GetType(typeof(Entity).AssemblyQualifiedName.Replace("Entity", this.DataSource.EntityName));
            // ReSharper restore PossibleNullReferenceException
            
            var repository = ObjectBuilder.GetObject<IReportDataSource>();
            var columns = await repository.GetColumnsAsync(type).ConfigureAwait(false);

            return columns;
        }

        public async Task<ObjectCollection<ReportRow>> ExecuteResultAsync()
        {
            var repository = ObjectBuilder.GetObject<IReportDataSource>();

            var rows = await repository.GetRowsAsync(this).ConfigureAwait(false);
            return rows;
        }
        /// <summary>
        /// used by script engine for simplified
        /// @Location
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public T Param<T>(string parameterName)
        {
            var parm = this.DataSource.ParameterCollection.SingleOrDefault(p => p.Name == parameterName);
            if (null == parm) return default(T);
            var stringValue = string.Format("{0}", parm.Value);
            if (typeof (T) == typeof (int))
            {
                return (T)(object)int.Parse(stringValue);
            }
            return (T)parm.Value;
        }

        public object Param(string parameterName)
        {
            var parm = this.DataSource.ParameterCollection.SingleOrDefault(p => p.Name == parameterName);
            if (null == parm) return null;
            return parm.Value;
        }

        [JsonIgnore]
        [XmlIgnore]
        public string Script
        {
            get
            {
                var script = new StringBuilder();
                foreach (var p in this.DataSource.ParameterCollection)
                {
                    script.AppendLine();
                    script.AppendFormat("var @{0} = item.Param<{1}>(\"{0}\");", p.Name, p.Type.Name);
                }
                script.AppendLine();
                return script.ToString();
            }
        }
    }
}
