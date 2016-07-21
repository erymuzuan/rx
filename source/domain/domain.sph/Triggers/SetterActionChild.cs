﻿using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class SetterActionChild : DomainObject
    {
        public string GenerateCode(EntityDefinition ed, string itemName, int count)
        {
            var act = this;
            var member = ed.GetMember(act.Path);
            var sc = act.Field.GenerateCode();
            if (!string.IsNullOrWhiteSpace(sc) && sc.EndsWith(";"))
            {
                return GenerateCodeWithFieldWithCode(sc, member, count);
            }
            if (!string.IsNullOrWhiteSpace(sc))
            {
                return $"          item.{Path} = {sc};";
            }

            return $@"           
            var setter{count} = endpoint.SetterActionChildCollection.Single(a => a.WebId == ""{act.WebId}"");
            item.{act.Path} = ({member.GetMemberTypeName()})setter{count}.Field.GetValue(rc);";

        }

        private string GenerateCodeWithFieldWithCode(string sc, Member member, int count)
        {
            var code = new StringBuilder();
            if (sc.EndsWith(";"))
            {
                var simpleMember = (SimpleMember)member;
                var asyncLambda = sc.Contains("await ");
                var assignment = $"f{count}()";
                if (asyncLambda)
                {
                    code.AppendLine($"          Func<Task<{simpleMember.Type.ToCSharp()}>> f{count}Async = async () =>{{{sc}}};");
                    assignment = ($"await f{count}Async()");
                }
                else
                {
                    code.AppendLine($"          Func<{simpleMember.Type.ToCSharp()}> f{count} = () =>{{{sc}}};");

                }

                if (member.AllowMultiple)
                {
                    code.AppendLine($"          item.{Path}.Clear();");
                    code.AppendLine($"          item.{Path}.AddRange({assignment});");
                }
                else
                {
                    code.AppendLine($"          item.{Path} = {assignment};");
                }
                return code.ToString();
            }
            code.AppendLine($"          item.{Path} = {sc};");
            return code.ToString();

        }
    }
}