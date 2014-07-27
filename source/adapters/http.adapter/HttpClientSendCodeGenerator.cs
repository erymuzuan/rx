using System;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class HttpClientSendCodeGenerator
    {
        public virtual string GenerateCode(HttpOperationDefinition operation)
        {
            return "throw new Exception(\"No code is generated\");";
        }

        public static HttpClientSendCodeGenerator Create(HttpOperationDefinition op)
        {
            switch (op.HttpMethod)
            {
                case "GET":
                    return new GetCodeGenerator();
                case "DELETE": return new DeleteCodeGenerator();
                case "POST":
                    if (!string.IsNullOrWhiteSpace(op.GetRequestHeader("Content-Type")))
                    {
                        return new PostWithContentTypeCodeGenerator();
                    }
                    return new PostCodeGenerator();
                case "PUT": return new PutCodeGenerator();
                case "PATCH": return new PatchCodeGenerator();
                default: throw new Exception("Not supported METHOD");
            }
        }
    }
}