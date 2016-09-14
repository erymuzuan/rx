using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RequestWithoutBodyApiOperationDefinition
    {

        public override IEnumerable<Class> GenerateRequestCode()
        {
            var list = base.GenerateRequestCode().ToList();
            list.RemoveAll(x => x.Name == $"{MethodName}Request");
            var request = new Class { Name = $"{MethodName}Request", Namespace = CodeNamespace };

            request.AddProperty($"public {MethodName}QueryString QueryStrings {{get;set;}} = new {MethodName}QueryString();");
            request.AddProperty($"public {MethodName}RequestHeader Headers {{get;set;}} = new {MethodName}RequestHeader();");

            list.Add(request);
            return list;
        }
        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var uri = new Uri(this.BaseAddress);
            var opUri = uri.LocalPath;
            var code = new StringBuilder();

            code.AppendLine(base.GenerateRequestQueryStringsCode(ref opUri));
            code.AppendLine(this.GenerateRequestHeadersCode((RestApiAdapter)adapter));

            if (this.ErrorRetry.IsEnabled)
            {
                code.AppendLine($@"
                            var url = $@""{opUri}"";
                            var result await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync({this.ErrorRetry.GenerateWaitCode()})
	                                .ExecuteAndCaptureAsync(async() => m_client.GetAsync(url));

                            if(null != result.FinalException)
	                            throw result.FinalException;
                            var response = await m_client.GetAsync(url);

                            ");
            }
            else
            {
                code.AppendLine($@" 
                            var url = $@""{opUri}"";
                            var response = await m_client.GetAsync(url);

                           ");
            }

            code.AppendLine(base.GenerateProcessHttpResonseCode());

            return code.ToString();
        }
    }
}