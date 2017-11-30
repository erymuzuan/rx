using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
   public static class OperationEndpointExtension
    {

        private static readonly string[] ImportDirectives =
       {
            typeof(Entity).Namespace,
            typeof(Int32).Namespace ,
            typeof(Task<>).Namespace,
            typeof(List<>).Namespace,
            typeof(Enumerable).Namespace ,
            //typeof(XmlAttributeAttribute).Namespace,
            "System.Web.Http",
            "Bespoke.Sph.WebApi"
        };

        public static async Task<IEnumerable<Class>> GenerateSourceAsync(this OperationEndpoint endpoint,
            EntityDefinition entityDefinition)
        {
            var sources = new List<Class>();

            var controller = endpoint.GenerateController(entityDefinition);
            var folder = $"{nameof(OperationEndpoint)}.{endpoint.Entity}.{endpoint.Name}";
            var assemblyInfo = await AssemblyInfoClass.GenerateAssemblyInfoAsync(endpoint, true, folder);

            sources.Add(controller);
            sources.Add(assemblyInfo.ToClass());
            return sources;
        }

        public static Task<RxCompilerResult> CompileAsync(this OperationEndpoint endpoint, 
            EntityDefinition entityDefinition,
            string controllerSource, 
            string assemblyInfoSource)
        {

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var output = $"{ConfigurationManager.CompilerOutputPath}\\{endpoint.AssemblyName}";
                var parameters = new CompilerParameters
                {
                    OutputAssembly = output,
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };

                parameters.AddReference(typeof(Entity),
                    typeof(int),
                    typeof(INotifyPropertyChanged),
                    typeof(Expression<>),
                    typeof(XmlAttributeAttribute),
                    typeof(SmtpClient),
                    typeof(HttpClient),
                    typeof(XElement),
                   // typeof(HttpResponseBase),
                    typeof(ConfigurationManager));
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\System.Web.Http.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\webapi.common.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll");
                parameters.ReferencedAssemblies.Add(ConfigurationManager.CompilerOutputPath + $@"\{ConfigurationManager.ApplicationName}.{endpoint.Entity}.dll");

                endpoint.ReferencedAssemblyCollection.ForEach(x => parameters.ReferencedAssemblies.Add(x.Location));

                var result = provider.CompileAssemblyFromFile(parameters, controllerSource, assemblyInfoSource);
                var cr = new RxCompilerResult
                {
                    Result = true,
                    Output = System.IO.Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                var errors = from CompilerError x in result.Errors
                             select new BuildError(endpoint.WebId, x.ErrorText)
                             {
                                 Line = x.Line,
                                 FileName = x.FileName
                             };
                cr.Errors.AddRange(errors);
                return Task.FromResult(cr);
            }
        }

        private static Class GenerateController(this OperationEndpoint endpoint,EntityDefinition ed)
        {
            var controller = new Class
            {
                Name = endpoint.TypeName,
                IsPartial = true,
                FileName = endpoint.TypeName,
                BaseClass = "BaseApiController",
                Namespace = endpoint.CodeNamespace
            };
            controller.ImportCollection.ClearAndAddRange(ImportDirectives);
            controller.ImportCollection.Add("System.Net");
            controller.ImportCollection.Add("System.Net.Http");
            controller.ImportCollection.Add("Newtonsoft.Json.Linq");
            controller.ImportCollection.Add(ed.CodeNamespace);
            controller.AttributeCollection.Add($"[RoutePrefix(\"api/{endpoint.Resource}\")]");
            
            controller.PropertyCollection.Add(new Property { Name = "CacheManager", Type = typeof(ICacheManager) });
            controller.CtorCollection.Add($"public {endpoint.TypeName}() {{ this.CacheManager = ObjectBuilder.GetObject<ICacheManager>(); }}");
            controller.AddProperty($@"private readonly static string EntityDefinitionSource = $""{{ConfigurationManager.SphSourceDirectory}}\\{nameof(EntityDefinition)}\\{ed.Id}.json"";");
            controller.AddProperty($@"private readonly static string EndpointSource = $""{{ConfigurationManager.SphSourceDirectory}}\\{nameof(OperationEndpoint)}\\{endpoint.Id}.json"";");

            if (endpoint.IsHttpPost)
                controller.MethodCollection.Add(endpoint.GeneratePostAction(ed));
            if (endpoint.IsHttpPatch)
                controller.MethodCollection.Add(endpoint.GeneratePatchAction(ed));

            if (endpoint.IsHttpPut)
                controller.MethodCollection.Add(endpoint.GeneratePutAction(ed));

            if (endpoint.IsHttpDelete)
                controller.MethodCollection.Add(endpoint.GenerateDeleteAction(ed));


            return controller;


        }


        public static Method GeneratePostAction(this OperationEndpoint endpoint, EntityDefinition ed)
        {
            if (!endpoint.IsHttpPost) return null;
            var post = new Method { Name = $"Post{endpoint.Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            post.AttributeCollection.Add("[HttpPost]");
            post.AttributeCollection.Add($"[PostRoute(\"{endpoint.Route}\")]");

            var edArg = new MethodArg { Name = "ed", Type = typeof(EntityDefinition) };
            edArg.AttributeCollection.Add($"[SourceEntity(\"{ed.Id}\")]");
            post.ArgumentCollection.Add(edArg);


            var endpointArg = new MethodArg { Name = "endpoint", Type = typeof(OperationEndpoint) };
            endpointArg.AttributeCollection.Add($"[SourceEntity(\"{endpoint.Id}\")]");
            post.ArgumentCollection.Add(endpointArg);

            var body = new MethodArg { Name = "item", TypeName = ed.Name };
            body.AttributeCollection.Add("[FromBody]");
            post.ArgumentCollection.Add(body);


            post.AppendLine("           var context = new SphDataContext();");

            post.AppendLine("      item.Id = Strings.GenerateId(); ");

            var rules = endpoint.GenerateRulesCode();
            post.AppendLine(rules);


            var setterCode = endpoint.GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                post.AppendLine(setterCode);

            post.AppendLine($@"
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{endpoint.Name}"");
            }}

            var result = new {{success = true, status=""OK"", id = item.Id, 
                      _links = new {{ 
                            rel = ""self"",
                            href=$""{{ConfigurationManager.BaseUrl}}/api/{ed.Plural.ToLowerInvariant()}/{{item.Id}}""
                        }}
                }};
            return Accepted($""{{ConfigurationManager.BaseUrl}}/api/{ed.Plural.ToLowerInvariant()}/{{item.Id}}"", result);");

            return post;
        }

        public static Method GeneratePatchAction(this OperationEndpoint endpoint, EntityDefinition ed)
        {
            if (!endpoint.IsHttpPatch) return null;

            var patch = new Method { Name = $"Patch{endpoint.Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            patch.AttributeCollection.Add("[HttpPatch]");
            patch.AttributeCollection.Add($"[PatchRoute(\"{endpoint.Route}\")]");

            var edArg = new MethodArg { Name = "ed", Type = typeof(EntityDefinition) };
            edArg.AttributeCollection.Add($"[SourceEntity(\"{ed.Id}\")]");
            patch.ArgumentCollection.Add(edArg);


            var endpointArg = new MethodArg { Name = "endpoint", Type = typeof(OperationEndpoint) };
            endpointArg.AttributeCollection.Add($"[SourceEntity(\"{endpoint.Id}\")]");
            patch.ArgumentCollection.Add(endpointArg);

            patch.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string) });
            var body = new MethodArg { Name = "body", Type = typeof(string) };
            body.AttributeCollection.Add("[RawBody]");
            patch.ArgumentCollection.Add(body);

            if (endpoint.IsConflictDetectionEnabled)
            {
                patch.ArgumentCollection.Add(new MethodArg { Name = "etag", TypeName = "ETag", AttributeCollection = { "[IfMatch]" } });
                patch.ArgumentCollection.Add(new MethodArg { Name = "unmodifiedSince", TypeName = "UnmodifiedSinceHeader", AttributeCollection = { "[UnmodifiedSince]" } });
            }

            patch.AppendLine("var context = new SphDataContext();");

            patch.Append(
                $@"
            var repos = ObjectBuilder.GetObject<IReadOnlyRepository<{ed.Name}>>();
            var lo = await repos.LoadOneAsync(id);
            var item = lo.Source;
            if(null == item) return NotFound(""Cannot find any {ed.Name} with Id "" + id);");

            patch.Append(endpoint.GenerateConflicDetectionCode());

            patch.AppendLine("var jo = JObject.Parse(body);");
            patch.AppendLine("var missingValues = new List<string>();");
            var count = 0;
            foreach (var prop in endpoint.PatchPathCollection)
            {
                count++;
                var member = ed.GetMember(prop.Path);
                if (null == member) throw new InvalidOperationException($"Cannot find member with path {prop}");
                patch.AppendLine($"            var val{count} = jo.SelectToken(\"$.{prop}\");");
                if (prop.IsRequired)
                {
                    patch.AppendLine($"             if(null == val{count})");
                    patch.AppendLine($"                 missingValues.Add(\"{prop.Path}\");");
                    patch.AppendLine("             else");
                    patch.AppendLine($"                 item.{prop} = val{count}.Value<{member.GetMemberTypeName()}>();");
                }
                else
                {
                    patch.AppendLine($"      item.{prop} =  null != val{count} ? val{count}.Value<{member.GetMemberTypeName()}>() : {prop.DefaultValue};");
                }
            }
            patch.AppendLine();
            patch.AppendLine("      if(missingValues.Count > 0)");
            patch.AppendLine("      {");
            patch.AppendLine("          return Invalid((HttpStatusCode)422, missingValues.ToArray());");
            patch.AppendLine("      }");

            var rules = endpoint.GenerateRulesCode();
            patch.AppendLine(rules);


            var setterCode = endpoint.GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                patch.AppendLine(setterCode);

            patch.AppendLine($@"
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{endpoint.Name}"");
            }}
            var result = new {{
                               success = true, 
                               status=""OK"", 
                               id = item.Id, 
                               _link = new {{
                                    rel = ""self"",
                                    href = $""{{ConfigurationManager.BaseUrl}}/api/{ed.Plural.ToLowerInvariant()}/{{item.Id}}""
                                }}
                            }};
            return Accepted(result);");

            return patch;
        }

        private static string GenerateConflicDetectionCode(this OperationEndpoint endpoint)
        {
            if (!endpoint.IsConflictDetectionEnabled) return string.Empty;
            var code = new StringBuilder();

            code.Append(
                @"               
                if (!etag.IsMatch(lo.Version, unmodifiedSince, lo.Source.ChangedDate, false))
                {
                    return Invalid((HttpStatusCode)428, new { message =""This request is required to be conditional;try using 'If-Match' or 'If-Unmodified-Since', or may be your resource is out of date""});
                }
                ");
            return code.ToString();
        }

        public static string GetPutRoute(this OperationEndpoint endpoint)
        {
            var route = endpoint.Route ?? "";
            if (!route.StartsWith("~/"))
            {
                if (!route.Contains("{id"))
                    route = $"{{id}}{(string.IsNullOrWhiteSpace(endpoint.Route) ? "" : "/")}{route}";
            }
            return route;
        }

        public static Method GeneratePutAction(this OperationEndpoint endpoint, EntityDefinition ed)
        {
            if (!endpoint.IsHttpPut) return null;
            var route = endpoint.GetPutRoute();

            var put = new Method { Name = $"Put{endpoint.Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            put.AttributeCollection.Add("[HttpPut]");
            put.AttributeCollection.Add($"[PutRoute(\"{route}\")]");

            var edArg = new MethodArg { Name = "ed", Type = typeof(EntityDefinition) };
            edArg.AttributeCollection.Add($"[SourceEntity(\"{ed.Id}\")]");
            put.ArgumentCollection.Add(edArg);


            var endpointArg = new MethodArg { Name = "endpoint", Type = typeof(OperationEndpoint) };
            endpointArg.AttributeCollection.Add($"[SourceEntity(\"{endpoint.Id}\")]");
            put.ArgumentCollection.Add(endpointArg);

            if (endpoint.IsConflictDetectionEnabled)
            {
                put.ArgumentCollection.Add(new MethodArg { Name = "etag", TypeName = "ETag", AttributeCollection = { "[IfMatch]" } });
                put.ArgumentCollection.Add(new MethodArg { Name = "unmodifiedSince", TypeName = "UnmodifiedSinceHeader", AttributeCollection = { "[UnmodifiedSince]" } });
            }


            var body = new MethodArg { Name = "item", TypeName = endpoint.Entity };
            body.AttributeCollection.Add("[FromBody]");
            put.ArgumentCollection.Add(body);
            put.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string), Default = "null" });


            put.AppendLine("           var context = new SphDataContext(); ");
            put.AppendLine(
                $@"
            var repos = ObjectBuilder.GetObject<IReadOnlyRepository<{ed.Name}>>();
            var lo = await repos.LoadOneAsync(id);
            var baru = null == lo.Source;
            item.Id = id ?? Strings.GenerateId();
            ");

            if (endpoint.IsConflictDetectionEnabled)
            {
                put.AppendLine(@"    
            if(!baru){");
                put.Append(endpoint.GenerateConflicDetectionCode());
                put.AppendLine(@"
            }");
            }

            var rules = endpoint.GenerateRulesCode();
            put.AppendLine(rules);


            var setterCode = endpoint.GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                put.AppendLine(setterCode);

            put.AppendLine($@"
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{endpoint.Name}"");
            }}

            var result = new {{
                               success = true, 
                               status=""OK"", 
                               id = item.Id, 
                               _link = new {{
                                    rel = ""self"",
                                    href = $""{{ConfigurationManager.BaseUrl}}/api/{endpoint.Resource}/{{item.Id}}""
                                }}
                            }};
            if(baru) return Accepted( result._link.href, result);
            return Accepted(result);");

            return put;
        }



        public static Method GenerateDeleteAction(this OperationEndpoint endpoint, EntityDefinition ed)
        {
            if (!endpoint.IsHttpDelete) return null;

            var route = endpoint.GetDeleteRoute();


            var delete = new Method { Name = $"Delete{endpoint.Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            delete.AttributeCollection.Add("[HttpDelete]");
            delete.AttributeCollection.Add($@"[DeleteRoute(""{route}"")]");

            var edArg = new MethodArg { Name = "ed", Type = typeof(EntityDefinition) };
            edArg.AttributeCollection.Add($"[SourceEntity(\"{ed.Id}\")]");
            delete.ArgumentCollection.Add(edArg);


            var endpointArg = new MethodArg { Name = "endpoint", Type = typeof(OperationEndpoint) };
            endpointArg.AttributeCollection.Add($"[SourceEntity(\"{endpoint.Id}\")]");
            delete.ArgumentCollection.Add(endpointArg);

            delete.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string) });


            delete.AppendLine(
                $@"
            var repos = ObjectBuilder.GetObject<IReadOnlyRepository<{ed.Name}>>();
            var item = (await repos.LoadOneAsync(id)).Source;
            if(null == item)
                return NotFound();");

            var rules = endpoint.GenerateRulesCode();
            delete.AppendLine(rules);

            delete.Append(
              $@"
            var context = new SphDataContext();
            using(var session = context.OpenSession())
            {{
                session.Delete(item);
                await session.SubmitChanges(""{endpoint.Name}"");
            }}
            return Accepted(new {{success = true, status=""OK"", id = item.Id}});");

            return delete;
        }

        public static string GetDeleteRoute(this OperationEndpoint endpoint)
        {
            var route = !string.IsNullOrWhiteSpace(endpoint.Route) ? $"{endpoint.Route.ToLowerInvariant()}/" : "";
            if (route.Contains("{id")) return route;
            return route + "{id}";
        }

        private static  string GetSetterCode(this OperationEndpoint endpoint, EntityDefinition ed)
        {
            if (endpoint.SetterActionChildCollection.Count == 0) return string.Empty;

            var code = new StringBuilder();
            // now the setter

            code.AppendLinf("           var rc = new RuleContext(item);");
            var count = 0;
            foreach (var act in endpoint.SetterActionChildCollection)
            {
                count++;
                code.AppendLine($"//{act.Field.Name}");
                code.AppendLine(act.GenerateCode(ed, "item", count));
            }

            return code.ToString();

        }

        private static string GenerateRulesCode(this OperationEndpoint endpoint)
        {
            return $@"
            var businessRuleResult = await endpoint.ValidateAsync<{endpoint.Entity}>(item, ed);
            if (!businessRuleResult.Success)
            {{
                return Invalid(businessRuleResult);
            }}";
        }



    }
}
