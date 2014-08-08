using System;
using System.ComponentModel.Composition;
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
      

        public override bool UseAsync
        {
            get { return true; }
        }

        public override Task ExecuteAsync(RuleContext context)
        {
            Console.WriteLine("....");
            return base.ExecuteAsync(context);
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
