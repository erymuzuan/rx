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
    public partial class OperationEndpoint : Entity, IEntityDefinitionAsset
    {


        public async Task<BuildValidationResult> ValidateBuildAsync(EntityDefinition ed)
        {
            if (null == this.BuildDiagnostics)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.BuildDiagnostics)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(QueryEndpoint)}.BuildDiagnostics");

            var result = new BuildValidationResult();
            var errors = new ObjectCollection<BuildError>();
            var everybody = this.Permissions.Contains("Everybody");
            var anonymous = this.Permissions.Contains("Anonymous");
            var roles = this.Permissions.Any(s => s != "Everybody" && s != "Anonymous");
            if (everybody && anonymous)
                errors.Add(new BuildError(this.WebId, $"[Operation] \"{this.Name}\" cannot have anonymous and everybody at the same time"));

            if (everybody && roles)
                errors.Add(new BuildError(this.WebId, $"[Operation] \"{this.Name}\" cannot have everybody and other roles at the same time"));

            if (anonymous && roles)
                errors.Add(new BuildError(this.WebId, $"[Operation] \"{this.Name}\" cannot have anonymous and other role set at the same time"));

            var errorTasks = this.BuildDiagnostics.Select(d => d.ValidateErrorsAsync(this, ed));
            var errors2 = (await Task.WhenAll(errorTasks)).SelectMany(x => x.ToArray());

            var warningTasks = this.BuildDiagnostics.Select(d => d.ValidateWarningsAsync(this, ed));
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x.ToArray());

            result.Errors.AddRange(errors);
            result.Errors.AddRange(errors2);
            result.Warnings.AddRange(warnings);


            result.Result = result.Errors.Count == 0;

            return result;
        }

        public Method GeneratePostAction(EntityDefinition ed)
        {
            if (!IsHttpPost) return null;
            var post = new Method { Name = $"Post{Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            post.AttributeCollection.Add("[HttpPost]");
            post.AttributeCollection.Add($"[Route(\"{Route}\")]");

            var authorize = GenerateAuthorizeAttribute();
            if (!string.IsNullOrWhiteSpace(authorize))
                post.AttributeCollection.Add(authorize);


            var body = new MethodArg { Name = "item", TypeName = ed.Name };
            body.AttributeCollection.Add("[FromBody]");
            post.ArgumentCollection.Add(body);


            if (this.Rules.Any() || this.SetterActionChildCollection.Any())
                post.AppendLine(GetEntityDefinitionCode(ed));
            else
                post.AppendLine("           var context = new SphDataContext();");

            post.AppendLine("      item.Id = Strings.GenerateId(); ");

            var rules = GenerateRulesCode();
            if (!string.IsNullOrWhiteSpace(rules))
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
            return Json(new {{success = true, status=""OK"", id = item.Id, 
                      _links = new {{ 
                            rel = ""self"",
                            href=$""{{ConfigurationManager.BaseUrl}}/api/{ed.Plural.ToLowerInvariant()}/{{ item.Id}}""
                        }}
                }});");

            return post;
        }
        public Method GeneratePatchAction(EntityDefinition ed)
        {
            if (!IsHttpPatch) return null;

            var patch = new Method { Name = $"Patch{Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            patch.AttributeCollection.Add("[HttpPatch]");
            patch.AttributeCollection.Add($"[Route(\"{Route}\")]");

            var authorize = GenerateAuthorizeAttribute();
            if (!string.IsNullOrWhiteSpace(authorize))
                patch.AttributeCollection.Add(authorize);


            patch.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string) });
            var body = new MethodArg { Name = "body", Type = typeof(string) };
            body.AttributeCollection.Add("[RawBody]");
            patch.ArgumentCollection.Add(body);

            if (this.IsConflictDetectionEnabled)
            {
                patch.ArgumentCollection.Add(new MethodArg {Name = "etag", TypeName = "ETag", AttributeCollection = { "[IfMatch]" } });
                patch.ArgumentCollection.Add(new MethodArg {Name = "modifiedSince", TypeName = "DateTime?", AttributeCollection = { "[ModifiedSince]" } });
            }


            var enabledGetEntityDefinition = this.Rules.Any() || this.SetterActionChildCollection.Any();
            if (enabledGetEntityDefinition)
                patch.AppendLine(GetEntityDefinitionCode(ed));
            else
                patch.AppendLine("var context = new SphDataContext();");

            patch.Append(
                $@"
            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{ed.Name}>>();
            var lo = await repos.LoadOneAsync(id);
            var item = lo.Source;
            if(null == item) return NotFound(""Cannot find any {ed.Name} with Id "" + id);
            
            var changedDate = item.ChangedDate;");

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
                    patch.AppendLine($"             else");
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
            patch.AppendLine("          return Invalid((HttpStatusCode)405, missingValues.ToArray());");
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
            return Json(result);");

            return patch;
        }

        private string GenerateConflicDetectionCode()
        {
            if (!this.IsConflictDetectionEnabled) return string.Empty;
            var code = new StringBuilder();

            code.Append(
                $@"        
                if ($""{{modifiedSince:s}}"" != lo.Source.ChangedDate.ToString(""s""))
                {{
                    return Invalid(new {{ message =""Your If-Modified-Since header is out of date""}});
                }}
                
                if (etag?.Tag != lo.Version)
                {{
                    return Invalid(new {{ message =""Your If-Match header is out of date""}});
                }}
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
            put.AttributeCollection.Add($"[Route(\"{route}\")]");

            var authorize = GenerateAuthorizeAttribute();
            if (!string.IsNullOrWhiteSpace(authorize))
                put.AttributeCollection.Add(authorize);


            var body = new MethodArg { Name = "body", Type = typeof(string) };
            body.AttributeCollection.Add("[RawBody]");
            put.ArgumentCollection.Add(body);
            put.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string), Default = "null" });


            put.AppendLine("           var context = new SphDataContext();");
            if (this.Rules.Any() || this.SetterActionChildCollection.Any())
                put.AppendLine(GetEntityDefinitionCode(ed));

            put.AppendLine(
                $@"
            var repos = ObjectBuilder.GetObject<IRepository<{ed.Name}>>();
            var item = await repos.LoadOneAsync(id);
            var baru = null == item;
            if(baru)
            {{
                item = body.DeserializeFromJson<{ed.Name}>();
                if (!string.IsNullOrWhiteSpace(item.Id))
                    item.Id = id ?? System.Guid.NewGuid().ToString();
            }}
            else
            {{
                var jo = JObject.Parse(body);
            ");

            foreach (var path in this.PatchPathCollection)
            {
                var member = ed.GetMember(path.Path);
                if (null == member) throw new InvalidOperationException($"Cannot find member with path {path}");
                put.AppendLine($"            item.{path.Path} = jo.SelectToken(\"$.{path}\").Value<{member.GetMemberTypeName()}>();");
            }
            put.AppendLine(@"
            }");

            var rules = GenerateRulesCode();
            if (!string.IsNullOrWhiteSpace(rules))
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
                                    href = $""{{ConfigurationManager.BaseUrl}}/api/{ed.Plural.ToLowerInvariant()}/{{item.Id}}""
                                }}
                            }};
            if(baru) return Created( result._link.href, result);
            return Ok(result);");

            return put;
        }


        public Method GenerateDeleteAction(EntityDefinition ed)
        {
            if (!IsHttpDelete) return null;

            var route = !string.IsNullOrWhiteSpace(this.Route) ? $"{this.Route.ToLowerInvariant()}/" : "";

            var delete = new Method { Name = $"Delete{Name}", ReturnTypeName = "Task<IHttpActionResult>", AccessModifier = Modifier.Public };
            delete.AttributeCollection.Add("[HttpDelete]");
            delete.AttributeCollection.Add($"[Route(\"{route}{{id}}\")]");
            delete.ArgumentCollection.Add(new MethodArg { Name = "id", Type = typeof(string) });

            var authorize = GenerateAuthorizeAttribute();
            if (!string.IsNullOrWhiteSpace(authorize))
                delete.AttributeCollection.Add(authorize);


            delete.AppendLine(
                $@"
            var repos = ObjectBuilder.GetObject<IReadonlyRepository<{ed.Name}>>();
            var item = (await repos.LoadOneAsync(id)).Source;
            if(null == item)
                return NotFound();");

            if (this.Rules.Any())
            {
                delete.AppendLine(GetEntityDefinitionCode(ed));
                var rules = GenerateRulesCode();
                delete.AppendLine(rules);
            }
            else
            {
                delete.AppendLine("var context = new SphDataContext();");
            }

            delete.Append(
              $@"
            using(var session = context.OpenSession())
            {{
                session.Delete(item);
                await session.SubmitChanges(""{this.Name}"");
            }}
            return Ok(new {{success = true, status=""OK"", id = item.Id}});");

            return delete;
        }

        private string GetSetterCode(EntityDefinition ed)
        {
            if (this.SetterActionChildCollection.Count == 0) return string.Empty;

            var code = new StringBuilder();
            // now the setter
            code.AppendLine($@"           var operation = CacheManager.Get<OperationEndpoint>(""{Id}"");");
            code.AppendLine("           if(null == operation)");
            code.AppendLine("           {");
            code.AppendLine($@"           operation = await context.LoadOneAsync<OperationEndpoint>(x => x.Id ==""{Id}"");");
            code.AppendLine($@"           CacheManager.Insert<OperationEndpoint>(""{Id}"", operation, EndpointSource);");
            code.AppendLine("           }");
            code.AppendLinf("           var rc = new RuleContext(item);");
            var count = 0;
            foreach (var act in this.SetterActionChildCollection)
            {
                count++;
                var member = ed.GetMember(act.Path);
                var sc = act.Field.GenerateCode();
                if (!string.IsNullOrWhiteSpace(sc))
                {
                    code.AppendLine($"          item.{act.Path} = {sc};");
                    continue;
                }
                code.AppendLine($"           var setter{count} = operation.SetterActionChildCollection.Single(a => a.WebId == \"{act.WebId}\");");
                code.AppendLine($"           item.{act.Path} = ({member.GetMemberTypeName()})setter{count}.Field.GetValue(rc);");
            }

            return code.ToString();

        }

        private string GetEntityDefinitionCode(EntityDefinition ed)
        {
            var code = new StringBuilder();
            code.AppendLine($"var context = new SphDataContext();");
            code.AppendLine($"var ed = CacheManager.Get<EntityDefinition>(\"{ed.Id}\");");
            code.AppendLine("if(null == ed)");
            code.AppendLine("{");
            code.AppendLine($"   ed = await context.LoadOneAsync<EntityDefinition>(d => d.Id == \"{ed.Id}\");");
            code.AppendLine($"   CacheManager.Insert(\"{ed.Id}\", ed, EntityDefinitionSource);");
            code.AppendLine("}");
            return code.ToString();

        }
        private string GenerateRulesCode()
        {
            if (this.Rules.Count == 0) return string.Empty;

            var code = new StringBuilder();

            code.AppendLine("           var brokenRules = new ObjectCollection<ValidationResult>();");
            var count = 0;
            foreach (var rule in this.Rules)
            {
                count++;
                code.AppendLine($@"
            var @rules{count} = ed.BusinessRuleCollection.Where(b => b.Name == ""{rule}"");
            var @result{count} = item.ValidateBusinessRule(@rules{count});
            if(!@result{count}.Success){{
                brokenRules.Add(@result{count});
            }}
");
            }
            code.AppendLine("           if( brokenRules.Count > 0) ");
            code.AppendLine("           {");
            code.AppendLine("               return Invalid(brokenRules.ToArray());");
            code.AppendLine("           }");
            return code.ToString();
        }




        [ImportMany(typeof(IBuildDiagnostics))]
        [JsonIgnore]
        [XmlIgnore]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        private string GenerateAuthorizeAttribute()
        {
            var code = new StringBuilder();
            var everybody = this.Permissions.Contains("Everybody");
            var anonymous = this.Permissions.Contains("Anonymous");
            if (everybody)
                code.AppendLine("       [Authorize]");

            if (!everybody && !anonymous &&
                string.Join(",", this.Permissions.Where(s => s != "Everybody" && s != "Anonymous")).Length > 0)
                code.AppendLinf("       [Authorize(Roles=\"{0}\")]",
                    string.Join(",", this.Permissions.Where(s => s != "Everybody" && s != "Anonymous")));
            return code.ToString();
        }

        [JsonIgnore]
        string IEntityDefinitionAsset.Icon { get; } = "fa fa-cloud-upload";
        [JsonIgnore]
        string IEntityDefinitionAsset.Url { get; } = "operation.endpoint.designer/{id}";

        public void AddRules(string rule)
        {
            this.Rules.Add(rule);
        }
    }
}