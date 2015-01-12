using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    public static class JsonSchemaHelper
    {
        
        public static IEnumerable<Member> GenerateSchema(string json)
        {
            var jo = JObject.Parse(json);
            var ed = new EntityDefinition();

            foreach (var c in jo.Children())
            {
                var p = c as JProperty;
                if (null == p) continue;

                var member = ExtractChildMember(p, null);
                ed.MemberCollection.Add(member);
            }
            return ed.MemberCollection;
        }

        private static Member ExtractChildMember(JToken jt, Member parent)
        {
            var m = new RegexMember();
            var p = jt as JProperty;
            if (null == p)
            {

                // array of objects
                if (jt.Type == JTokenType.Object)
                {
                    foreach (var c in jt.Children())
                    {
                        var cm = ExtractChildMember(c, m);

                        if (parent.MemberCollection.All(x => x.Name != cm.Name))
                            parent.MemberCollection.Add(cm);
                    }
                }
                return m;
            }

            m.Name = p.Name;
            var typeName = p.Value.Type;
            var type = Type.GetType(string.Format("System.{0}, mscorlib", p.Value.Type));
            if (typeName == JTokenType.Integer)
                type = typeof(int);//"
            if (typeName == JTokenType.Null)
                type = typeof(string);//"
            if (typeName == JTokenType.Date)
                type = typeof(DateTime);//"
            if (null != type)
                m.Type = type;

            if (p.Value.Type == JTokenType.Object || p.Value.Type == JTokenType.Array)
            {
                foreach (var c in p.Value.Children())
                {
                    ExtractChildMember(c, m);
                }
            }
            if (null != parent)
                parent.MemberCollection.Add(m);

            return m;
        }
    }
}
