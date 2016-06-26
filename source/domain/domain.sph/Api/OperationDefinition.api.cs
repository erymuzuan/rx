using System.Text;

namespace Bespoke.Sph.Domain.Api
{
    public partial class OperationDefinition
    {
        /// <summary>
        /// When the sproc is safe and idempotent , we could use GET
        /// </summary>
        public bool UseHttpGet { get; set; }
        public string GenerateApiCode(Adapter adapter)
        {
            if (this.UseHttpGet)
            {
                return GenerateGetApiCode(adapter);
            }
            var code = new StringBuilder();
            var action = this.MethodName.ToCsharpIdentitfier();
            code.AppendLine("[HttpPost]");
            code.AppendLine($@"[Route(""{this.MethodName.ToIdFormat()}"")]");
            code.AppendLine($"public async Task<IHttpActionResult> {action}([FromBody]{action}Request  request)");
            code.AppendLine("{");

            code.AppendLine(GenerateActionBody(adapter, action));
            code.AppendLine("}");
            return code.ToString();
        }

        private string GenerateGetApiCode(Adapter adapter)
        {
            var code = new StringBuilder();
            var action = this.MethodName.ToCsharpIdentitfier();
            var parameters = this.RequestMemberCollection.ToString(", ", x => $@"[FromUri(Name=""{x.Name.ToCamelCase()}"")]" + x.GenerateParameterCode());
            var routesParamters = this.RequestMemberCollection.ToString("/", x => "{" + x.Name.ToCamelCase() + "}");

            code.AppendLine("[HttpGet]");
            code.AppendLine($@"[Route(""{this.MethodName.ToIdFormat()}/{routesParamters}"")]");
            code.AppendLine($"public async Task<IHttpActionResult> {action}({parameters})");
            code.AppendLine("{");

            code.AppendLine($"   var request = new {action}Request();");
            var values = this.RequestMemberCollection.ToString("\r\n", x => $"request.{x.Name} = {x.Name.ToCamelCase()};");
            code.AppendLine(values);

            code.AppendLine(GenerateActionBody(adapter, action));
            code.AppendLine("}");
            return code.ToString();
        }


        private string GenerateActionBody(Adapter adapter, string action)
        {
            var code = new StringBuilder();
            code.AppendLine($"  var adapter = new {adapter.Name}();");
            if (this.ErrorRetry.IsEnabled)
            {


                code.AppendLine(
                    $@"	        
                var result = await Policy.Handle<Exception>()
	                                .WaitAndRetryAsync({this.ErrorRetry.GenerateWaitCode()})
	                                .ExecuteAndCaptureAsync(async() =>  await adapter.{action}Async(request) );

                if(null != result.FinalException)
	                throw result.FinalException;

                var response = result.Result;
");
            }
            else
            {
                code.AppendLine($"  var response = await adapter.{action}Async(request);");
            }
            code.AppendLine("   return Ok(response);");

            return code.ToString();
        }
    }
}
