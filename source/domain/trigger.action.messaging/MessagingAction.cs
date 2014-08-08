using System;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
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

        public override string GeneratorCode()
        {
            var code = new StringBuilder();
            code.AppendLinf("var map = new {0}();", this.OutboundMapType.FullName);
            code.AppendLine("var source = await map.TramsformAsync(item);");
            code.AppendLinf("var adapter = new {0}();", this.AdapterType.FullName);
            code.AppendLinf("var response = await adapter.{0}(source);", this.Operation);
            code.AppendLinf("return response;", this.Operation);
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
