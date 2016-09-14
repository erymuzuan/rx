using System;
using System.Text;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RequestWithoutBodyApiOperationDefinition
    {
        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            var uri = new Uri(this.BaseAddress);
            var opUri = uri.LocalPath;
            var code = new StringBuilder();

            code.AppendLine(base.GenerateRequestQueryStringsCode(ref opUri));
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