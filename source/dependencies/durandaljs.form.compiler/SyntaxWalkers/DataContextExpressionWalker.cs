using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(typeof(CustomObjectSyntaxWalker))]
    class DataContextExpressionWalker : CustomObjectSyntaxWalker
    {
        public const string DATA_CONTEXT = "DataContext";
        protected override SyntaxKind[] Kinds
        {
            get { return new[] { SyntaxKind.SimpleMemberAccessExpression, SyntaxKind.GenericName }; }
        }

        public override CustomObjectModel GetObjectModel(IProjectProvider project)
        {
            var code = new StringBuilder();
            code.AppendLine("using System;");
            code.AppendLine("using System.Collections.Generic;");
            code.AppendLine("using System.Threading.Tasks;");
            code.AppendLine("using Bespoke.Sph.Domain;");
            code.AppendLine("using System.Linq;");
            code.AppendLine("using System.Linq.Expressions;");

            code.AppendLine("namespace " + project.DefaultNamespace);
            code.AppendLine("{");

            code.AppendLine("   public class DataContext");
            code.AppendLine("   {");
            code.AppendLine("       public Task<T> LoadOneAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<LoadOperation<T>> LoadAsync<T>(Expression<Func<T, bool>> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<LoadOperation<T>> SearchAsync<T>(object queryDsl) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<List<Tuple<T1,T2>>> GetTuplesAsync<T, T1,T2>(string member1, string member2, Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<List<Tout>> GetListAsync<T, Tout>(string member, Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<List<Tout>> GetDistinctAsync<T, Tout>(string member, Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<int> GetCountAsync<T>(Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<int> GetSumAsync<T>(string member, Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<int> GetMinAsync<T>(string member, Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<int> GetMaxAsync<T>(string member, Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<Tout> GetScalarAsync<T, Tout>(string member, Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<string> PostAsync<T>(Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<string> SendAsync<T>(Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("       public Task<string> GetAsync<T>(Func<T, bool> predicate) where T : Entity { throw new Exception();}");
            code.AppendLine("   }");
            code.AppendLine("}");
            var com = new CustomObjectModel
            {
                SyntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code.ToString()),
                IncludeAsParameter = true,
                ClassName = DATA_CONTEXT,
                IdentifierText = "context"
            };
            return com;
        }


        protected override bool Filter(IParameterSymbol parameter)
        {
            return parameter.Name == "context";
        }

        protected override bool Filter(IMethodSymbol method)
        {
            return method.ContainingType.Name == DATA_CONTEXT
                   && method.ContainingAssembly.Name == EVAL;
        }

        protected override string InferredTypeName
        {
            get { return DATA_CONTEXT; }
        }

        public override string Walk(SyntaxNode node, SemanticModel model)
        {
            if (node.CSharpKind() != SyntaxKind.GenericName)
                return base.Walk(node, model);

            var genericSyntax = (GenericNameSyntax) node;
            var types = string.Join(", ", genericSyntax.TypeArgumentList.Arguments.Select(x => x));

            var text = genericSyntax.Identifier.Text;
            var compiler = this.IdentifierCompilers.LastOrDefault(x => x.Metadata.Text == text && x.Metadata.TypeName == this.InferredTypeName);
            if (null != compiler)
            {
                var argumentList = this.GetArguments(genericSyntax).ToList();
                var xp = compiler.Value.Compile(genericSyntax, argumentList, genericSyntax.TypeArgumentList.Arguments);
                return xp;
            }


            return string.Format("{0}<{1}>", genericSyntax.Identifier.Text, types);
        }
    }
}