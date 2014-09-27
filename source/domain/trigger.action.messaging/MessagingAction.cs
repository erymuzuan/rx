using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;

namespace Bespoke.Sph.Messaging
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Messaging", Description = "Re route the message to the specified adapter send port", FontAwesomeIcon = "chevron-circle-right")]
    public class MessagingAction : CustomAction
    {
        public string OutboundMap { get; set; }
        public string Adapter { get; set; }
        public string Operation { get; set; }
        public string Table { get; set; }
        public string Crud { get; set; }
        public int? Retry { get; set; }
        /// <summary>
        /// Interval in miliseconds
        /// </summary>
        public long RetryInterval { get; set; }
        /// <summary>
        /// the multiplies of miliseconds, second = 1000, minute = 60 000 , hour 3 600 000, day =3600000*24
        /// </summary>
        public long RetryIntervalTimeSpan { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public Type OutboundMapType
        {
            get
            {
                return Type.GetType(this.OutboundMap);
            }
            set
            {
                this.OutboundMap = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }
        [XmlIgnore]
        [JsonIgnore]
        public Type AdapterType
        {
            get
            {
                return Type.GetType(this.Adapter);
            }
            set
            {
                this.Adapter = value.GetShortAssemblyQualifiedName();
                RaisePropertyChanged();
            }
        }

        public override bool UseAsync
        {
            get { return true; }
        }

        public override bool UseCode
        {
            get { return true; }
        }

        public override async Task ExecuteAsync(RuleContext context)
        {
            if (string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Operation))
            {
                dynamic adapter = Activator.CreateInstance(this.AdapterType);
                Console.WriteLine(adapter);

            }
            if (!string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Crud))
            {
                var dll = this.AdapterType.Assembly;
                var ttname = this.AdapterType.Namespace + "." + this.Table + "Adapter";
                var tt = dll.GetType(ttname, true);
                if (null == tt) throw new InvalidOperationException(this.AdapterType.Namespace + "." + this.Table);
                dynamic table = Activator.CreateInstance(tt);

                // map
                dynamic map = Activator.CreateInstance(this.OutboundMapType);
                var item = await map.TransformAsync(context.Item);
                await table.InsertAsync(item);

            }


        }

        public override string GeneratorCode()
        {
            var code = new StringBuilder();

            var context = new SphDataContext();
            var map = context.LoadOne<TransformDefinition>(x => x.Id == this.OutboundMap);
            var adapter = context.LoadOne<Adapter>(x => x.Name == this.Adapter);
                        
            code.AppendLinf("var map = new {0}.Integrations.Transforms.{1}();", ConfigurationManager.ApplicationName, map.Name);
            code.AppendLine("var source = await map.TransformAsync(item);");
            if (string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Operation))
            {
                code.AppendLinf("var adapter = new {0}();", this.AdapterType.FullName);
                code.AppendLinf("var response = await adapter.{0}(source);", this.Operation);

            }
            if (!string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Crud))
            {
                code.AppendLinf("var adapter = new {0}.Adapters.{1}.{2}Adapter();", ConfigurationManager.ApplicationName, adapter.Schema, this.Table);
                code.AppendLinf("var response = await adapter.{0}Async(source);", this.Crud);
            }
            code.AppendLine("return response;");
            return code.ToString();
        }


        public override string GetEditorView()
        {
            return Properties.Resources.MessagingActionHtml;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.MessagingActionJs;
        }
    }
}
