using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource]
    public partial class ReportDefinition : Entity, ICustomScript
    {
        public Task<BuildValidationResult> ValidateBuildAsync()
        {
            var result = new BuildValidationResult();
            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9_ ]*$");
            if (!validName.Match(this.Title).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Title must start a with letter.You cannot use symbol or number as first character" });

           
            if (string.IsNullOrWhiteSpace(this.DataSource.EntityName))
                result.Errors.Add(new BuildError(this.WebId) { Message = "You have not select an entity for your report" });

           


            result.Result = result.Errors.Count == 0;
            return Task.FromResult(result);
        }


        public async Task<ObjectCollection<ReportColumn>> GetAvailableColumnsAsync()
        {

            var type = this.DataSource.EntityName;

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
            if (typeof(T) == typeof(int))
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
