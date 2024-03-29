﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Messaging
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Messaging", TypeName = "Bespoke.Sph.Messaging.MessagingAction, trigger.action.messaging", Description = "Re route the message to the specified adapter send port", FontAwesomeIcon = "chevron-circle-right")]
    public partial class MessagingAction : CustomAction
    {

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
            var rt = new ErrorRetry();
            var hasRetry = this.Retry.HasValue;
            if (hasRetry)
            {
                rt = new ErrorRetry
                {
                    Algorithm = this.RetryAlgorithm,
                    WebId = this.WebId,
                    Attempt = this.Retry.Value,
                    Wait = Convert.ToInt32(this.RetryIntervalTimeSpan * this.RetryInterval),
                    IsEnabled = true
                };
            }

            code.AppendLine($"var map = new {map.CodeNamespace}.{map.ClassName}();");
            if (hasRetry)
            {
                code.AppendLine($@" var sourceRetry = await Policy.Handle<Exception>()
                                                    .WaitAndRetryAsync({rt.GenerateWaitCode()})
                                                    .ExecuteAndCaptureAsync(async () => await map.TransformAsync(item));");
                code.AppendLine("if(null != sourceRetry.FinalException)");
                code.AppendLine("    throw sourceRetry.FinalException;");
                code.AppendLine("var source = sourceRetry.Result;");
            }
            else
                code.AppendLine("var source = await map.TransformAsync(item);");
            var useOperation = string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Operation);
            var useTable = !string.IsNullOrWhiteSpace(this.Table) && !string.IsNullOrWhiteSpace(this.Crud);


            if (useOperation)
            {
                var op = adapter.OperationDefinitionCollection.Single(x => x.Name == this.Operation);
                code.AppendLine($"var adapter = new {AdapterType.FullName}();");
                if (hasRetry)
                {
                    code.AppendLine($@" var pr = await Policy.Handle<Exception>()
                                                    .WaitAndRetryAsync({rt.GenerateWaitCode()})
                                                    .ExecuteAndCaptureAsync(async () => await adapter.{op.MethodName}Async(source));");
                    code.AppendLine("if(null != pr.FinalException)");
                    code.AppendLine("    throw pr.FinalException;");
                    code.AppendLine("var response = pr.Result;");
                }
                else
                    code.AppendLine($"var response = await adapter.{op.MethodName}Async(source);");

            }
            if (useTable)
            {
                var table = adapter.TableDefinitionCollection.Single(x => x.Name == this.Table);
                code.AppendLine($"var adapter = new {adapter.CodeNamespace}.{table.ClrName}Adapter();");
                if (hasRetry)
                {
                    code.AppendLine($@" var pr = await Policy.Handle<Exception>()
                                                    .WaitAndRetryAsync({rt.GenerateWaitCode()})
                                                    .ExecuteAndCaptureAsync(async () => await adapter.{Crud}Async(source));");
                    code.AppendLine("if(null != pr.FinalException)");
                    code.AppendLine("    throw pr.FinalException;");
                    code.AppendLine("var response = pr.Result;");
                }
                else
                {
                    code.AppendLine($"var response = await adapter.{Crud}Async(source);");
                }
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
