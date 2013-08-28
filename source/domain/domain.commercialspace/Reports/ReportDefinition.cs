using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ReportDefinition : Entity, ICustomScript
    {
        public async Task<ObjectCollection<ReportColumn>> GetAvailableColumnsAsync()
        {

            // ReSharper disable PossibleNullReferenceException
            var type = Type.GetType(typeof(Entity).AssemblyQualifiedName.Replace("Entity", this.DataSource.EntityName));
            // ReSharper restore PossibleNullReferenceException
            
            var repository = ObjectBuilder.GetObject<IReportDataSource>();
            var columns = await repository.GetColumnsAsync(type);

            return columns;
        }

        public async Task<ObjectCollection<ReportRow>> ExecuteResultAsync()
        {
            var repository = ObjectBuilder.GetObject<IReportDataSource>();

            var rows = await repository.GetRowsAsync(this);
            return rows;
        }
        /// <summary>
        /// used by script engine for simplified
        /// @Location
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public object Param(string parameterName)
        {
            var parm = this.DataSource.ParameterCollection.SingleOrDefault(p => p.Name == parameterName);
            if (null == parm) return null;
            return parm.Value;
        }

        public string Script
        {
            get
            {
                var script = new StringBuilder();
                foreach (var p in this.DataSource.ParameterCollection)
                {
                    script.AppendLine();
                    script.AppendFormat("var @{0} = item.Param(\"{0}\");", p.Name);
                }
                script.AppendLine();
                return script.ToString();
            }
        }
    }
}
