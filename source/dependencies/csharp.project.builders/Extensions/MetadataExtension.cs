﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{


    internal static class CompilerHelper
    {
        public static void AddReference(this CompilerParameters parameters, params Type[] types)
        {
            foreach (var type in types)
            {
                parameters.ReferencedAssemblies.Add(type.Assembly.Location);
            }
        }
        public static MetadataReference CreateMetadataReference(this Type type)
        {
            return MetadataReference.CreateFromFile(type.Assembly.Location);
        }


        public static void AddMetadataReference<T>(this IList<MetadataReference> list)
        {
            var mt = MetadataReference.CreateFromFile(typeof(T).Assembly.Location);
            list.Add(mt);
        }
        public static void AddMetadataReference(this IList<MetadataReference> list, Type type)
        {
            var mt = MetadataReference.CreateFromFile(type.Assembly.Location);
            list.Add(mt);
        }

        public static MetadataReference CreateMetadataReference<T>(this IProjectDefinition project)
        {
            return MetadataReference.CreateFromFile(typeof(T).Assembly.Location);
        }

        public static CSharpCompilation AddReference<T>(this CSharpCompilation type)
        {
            return type.AddReferences(MetadataReference.CreateFromFile((typeof(T)).Assembly.Location));
        }
        public static Project AddMetadataReference<T>(this Project type)
        {
            return type.AddMetadataReference(MetadataReference.CreateFromFile((typeof(T)).Assembly.Location));
        }

        public static Project AddMetadataReference(this Project project, Type type)
        {
            return project.AddMetadataReference(MetadataReference.CreateFromFile(type.Assembly.Location));
        }

        public static Project AddMetadataReference(this Project project, string location)
        {
            return project.AddMetadataReference(MetadataReference.CreateFromFile(location));
        }
        public static ProjectInfo AddMetadataReference<T>(this ProjectInfo info)
        {
            var list = info.MetadataReferences.ToList();
            list.Add(MetadataReference.CreateFromFile((typeof(T)).Assembly.Location));
            return info.WithMetadataReferences(list.ToArray());
        }


        public static ProjectInfo AddMetadataReference(this ProjectInfo info, Type type)
        {
            var list = info.MetadataReferences.ToList();
            list.Add(MetadataReference.CreateFromFile(type.Assembly.Location));
            return info.WithMetadataReferences(list.ToArray());
        }

        public static ProjectInfo AddMetadataReference(this ProjectInfo info, params string[] locations)
        {
            var list = info.MetadataReferences.ToList();
            var references = locations.Select(x => MetadataReference.CreateFromFile(x));
            list.AddRange(references);
            return info.WithMetadataReferences(list.ToArray());
        }

        public static string FormatCode(this string code)
        {
            var ws = new AdhocWorkspace();
            var info = ProjectInfo.Create(ProjectId.CreateNewId("formatter"),
                VersionStamp.Default, "formatter", "formatter", LanguageNames.CSharp);
            var project = ws.AddProject(info);
            var tree = CSharpSyntaxTree.ParseText(code);
            var doc = project.AddDocument("formatted", tree.GetText());
            Debug.WriteLine("What do we do with doc " + doc);
            var res = Microsoft.CodeAnalysis.Formatting.Formatter.Format(tree.GetRoot(), ws);
            return res.ToFullString();
        }
        public static string FormatCode(this StringBuilder code)
        {
            return code.ToString().FormatCode();
        }

        public static bool HasAsyncAwaitExpression(this CodeExpression expression)
        {
            if (expression.IsEmpty) return false;
            var code = $@" 
public object Evaluate(object item)
{{
	{expression}
}}
""; ";
            var snippet = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(code);
            var root = (CompilationUnitSyntax)snippet.GetRoot();
            var nodes = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .Single(x => x.Identifier.Text == "Evaluate")
                .Body
                .Statements;

            return nodes.SelectMany(x => x.DescendantNodes()).OfType<AwaitExpressionSyntax>().Any();
        }
    }
}
