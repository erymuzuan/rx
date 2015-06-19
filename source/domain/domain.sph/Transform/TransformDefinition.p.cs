using System;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition
    {

        [XmlIgnore]
        [JsonIgnore]
        public Type OutputType
        {
            get
            {
                var t = Type.GetType(this.OutputTypeName);
                if (null == t)
                {
                    var splits = this.OutputTypeName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    var dll = Assembly.LoadFile($"{ConfigurationManager.WorkflowCompilerOutputPath}\\{splits.Last().Trim()}.dll");
                    t = dll.GetType(splits.First().Trim());
                }
                return t;
            }
            set
            {
                this.OutputTypeName = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public Type InputType
        {
            get
            {
                var t = Type.GetType(this.InputTypeName);
                if (null == t)
                {
                    var splits = this.InputTypeName.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    var dll = Assembly.LoadFile($"{ConfigurationManager.WorkflowCompilerOutputPath}\\{splits.Last().Trim()}.dll");
                    t = dll.GetType(splits.First().Trim());
                }
                return t;
            }
            set
            {
                this.InputTypeName = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

       
    }
}
