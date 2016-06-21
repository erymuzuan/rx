using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class OperationDefinition : DomainObject
    {
        public string CodeNamespace { get; set; }
        protected virtual string[] RequestCodeClassUsingImports => new[] { typeof(string).Namespace, typeof(Enumerable).Namespace, typeof(DomainObject).Namespace };
        protected virtual string[] ResponseCodeClassUsingImports => new[] { typeof(string).Namespace, typeof(Enumerable).Namespace , typeof(DomainObject).Namespace};

        public virtual IEnumerable<Class> GenerateRequestCode()
        {
            var @class = new Class { Name = this.Name.ToCsharpIdentitfier() + "Request", BaseClass = nameof(DomainObject), Namespace = CodeNamespace };
            @class.AddNamespaceImport<DateTime, DomainObject>();
            var sources = new ObjectCollection<Class> { @class };

            var properties = from m in this.RequestMemberCollection
                             select new Property { Code = m.GeneratedCode("   ") };
            @class.PropertyCollection.ClearAndAddRange(properties);

            var otherClasses = this.RequestMemberCollection
                            .Select(m => m.GeneratedCustomClass(this.CodeNamespace, this.RequestCodeClassUsingImports))
                            .SelectMany(x => x.ToArray());
            sources.AddRange(otherClasses);

            return sources;
        }

        public virtual IEnumerable<Class> GenerateResponseCode()
        {
            var @class = new Class { Name = this.Name.ToCsharpIdentitfier() + "Response", Namespace = CodeNamespace };
            @class.AddNamespaceImport<DateTime, DomainObject>();
            var sources = new ObjectCollection<Class> { @class };

            var properties = from m in this.ResponseMemberCollection
                             select new Property { Code = m.GeneratedCode("   ") };
            @class.PropertyCollection.ClearAndAddRange(properties);


            var otherClasses = this.ResponseMemberCollection
                            .Select(m => m.GeneratedCustomClass(this.CodeNamespace, this.ResponseCodeClassUsingImports))
                            .SelectMany(x => x.ToArray());
            sources.AddRange(otherClasses);

            return sources;
        }

        public virtual string GenerateActionCode(Adapter adapter)
        {

            var code = new StringBuilder();
            var methodName = this.MethodName.ToCsharpIdentitfier();
            code.AppendLine($"       public async Task<{methodName}Response> {methodName}Async({methodName}Request request)");
            code.AppendLine("       {");
            code.AppendLine(this.GenerateAdapterActionBody(adapter));
            code.AppendLine("}");
            return code.ToString();
        }
        protected abstract string GenerateAdapterActionBody(Adapter adapter);

        public string Uuid { get; set; }
    }
}
