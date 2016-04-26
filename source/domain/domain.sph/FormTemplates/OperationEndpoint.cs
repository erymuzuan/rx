using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class OperationEndpoint : Entity
    {


        public async Task<BuildValidationResult> ValidateBuildAsync(EntityDefinition ed)
        {
            if (null == this.BuildDiagnostics)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.BuildDiagnostics)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(QueryEndpoint)}.BuildDiagnostics");

            var result = new BuildValidationResult();

            var errorTasks = this.BuildDiagnostics.Select(d => d.ValidateErrorsAsync(this, ed));
            var errors = (await Task.WhenAll(errorTasks)).SelectMany(x => x.ToArray());

            var warningTasks = this.BuildDiagnostics.Select(d => d.ValidateWarningsAsync(this, ed));
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x.ToArray());

            result.Errors.AddRange(errors);
            result.Warnings.AddRange(warnings);


            result.Result = result.Errors.Count == 0;

            return result;
        }

        public Method GeneratePostAction(EntityDefinition ed)
        {
            if (!IsHttpPost) return null;
            var post = new Method { Name = $"Post{Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            post.AttributeCollection.Add("[HttpPost]");
            post.AttributeCollection.Add($"[PostRoute(\"{Route}\")]");

            var edArg = new MethodArg { Name = "ed", Type = typeof(EntityDefinition) };
            edArg.AttributeCollection.Add($"[SourceEntity(\"{ed.Id}\")]");
            post.ArgumentCollection.Add(edArg);


            var endpointArg = new MethodArg { Name = "endpoint", Type = typeof(OperationEndpoint) };
            endpointArg.AttributeCollection.Add($"[SourceEntity(\"{this.Id}\")]");
            post.ArgumentCollection.Add(endpointArg);

            var body = new MethodArg { Name = "item", TypeName = ed.Name };
            body.AttributeCollection.Add("[FromBody]");
            post.ArgumentCollection.Add(body);


            post.AppendLine("           var context = new SphDataContext();");

            post.AppendLine("      item.Id = Strings.GenerateId(); ");

            var rules = GenerateRulesCode();
            post.AppendLine(rules);


            var setterCode = GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                post.AppendLine(setterCode);

            post.AppendLine($@"
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{Name}"");
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
        public Method GeneratePatchAction(EntityDefinition ed)
        {
            if (!IsHttpPatch) return null;

            var patch = new Method { Name = $"Patch{Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            patch.AttributeCollection.Add("[HttpPatch]");
            patch.AttributeCollection.Add($"[PatchRoute(\"{Route}\")]");

            var edArg = new MethodArg { Name = "ed", Type = typeof(EntityDefinition) };
            edArg.AttributeCollection.Add($"[SourceEntity(\"{ed.Id}\")]");
            patch.ArgumentCollection.Add(edArg);


            var endpointArg = new MethodArg { Name = "endpoint", Type = typeof(OperationEndpoint) };
            endpointArg.AttributeCollection.Add($"[SourceEntity(\"{this.Id}\")]");
            patch.ArgumentCollection.Add(endpointArg);

            patch.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string) });
            var body = new MethodArg { Name = "body", Type = typeof(string) };
            body.AttributeCollection.Add("[RawBody]");
            patch.ArgumentCollection.Add(body);

            if (this.IsConflictDetectionEnabled)
            {
                patch.ArgumentCollection.Add(new MethodArg { Name = "etag", TypeName = "ETag", AttributeCollection = { "[IfMatch]" } });
                patch.ArgumentCollection.Add(new MethodArg { Name = "modifiedSince", TypeName = "ModifiedSinceHeader", AttributeCollection = { "[ModifiedSince]" } });
            }

            patch.AppendLine("var context = new SphDataContext();");

            patch.Append(
                $@"
            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{ed.Name}>>();
            var lo = await repos.LoadOneAsync(id);
            var item = lo.Source;
            if(null == item) return NotFound(""Cannot find any {ed.Name} with Id "" + id);");

            patch.Append(this.GenerateConflicDetectionCode());

            patch.AppendLine("var jo = JObject.Parse(body);");
            patch.AppendLine("var missingValues = new List<string>();");
            var count = 0;
            foreach (var prop in this.PatchPathCollection)
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

            var rules = GenerateRulesCode();
            patch.AppendLine(rules);


            var setterCode = GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                patch.AppendLine(setterCode);

            patch.AppendLine($@"
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{Name}"");
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

        private string GenerateConflicDetectionCode()
        {
            if (!this.IsConflictDetectionEnabled) return string.Empty;
            var code = new StringBuilder();

            code.Append(
                @"               
                if (!etag.IsMatch(lo.Version, modifiedSince, lo.Source.ChangedDate, false))
                {
                    return Invalid((HttpStatusCode)428, new { message =""This request is required to be conditional;try using 'If-Match', or may be your resource is out of date""});
                }
                ");
            return code.ToString();
        }

        public Method GeneratePutAction(EntityDefinition ed)
        {
            if (!IsHttpPut) return null;
            var route = this.Route ?? "";
            if (!route.StartsWith("~/"))
            {
                if (!route.Contains("{id"))
                    route = $"{{id:guid}}/{route}";
            }


            var put = new Method { Name = $"Put{Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            put.AttributeCollection.Add("[HttpPut]");
            put.AttributeCollection.Add($"[PutRoute(\"{route}\")]");

            var edArg = new MethodArg { Name = "ed", Type = typeof(EntityDefinition) };
            edArg.AttributeCollection.Add($"[SourceEntity(\"{ed.Id}\")]");
            put.ArgumentCollection.Add(edArg);


            var endpointArg = new MethodArg { Name = "endpoint", Type = typeof(OperationEndpoint) };
            endpointArg.AttributeCollection.Add($"[SourceEntity(\"{this.Id}\")]");
            put.ArgumentCollection.Add(endpointArg);

            if (this.IsConflictDetectionEnabled)
            {
                put.ArgumentCollection.Add(new MethodArg { Name = "etag", TypeName = "ETag", AttributeCollection = { "[IfMatch]" } });
                put.ArgumentCollection.Add(new MethodArg { Name = "modifiedSince", TypeName = "ModifiedSinceHeader", AttributeCollection = { "[ModifiedSince]" } });
            }


            var body = new MethodArg { Name = "item", TypeName = Entity };
            body.AttributeCollection.Add("[FromBody]");
            put.ArgumentCollection.Add(body);
            put.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string), Default = "null" });


            put.AppendLine("           var context = new SphDataContext(); ");
            put.AppendLine(
                $@"
            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{ed.Name}>>();
            var lo = await repos.LoadOneAsync(id);
            var baru = null == lo.Source;
            item.Id = id ?? System.Guid.NewGuid().ToString();
            ");

            if (this.IsConflictDetectionEnabled)
            {
                put.AppendLine(@"    
            if(!baru){");
                put.Append(this.GenerateConflicDetectionCode());
                put.AppendLine(@"
            }");
            }

            var rules = GenerateRulesCode();
            put.AppendLine(rules);


            var setterCode = GetSetterCode(ed);
            if (!string.IsNullOrWhiteSpace(setterCode))
                put.AppendLine(setterCode);

            put.AppendLine($@"
        
            using(var session = context.OpenSession())
            {{
                session.Attach(item);
                await session.SubmitChanges(""{Name}"");
            }}

            var result = new {{
                               success = true, 
                               status=""OK"", 
                               id = item.Id, 
                               _link = new {{
                                    rel = ""self"",
                                    href = $""{{ConfigurationManager.BaseUrl}}/api/{Resource}/{{item.Id}}""
                                }}
                            }};
            if(baru) return Accepted( result._link.href, result);
            return Accepted(result);");

            return put;
        }


        public Method GenerateDeleteAction(EntityDefinition ed)
        {
            if (!IsHttpDelete) return null;

            var route = !string.IsNullOrWhiteSpace(this.Route) ? $"{this.Route.ToLowerInvariant()}/" : "";


            var delete = new Method { Name = $"Delete{Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            delete.AttributeCollection.Add("[HttpDelete]");
            delete.AttributeCollection.Add($"[DeleteRoute(\"{route}{{id}}\")]");

            var edArg = new MethodArg { Name = "ed", Type = typeof(EntityDefinition) };
            edArg.AttributeCollection.Add($"[SourceEntity(\"{ed.Id}\")]");
            delete.ArgumentCollection.Add(edArg);


            var endpointArg = new MethodArg { Name = "endpoint", Type = typeof(OperationEndpoint) };
            endpointArg.AttributeCollection.Add($"[SourceEntity(\"{this.Id}\")]");
            delete.ArgumentCollection.Add(endpointArg);

            delete.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string) });


            delete.AppendLine(
                $@"
            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{ed.Name}>>();
            var item = (await repos.LoadOneAsync(id)).Source;
            if(null == item)
                return NotFound();");

            var rules = GenerateRulesCode();
            delete.AppendLine(rules);

            delete.Append(
              $@"
            var context = new SphDataContext();
            using(var session = context.OpenSession())
            {{
                session.Delete(item);
                await session.SubmitChanges(""{this.Name}"");
            }}
            return Accepted(new {{success = true, status=""OK"", id = item.Id}});");

            return delete;
        }

        private string GetSetterCode(EntityDefinition ed)
        {
            if (this.SetterActionChildCollection.Count == 0) return string.Empty;

            var code = new StringBuilder();
            // now the setter

            code.AppendLinf("           var rc = new RuleContext(item);");
            var count = 0;
            foreach (var act in this.SetterActionChildCollection)
            {
                count++;
                code.AppendLine($"//{act.Field.Name}");
                code.AppendLine(act.GenerateCode(ed, "item", count));
            }

            return code.ToString();

        }

        private string GenerateRulesCode()
        {
            return $@"
            var businessRuleResult = await endpoint.ValidateAsync<{this.Entity}>(item, ed);
            if (!businessRuleResult.Success)
            {{
                return Invalid(businessRuleResult);
            }}";
        }




        [ImportMany(typeof(IBuildDiagnostics))]
        [JsonIgnore]
        [XmlIgnore]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        public void AddRules(string rule)
        {
            this.Rules.Add(rule);
        }

        public Task<ValidationResult> ValidateAsync<T>(T item, EntityDefinition ed) where T : Entity
        {
            var rules = ed.BusinessRuleCollection.Where(x => this.Rules.Contains(x.Name));
            var result = item.ValidateBusinessRule(rules);

            return Task.FromResult(result);
        }
    }
}